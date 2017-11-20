/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Microsoft Corporation. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Spp.Application.Core.Models;
using Spp.Tests.Fixture;
using Xunit;
using Xunit.Abstractions;

namespace Spp.Application.Services.Tests.Unit
{
    [CollectionDefinition("SppDatabase")]
    public class SettingsTests : IClassFixture<DataInitFixture>
    {
        private readonly ITestOutputHelper _output;

        private DataInitFixture _fixture;
        private SettingsService _service;
        private SessionService _sessionservice;

        public SettingsTests(DataInitFixture fixture, ITestOutputHelper output)
        {
            //Initialize the output
            this._output = output;

            //Initialize DbContext in memory
            this._fixture = fixture;

            //Create the test service
            _service = new SettingsService(_fixture.Context);
            _sessionservice = new SessionService(_fixture.Context);
        }

        [Fact]
        public async void GetPlayerPositions()
        {
            var result = await _service.GetPlayerPositions();
            Assert.Null(result);
        }

        [Fact]
        public async void GetSubPositions()
        {
            var result = await _service.GetSubPositions();
            Assert.Null(result);
        }

        [Fact]
        public async void GetMesocycles()
        {
            var result = await _service.GetMesocycles();
            Assert.Null(result);
        }

        [Fact]
        public async void GetMicrocycles()
        {
            var result = await _service.GetMicrocycles();
            Assert.Null(result);
        }

        [Fact]
        public async void GetImages()
        {
            var result = await _service.GetImages();
            Assert.Null(result);
        }

        [Fact]
        public async void CreatePlayerPosition()
        {
            var position = new PlayerPositionDto()
            {
                Abbreviation = "FWD",
                Name = "Forward",
                SportId = 1
            };
            var result = await _service.CreatePlayerPosition(position);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void CreateSessionUser()
        {
            var sessionuser = new SessionUserDto()
            {
                SessionId = 10000,
                UserId = 3
            };
            var result = await _sessionservice.CreateSessionUser(sessionuser);
            Assert.True(result);
        }

        [Fact]
        public async void CreateSubPosition()
        {
            var subposition = new SubPositionDto()
            {
                Abbreviation = "FWD",
                Name = "Forward",
                PositionId = 1
            };
            var result = await _service.CreateSubPosition(subposition);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void CreateMesocycle()
        {
            var mesocycle = new MesocycleDto()
            {
                EndDate = DateTime.Now,
                MesocycleNumber = 1,
                Name = "TestMesoCycle",
                SeasonId = 1,
                StartDate = DateTime.Now,
                SubTeamId = 2,
                TeamId = 1
            };
            var result = await _service.CreateMesocycle(mesocycle);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void CreateMicrocycle()
        {
            var microcycle = new MicrocycleDto()
            {
                EndDate = DateTime.Now,
                Name = "TestMicroCycle",
                SeasonId = 1,
                StartDate = DateTime.Now,
                SubTeamId = 2,
                TeamId = 1
            };
            var result = await _service.CreateMicrocycle(microcycle);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void CreateImage()
        {
            var image = new ImageDto()
            {
                Url = "http://news.com",
                Tags = "MotivationalDelete"
            };
            var result = await _service.CreateImage(image);
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async void UpdatePosition()
        {
            var position = new PlayerPositionDto()
            {
                Abbreviation = "FWD",
                Name = "ForwardUPDATED",
                SportId = 1,
                Id = 1
            };
            var result = await _service.UpdatePlayerPosition(position);
            Assert.True(result);
        }

        [Fact]
        public async void UpdateMicrocycle()
        {
            var microcycle = new MicrocycleDto()
            {
                EndDate = DateTime.Now,
                Name = "TestMicroCycleUPDATED",
                SeasonId = 1,
                StartDate = DateTime.Now,
                SubTeamId = 2,
                TeamId = 1,
                Id = 2
            };
            var result = await _service.UpdateMicrocycle(microcycle);
            Assert.True(result);
        }

        [Fact]
        public async void UpdateMesocycle()
        {
            var mesocycle = new MesocycleDto()
            {
                EndDate = DateTime.Now,
                MesocycleNumber = 1,
                Name = "TestMesoCycleUpdated",
                SeasonId = 1,
                StartDate = DateTime.Now,
                SubTeamId = 2,
                TeamId = 1,
                Id = 2
            };
            var result = await _service.UpdateMesocycle(mesocycle);
            Assert.True(result);
        }

        [Fact]
        public async void UpdateSubPosition()
        {
            var subposition = new SubPositionDto()
            {
                Abbreviation = "FWD UPDATED",
                Name = "Forward",
                PositionId = 1,
                Id = 1

            };
            var result = await _service.UpdateSubPosition(subposition);
            Assert.True(result);
        }

        [Fact]
        public async void UpdateImage()
        {
            var image = new ImageDto()
            {
                Url = "http://news.com",
                Tags = "MotivationalDeleteUPDATED",
                Id = 47

            };
            var result = await _service.UpdateImage(image);
            Assert.True(result);
        }

        [Theory]
        [InlineData(5)]
        public async void DeletePlayerPosition(int positionId)
        {
            var result = await _service.DeletePlayerPosition(positionId);
            Assert.True(result);
        }



        [Theory]
        [InlineData(1)]
        public async void DeleteSubPosition(int subpositionId)
        {
            var result = await _service.DeleteSubPosition(subpositionId);
            Assert.True(result);
        }

        [Theory]
        [InlineData(12)]
        public async void DeleteMesocycle(int mesocycleId)
        {
            var result = await _service.DeleteMesocycle(mesocycleId);
            Assert.True(result);
        }

        [Theory]
        [InlineData(1)]
        public async void DeleteMicrocycle(int microsycleId)
        {
            var result = await _service.DeleteMicrocycle(microsycleId);
            Assert.True(result);
        }

        [Theory]
        [InlineData(46)]
        public async void DeleteImage(int imageId)
        {
            var result = await _service.DeleteImage(imageId);
            Assert.True(result);
        }
    }
}
