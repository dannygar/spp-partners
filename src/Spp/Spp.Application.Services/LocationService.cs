/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
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
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<LocationDto, Location>();
            CreateMap<Location, LocationDto>();
        }
    }

    public class LocationService : SppDbService, ILocationService
    {
        private IDbConnection _db;

        public LocationService(SppDbContext context) : base(context)
        {
            _db = context?.SppDbConnection;
            RegisterEntities();
        }

        public LocationService(BaseDbContext context) : base(context)
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

        public async Task<List<LocationDto>> GetLocations()
        {
            var dbRecs = await _db.FindAsync<Location>();

            return dbRecs.Select(rec =>
                Mapper.Map<LocationDto>(rec)).ToList();
        }

        public async Task<int> CreateLocation(LocationDto locationDto)
        {
            //Map to the Data Entity object
            var recDb = Mapper.Map<Location>(locationDto);

            //Insert into Message table
            await _db.InsertAsync(recDb);

            return recDb.Id;
        }

        public async Task<bool> UpdateLocatioin(LocationDto locationDto)
        {
            //Map the DTO object into the data entity
            var recDb = Mapper.Map<Location>(locationDto);
            return await _db.UpdateAsync(recDb);
        }

        public async Task<bool> DeleteLocation(int locationId)
        {
            //Find the the record to be deleted
            var rec = new Location() { Id = locationId };
            var recDb = await _db.GetAsync(rec);

            //Delete the the record from the DB
            return await _db.DeleteAsync(recDb);
        }
    }
}
