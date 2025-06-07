using System;
using System.Threading.Tasks;
using MassTransit;
using Pcf.GivingToCustomer.Core.Services;
using Pcf.GivingToCustomer.WebHost.Models;
using Pcf.ReceivingFromPartner.Integration.Dto;

namespace Pcf.GivingToCustomer.WebHost.Consumers
{
    public class PromocodeConsumer : IConsumer<GivePromoCodeToCustomerDto>
    {
        private readonly PromocodesService _promocodesService;

        public PromocodeConsumer(PromocodesService promocodesService)
        {
            _promocodesService = promocodesService;
        }

        public async Task Consume(ConsumeContext<GivePromoCodeToCustomerDto> context)
        {
            Console.WriteLine($"GivingToCustomer Consume {context.Message.PartnerManagerId}");

            var givePromoCodeToCustomerDto = context.Message;
            GivePromoCodeRequest request = new()
            {
                ServiceInfo = givePromoCodeToCustomerDto.ServiceInfo,
                PartnerId = givePromoCodeToCustomerDto.PartnerId,
                PromoCodeId = givePromoCodeToCustomerDto.PromoCodeId,
                PromoCode = givePromoCodeToCustomerDto.PromoCode,
                PreferenceId = givePromoCodeToCustomerDto.PreferenceId,
                BeginDate = givePromoCodeToCustomerDto.BeginDate,
                EndDate = givePromoCodeToCustomerDto.EndDate
            };

            await _promocodesService.GivePromoCodesToCustomersWithPreferenceAsync(request);
        }
    }
}