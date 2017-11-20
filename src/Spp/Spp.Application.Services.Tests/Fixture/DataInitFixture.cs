/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Spp.Data;
using Xunit;

namespace Spp.Tests.Fixture
{
    public class DataInitFixture : IDisposable
    {
        public const string ActiveTenant = "DbTenants:ActiveTenant";

        public BaseDbContext Context { get; }

        public IConfigurationRoot Configuration { get; }

        public DataInitFixture()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("testsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //Get database connnection string
            var dbConnection = new SqlConnection(
                Configuration[$"DbTenants:{Configuration[ActiveTenant]}:SppDbConnection"]);

            //Initialize DbContext in memory
            var optionBuiler = new DbContextOptionsBuilder();
            optionBuiler.UseInMemoryDatabase("TestSppDatabase");
            Context = new BaseDbContext(optionBuiler.Options, dbConnection);

            // Do "global" initialization here; Only called once.
            //InsertTestData();
        }

        public void Dispose()
        {
            // Do "global" teardown here; Only called once.
        }
    }

    [CollectionDefinition("SppDatabase")]
    public class SppDataCollection : ICollectionFixture<DataInitFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
