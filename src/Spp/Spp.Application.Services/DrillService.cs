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
    public class DrillProfile : Profile
    {
        public DrillProfile()
        {
            CreateMap<Drill, DrillDto>();
            CreateMap<DrillDto, Drill>();
            CreateMap<Image, ImageDto>();
        }
    }

    public class DrillService : SppDbService, IDrillService
    {
        private IDbConnection _db;

        public DrillService(SppDbContext context) : base(context)
        {
            _db = context?.SppDbConnection;
            RegisterEntities();
        }

        public DrillService(BaseDbContext context) : base(context)
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

        public async Task<IList<DrillDto>> GetAllDrills()
        {
            var drillsDb = await _db.FindAsync<Drill>(query => query
                .Include<Image>(join => join.InnerJoin()));

            return drillsDb.Select(rec =>
                Mapper.Map<DrillDto>(rec)).ToList();
        }
    }
}
