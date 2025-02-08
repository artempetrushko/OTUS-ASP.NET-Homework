using System;
using System.Collections.Generic;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class PartnerBuilder
    {
        private Guid _id;
        private string _name;
        private int _numberIssuedPromoCodes;
        private bool _isActive;
        private ICollection<PartnerPromoCodeLimit> _partnerLimits;

        public PartnerBuilder WithGuid(Guid id)
        {
            _id = id;
            return this;
        }

        public PartnerBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PartnerBuilder WithNumberIssuedPromoCodes(int numberIssuedPromoCodes)
        {
            _numberIssuedPromoCodes = numberIssuedPromoCodes;
            return this;
        }

        public PartnerBuilder SetActive(bool isActive)
        {
            _isActive = isActive;
            return this;
        }

        public PartnerBuilder WithPartnerLimits(ICollection<PartnerPromoCodeLimit> partnerLimits)
        {
            _partnerLimits = partnerLimits;
            return this;
        }

        public Partner Build()
        {
            return new Partner()
            {
                Id = _id,
                Name = _name,
                NumberIssuedPromoCodes = _numberIssuedPromoCodes,
                IsActive = _isActive,
                PartnerLimits = _partnerLimits
            };
        }
    }
}