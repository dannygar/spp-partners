/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Data;
using System.Threading.Tasks;
using AutoMapper;
using Dapper.FastCrud;
using Spp.Application.Core.Contracts;
using Spp.Application.Core.Models;
using Spp.Data;
using Spp.Data.Entities;

namespace Spp.Application.Services
{
    public class BenchmarkProfile : Profile
    {
        public BenchmarkProfile()
        {
            CreateMap<Benchmark, BenchmarkDto>();
            CreateMap<BenchmarkDto, Benchmark>();
        }
    }

    public class BenchmarkService : SppDbService, IBenchmarkService
    {
        private IDbConnection _db;

        public BenchmarkService(SppDbContext context) : base(context)
        {
            _db = context?.SppDbConnection;
        }

        public BenchmarkService(BaseDbContext context) : base(context)
        {
            _db = context.DbConnection;
        }

        public void RegisterEntities()
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> AddBenchmark(BenchmarkDto benchmarkDto)
        {
            var benchmark = Mapper.Map<Benchmark>(benchmarkDto);
            await _db.InsertAsync(benchmark);

            return benchmark.Id;
        }
    }
}
