using System;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitRequestBuilder
    {
        private DateTime _endDate;
        private int _limit;

        public SetPartnerPromoCodeLimitRequestBuilder()
        {
            _endDate = DateTime.UtcNow;
            _limit = 1;
        }

        public SetPartnerPromoCodeLimitRequestBuilder WithEndDate(DateTime endDate)
        {
            _endDate = endDate;
            return this;
        }

        public SetPartnerPromoCodeLimitRequestBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public SetPartnerPromoCodeLimitRequest Build()
        {
            return new SetPartnerPromoCodeLimitRequest()
            {
                EndDate = _endDate,
                Limit = _limit
            };
        }
    }
}