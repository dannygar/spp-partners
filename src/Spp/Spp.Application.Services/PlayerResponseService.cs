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
    public class PlayerResponseProfile : Profile
    {
        public PlayerResponseProfile()
        {
            CreateMap<QuestionResponseDto, QuestionnaireResponse>()
                .ForMember(dest => dest.QuestionnaireId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Answer.Value));
        }
    }

    public class PlayerResponseService : SppDbService, IPlayerResponseService
    {
        private IDbConnection _db;

        public PlayerResponseService(SppDbContext context) : base(context)
        {
            _db = context?.SppDbConnection;
            RegisterEntities();
        }

        public PlayerResponseService(BaseDbContext context) : base(context)
        {
            _db = context?.DbConnection;
            RegisterEntities();
        }

        public void RegisterEntities()
        {
            OrmConfiguration.DefaultDialect = SqlDialect.MsSql;

            //Initialize static mapping
            AutoMapperConfiguration.Configure();
        }

        public async Task<bool> CreatePlayerResponse(PlayerResponseDto playerResponseDto)
        {
            var questionnaireResponses = playerResponseDto.Answers.Select(response => new QuestionnaireResponse()
            {
                QuestionnaireId = playerResponseDto.QuestionnaireId,
                UserId = playerResponseDto.PlayerId,
                QuestionId = response.QuestionId,
                AnswerDateTime = response.AnswerDateTime,
                Value = response.Answer.Value
            }).ToList();

            foreach (var response in questionnaireResponses)
            {
                //Insert into Message table
                await _db.InsertAsync(response);
            }

            return true;
        }

        public async Task<bool> UpdatePlayerResponse(PlayerResponseDto playerResponseDto)
        {
            bool result = true;

            //Map the DTO object into the data entity
            var questionnaireResponses = playerResponseDto.Answers.Select(response => new QuestionnaireResponse()
            {
                QuestionnaireId = playerResponseDto.QuestionnaireId,
                UserId = playerResponseDto.PlayerId,
                QuestionId = response.QuestionId,
                AnswerDateTime = response.AnswerDateTime,
                Value = response.Answer.Value
            }).ToList();

            foreach (var response in questionnaireResponses)
            {
                //Find all records belonging to this questionnaire
                var recs = await _db.FindAsync<QuestionnaireResponse>(query => query
                    .Where($"{nameof(QuestionnaireResponse.QuestionId):C} = @questionId And {nameof(QuestionnaireResponse.QuestionnaireId):C} = @questionnaireId")
                  .WithParameters(new
                  {
                      questionId = response.QuestionId,
                      questionnaireId = response.QuestionnaireId
                  }));

                result &= await _db.UpdateAsync(recs);
            }

            return result;
        }

        public async Task<bool> DeletePlayerResponse(int questionId)
        {
            bool result = true;

            //Find all records belonging to this questionnaire
            var recs = await _db.FindAsync<QuestionnaireResponse>(query => query
                .Where($"{nameof(QuestionnaireResponse.QuestionId):C} = @questionId")
              .WithParameters(new
              {
                  questionId = questionId
              }));
            foreach (var rec in recs)
            {
                result &= await _db.DeleteAsync(rec);
            }

            return result;
        }

        public async Task<bool> DeleteAllPlayerResponses(int playerId)
        {
            bool result = true;

            //Find all records belonging to this questionnaire
            var recs = await _db.FindAsync<QuestionnaireResponse>(query => query
                .Where($"{nameof(QuestionnaireResponse.UserId):C} = @playerId")
              .WithParameters(new
              {
                  playerId = playerId
              }));
            foreach (var rec in recs)
            {
                result &= await _db.DeleteAsync(rec);
            }

            return result;
        }

        public async Task<PlayerResponseDto> GetPlayerResponse(PlayerResponseDto playerQuestionnaire)
        {
            //Find all records belonging to this questionnaire
            var recs = await _db.FindAsync<QuestionnaireResponse>(query => query
                .Where($"{nameof(QuestionnaireResponse.UserId):C} = @playerId And {nameof(QuestionnaireResponse.QuestionnaireId):C} = @questionnaireId")
              .WithParameters(new
              {
                  playerId = playerQuestionnaire.PlayerId,
                  questionnaireId = playerQuestionnaire.QuestionnaireId
              }));

            playerQuestionnaire.Answers = new List<QuestionResponseDto>();
            foreach (var response in recs)
            {
                //Map to the DTO table
                var responses = Mapper.Map<QuestionResponseDto>(recs);
                playerQuestionnaire.Answers.Add(responses);
            }

            return playerQuestionnaire;
        }

        public async Task<bool> GetQuestionnairePlayerResponse(int playerId, int questionnaireId)
        {
            //Find all records belonging to this questionnaire
            var recs = await _db.FindAsync<QuestionnaireResponse>(query => query
                .Where($"{nameof(QuestionnaireResponse.UserId):C} = @playerId And {nameof(QuestionnaireResponse.QuestionnaireId):C} = @questionnaireId")
              .WithParameters(new
              {
                  playerId = playerId,
                  questionnaireId = questionnaireId
              }));
            if (recs.Count() > 0) return true;

            return false;
        }
    }
}
