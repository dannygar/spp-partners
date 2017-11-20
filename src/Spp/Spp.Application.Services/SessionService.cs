/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper.FastCrud;
using Spp.Application.Core.Contracts;
using Spp.Application.Core.Models;
using Spp.Application.Services.AutoMapper;
using Spp.Data;
using Spp.Data.Entities;

namespace Spp.Application.Services
{

    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<SessionUser, SessionUserDto>();
            CreateMap<SessionUserDto, SessionUser>();

            CreateMap<LocationDto, Location>();
            CreateMap<Location, LocationDto>();

            CreateMap<Position, PositionDto>();
            CreateMap<Player, PlayerDto>();

            CreateMap<UserDto, User>();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.PlayerInfo, opt => opt.MapFrom(src => src.Player));

            CreateMap<SessionDto, Session>()
                .ForMember(dest => dest.LocationId, opt => opt.Ignore())
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.SessionType))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Scheduled));

            CreateMap<Session, SessionDto>()
                .ForMember(dest => dest.Users, opt => opt.Ignore())
                .ForMember(dest => dest.SessionType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Scheduled, opt => opt.MapFrom(src => src.StartTime));

            CreateMap<SessionType, SessionTypeDto>();
        }
    }

    public class SessionService : SppDbService, ISessionService
    {
        private IDbConnection _db;

        public SessionService(SppDbContext context) : base(context)
        {
            _db = context?.SppDbConnection;
            RegisterEntities();
        }

        public SessionService(BaseDbContext context) : base(context)
        {
            _db = context.DbConnection;
            RegisterEntities();
        }

        public void RegisterEntities()
        {
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;

            //Initialize static mapping
            AutoMapperConfiguration.Configure();
        }

        public async Task<List<SessionDto>> GetSessionsByDate(DateTime sessionDate)
        {
            var sessionsList = new List<SessionDto>();

            //Get the session
            var sessions = await _db.FindAsync<SessionUser>(query => query
                .Include<Session>(join => join.InnerJoin()
                .Where($"{nameof(Session.StartTime):C} = @sessionDate"))
                .Include<Location>(join => join.InnerJoin())
                .Include<User>(join => join.InnerJoin())
                .WithParameters(new { sessionDate }));

            //Find the navigation properties (Users, Location) for each session in the returning list
            foreach (var dbSessions in sessions.GroupBy(x => x.SessionId))
            {
                var session = Mapper.Map<SessionDto>(dbSessions.First().Session);
                session.Users = new List<UserDto>();

                foreach (var sessionDb in dbSessions)
                {
                    if ((bool)sessionDb.User?.isActive && (bool)sessionDb.User?.isEnabled)
                    {
                        //Map to the user dto
                        var user = Mapper.Map<UserDto>(sessionDb.User);

                        //Get the player's info
                        if ((UserRoles)user.RoleId == UserRoles.Player)
                        {
                            var players = await _db.FindAsync<Player>(query => query
                                .Where($"{nameof(Player.UserId):C} = @userId")
                                .Include<Position>(join => join.InnerJoin())
                                .WithParameters(new { userId = user.Id }));
                            var player = players?.FirstOrDefault();

                            //Assign player's info, if available
                            user.PlayerInfo = (player != null) ? Mapper.Map<PlayerDto>(player) : null;
                        }

                        ((IList)session.Users).Add(user);

                    }
                }

                sessionsList.Add(session);
            }

            return sessionsList;
        }

        public async Task<List<SessionDto>> GetSessionsRange(DateTime fromDate, DateTime toDate)
        {
            var sessionsList = new List<SessionDto>();

            //Get the sessions
            var sessions = await _db.FindAsync<SessionUser>(query => query
                .Include<Session>(join => join.InnerJoin()
                .Where($"{nameof(Session.StartTime):C} >= @fromDate and {nameof(Session.StartTime):C} <= @toDate"))
                .Include<Location>(join => join.InnerJoin())
                .Include<User>(join => join.InnerJoin())
                .WithParameters(new { fromDate, toDate }));

            //Find the navigation properties (Users, Location) for each session in the returning list
            foreach (var dbSessions in sessions.GroupBy(x => x.SessionId))
            {
                var session = Mapper.Map<SessionDto>(dbSessions.First().Session);
                session.Users = new List<UserDto>();

                foreach (var sessionDb in dbSessions)
                {
                    if ((bool)sessionDb.User?.isActive && (bool)sessionDb.User?.isEnabled)
                        ((IList)session.Users).Add(Mapper.Map<UserDto>(sessionDb.User));
                }

                sessionsList.Add(session);
            }

            return sessionsList;
        }

        public async Task<List<SessionTypeDto>> GetSessionTypes()
        {
            var dbRecs = await _db.FindAsync<SessionType>();

            return dbRecs.Select(rec => Mapper.Map<SessionTypeDto>(rec)).ToList();
        }

        public async Task<bool> CreateSessionUser(SessionUserDto sessionUserDto)
        {
            //Map to the Data Entity object
            var recDb = Mapper.Map<SessionUser>(sessionUserDto);

            //Insert into Message table
            await _db.InsertAsync(recDb);

            return recDb.Id > 0;
        }

        public async Task<int> CreateSession(SessionDto sessionDto)
        {
            //Get the corresponding location Id
            var dbLocation = await _db.GetAsync(new Location() { Id = (int)sessionDto.Location?.Id });

            //Map to the Data Entity object
            var recDb = Mapper.Map<Session>(sessionDto);
            recDb.LocationId = dbLocation?.Id;

            //Insert into Message table
            await _db.InsertAsync(recDb);

            return recDb.Id;
        }

        public async Task<bool> UpdateSession(SessionDto sessionDto)
        {
            //Map to the DB object
            var newRec = Mapper.Map<Session>(sessionDto);

            //Update the database
            newRec.LocationId = sessionDto.Location.Id;
            var result = await _db.UpdateAsync(newRec);

            //Get current session
            var currentSession = await GetSession(sessionDto.Id);

            //Remove all users from this session
            await RemoveUsersFromSession(currentSession.Id);

            //Get the list of users belonging to this session and re-insert them into the session
            foreach (var user in sessionDto.Users)
            {
                await AddUserToSession(sessionDto.Id, user.Id);
            }

            return result;
        }

        public async Task<bool> DeleteSession(int sessionId)
        {
            //Get the questionnaire by its Id
            var dbRec = await _db.GetAsync(new Session() { Id = sessionId });
            //Delete the the record from the DB
            return await _db.DeleteAsync(dbRec);
        }

        public async Task<SessionDto> GetSession(int sessionId)
        {
            //Get the session
            var sessions = await _db.FindAsync<SessionUser>(query => query
                .Include<Session>(join => join.InnerJoin()
                    .Where($"{nameof(Session.Id):C} = @sessionId"))
                .Include<Location>(join => join.InnerJoin())
                .Include<User>(join => join.InnerJoin())
                .WithParameters(new { sessionId }));

            var sessionDb = sessions?.ToList();
            if (sessionDb == null || !sessionDb.Any()) return null;

            var session = Mapper.Map<SessionDto>(sessionDb.First().Session);
            session.Users = new List<UserDto>();

            foreach (var sessionUser in sessionDb)
            {
                if ((bool)sessionUser.User?.isActive && (bool)sessionUser.User?.isEnabled)
                {
                    var user = Mapper.Map<UserDto>(sessionUser.User);
                    ((List<UserDto>)session.Users).Add(user);
                }
            }

            return session;
        }

        public async Task AddUserToSession(int sessionId, int userId)
        {
            var sessionUser = new SessionUser()
            {
                SessionId = sessionId,
                UserId = userId
            };

            await _db.InsertAsync(sessionUser);
        }

        public async Task RemoveUsersFromSession(int sessionId)
        {
            //Delete all the users associated with this session
            await _db.BulkDeleteAsync<SessionUser>(query => query
                .Where($"{nameof(SessionUser.SessionId):C} = @sessionId")
                .WithParameters(new { sessionId }));
        }
    }
}
