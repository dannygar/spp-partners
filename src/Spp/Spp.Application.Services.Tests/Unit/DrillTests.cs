/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Spp.Tests.Fixture;
using Xunit;
using Xunit.Abstractions;

namespace Spp.Application.Services.Tests.Unit
{
    [CollectionDefinition("SppDatabase")]
    public class DrillTests : IClassFixture<DataInitFixture>
    {
        private readonly ITestOutputHelper _output;

        private DataInitFixture _fixture;
        private DrillService _service;

        public DrillTests(DataInitFixture fixture, ITestOutputHelper output)
        {
            //Initialize the output
            this._output = output;

            //Initialize DbContext in memory
            this._fixture = fixture;

            //Create the test service
            _service = new DrillService(_fixture.Context);

        }

        [Fact]
        public async void GetAllDrillsTest()
        {
            try
            {
                var result = await _service.GetAllDrills();
                Assert.NotNull(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
