using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Dapper.FastCrud;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spp.Application.Core.Contracts;
using Spp.Application.Core.Models;
using Spp.Application.Services.AutoMapper;
using Spp.Data;
using Spp.Data.Entities;

namespace Spp.Application.Services
{
    public class WellnessProfile : Profile
    {
        public WellnessProfile()
        {
            CreateMap<WellnessDto, Wellness>();
            CreateMap<Wellness, WellnessDto>();
        }
    }

    public class StringTable
    {
        public string[] ColumnNames { get; set; }
        public int[,] Values { get; set; }
    }

    public class WellnessService : SppDbService, IWellnessService
    {
        private IDbConnection _db;

        public WellnessService(SppDbContext context) : base(context)
        {
            _db = context.SppDbConnection;
            RegisterEntities();
        }

        public WellnessService(BaseDbContext context) : base(context)
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

        public async Task<bool> SubmitWellness(WellnessDto dto)
        {
            await InvokeRequestResponseService(dto);

            var wellness = Mapper.Map<Wellness>(dto);

            try
            {
                await _db.InsertAsync(wellness);
            }
            catch (System.Exception)
            {
                await _db.UpdateAsync(wellness);
            }


            return true;
        }

        public async Task<List<WellnessDto>> GetWellnessesByPlayerId(int playerId)
        {
            var wellnesses = await _db.FindAsync<Wellness>(query => query
                .Where($"{nameof(Wellness.Player_ID):C} = @playerId")
                .WithParameters(new { playerId }));

            return wellnesses.Select(rec =>
                Mapper.Map<WellnessDto>(rec)).ToList();
        }

        private async Task InvokeRequestResponseService(WellnessDto dto)
        {
            using (var client = new HttpClient())
            {
                var scoreRequest = new
                {

                    Inputs = new Dictionary<string, StringTable>() {
                        {
                            "wellness",
                            new StringTable()
                            {
                                ColumnNames = new string[] {"Energy", "Sleep", "Stress", "Soreness"},
                                Values = new int[,] { { dto.Energy, dto.Sleep, dto.Stress, dto.Soreness } }
                            }
                        },
                    },
                    GlobalParameters = new Dictionary<string, string>() { }
                };
                const string apiKey = "VQpE4GE+r+tdsR6w8PZkiKcgmhwTmYejfaYTBSvtEHnRMIK+Kj9zmsjavE9L/jf2P3pTKo4iraq5Ft5DPDURzQ=="; // Replace this with the API key for the web service
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                client.BaseAddress = new Uri("https://uswestcentral.services.azureml.net/workspaces/caf948b149954e37955d0bc710b1a545/services/7e58bd7f7cdb4de7b999f8e522200edc/execute?api-version=2.0&details=true");

                // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                // For instance, replace code such as:
                //      result = await DoSomeTask()
                // with the following:
                //      result = await DoSomeTask().ConfigureAwait(false)

                var json = JsonConvert.SerializeObject(scoreRequest);
                var content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                HttpResponseMessage response = await client.PostAsync("", content);

                string result = await response.Content.ReadAsStringAsync();
                var resultJson = JObject.Parse(result);


                var readinessProbability = resultJson["Results"]["readiness"]["value"]["Values"][0][5].ToString();
                var readiness = resultJson["Results"]["readiness"]["value"]["Values"][0][4].ToString();
                dto.ReadinessProbability = Convert.ToDouble(readinessProbability);
                dto.Readiness = Convert.ToInt32(readiness);

            }
        }

    }
}
