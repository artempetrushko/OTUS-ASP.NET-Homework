using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using Xunit;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();
            _partnersController = fixture.Build<PartnersController>().OmitAutoProperties().Create();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            var partnerId = Guid.NewGuid();
            var request = new SetPartnerPromoCodeLimitRequestBuilder().Build();

            Partner partner = null;
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotActive_ReturnsBadRequestAsync()
        {
            var partnerId = Guid.NewGuid();
            var request = new SetPartnerPromoCodeLimitRequestBuilder().Build();

            var partner = new PartnerBuilder()
                .WithGuid(partnerId)
                .SetActive(false)
                .Build();
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerHasActiveLimit_NumberIssuedPromoCodesShouldEqualZeroAsync()
        {
            var partnerId = Guid.NewGuid();
            var request = new SetPartnerPromoCodeLimitRequestBuilder().Build();

            var (partner, partnerLimit) = CreatePartnerWithActiveLimit(partnerId);

            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            partner.NumberIssuedPromoCodes.Should().Be(0);
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerHasActiveLimit_LimitCancelDataShouldEqualCurrentTimeAsync()
        {
            var partnerId = Guid.NewGuid();
            var request = new SetPartnerPromoCodeLimitRequestBuilder().Build();

            var (partner, partnerLimit) = CreatePartnerWithActiveLimit(partnerId);

            var currentTime = DateTime.Now;
            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            partnerLimit.CancelDate.Value.Should().BeAfter(currentTime);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task SetPartnerPromoCodeLimitAsync_LimitLessThanOrEqualsZero_ReturnsBadRequestAsync(int promoCodeLimitValue)
        {
            var partnerId = Guid.NewGuid();
            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithLimit(promoCodeLimitValue)
                .Build();

            CreatePartnerWithActiveLimit(partnerId);

            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_NewLimitAddedForPartner_NewLimitSavedInDatabaseAsync()
        {
            var partnerId = Guid.NewGuid();
            var request = new SetPartnerPromoCodeLimitRequestBuilder()
                .WithLimit(1)
                .Build();

            var (partner, partnerLimit) = CreatePartnerWithActiveLimit(partnerId);

            await _partnersController.SetPartnerPromoCodeLimitAsync(partnerId, request);

            partner.PartnerLimits
                .FirstOrDefault(limit => limit.Limit == request.Limit && limit.EndDate == request.EndDate)
                .Should()
                .NotBeNull();
        }

        private (Partner partner, PartnerPromoCodeLimit limit) CreatePartnerWithActiveLimit(Guid partnerId)
        {
            Partner partner = null;
            var partnerLimit = new Fixture()
                .Build<PartnerPromoCodeLimit>()
                .With(limit => limit.Partner, partner)
                .Without(limit => limit.CancelDate)
                .Create();
            partner = new PartnerBuilder()
                .WithGuid(partnerId)
                .SetActive(true)
                .WithPartnerLimits(new List<PartnerPromoCodeLimit>() { partnerLimit })
                .Build();
            _partnersRepositoryMock.Setup(repo => repo.GetByIdAsync(partnerId)).ReturnsAsync(partner);

            return (partner, partnerLimit);
        }
    }
}