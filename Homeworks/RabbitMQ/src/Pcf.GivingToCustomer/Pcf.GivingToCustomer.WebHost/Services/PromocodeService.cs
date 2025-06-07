using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.GivingToCustomer.WebHost.Models;

namespace Pcf.GivingToCustomer.WebHost.Services;

public class PromocodeService : PromocodeCreator.PromocodeCreatorBase
{
    private readonly IRepository<PromoCode> _promoCodesRepository;
    private readonly IRepository<Preference> _preferencesRepository;
    private readonly IRepository<Customer> _customersRepository;

    public PromocodeService(
        IRepository<PromoCode> promocodeRepository,
        IRepository<Preference> preferencesRepository,
        IRepository<Customer> customersRepository)
    {
        _promoCodesRepository = promocodeRepository;
        _preferencesRepository = preferencesRepository;
        _customersRepository = customersRepository;
    }

    public override async Task<Response> Create(Request request, ServerCallContext context)
    {
        var errors = string.Empty;

        var givePromoCodeRequest = MapToRequest(request);

        if (givePromoCodeRequest.PartnerId == default ||
            givePromoCodeRequest.PreferenceId == default ||
            givePromoCodeRequest.PromoCodeId == default)
        {
            return new Response() { Result = false, Message = "Guid parse error" };
        }

        var preference = await _preferencesRepository.GetByIdAsync(givePromoCodeRequest.PreferenceId);

        if (preference == null)
        {
            return new Response() { Result = false, Message = "BadRequest" };
        }

        var customers = await _customersRepository
            .GetWhere(d => d.Preferences.Any(x =>
                x.Preference.Id == preference.Id));

        PromoCode promoCode = PromoCodeMapper.MapFromModel(givePromoCodeRequest, preference, customers);

        await _promoCodesRepository.AddAsync(promoCode);

        return new Response() { Result = true };
    }

    private static GivePromoCodeRequest MapToRequest(Request request)
    {
        _ = Guid.TryParse(request.PartnerId.Value, out Guid partnerId);
        _ = Guid.TryParse(request.PreferenceId.Value, out Guid preferenceId);
        _ = Guid.TryParse(request.PromoCodeId.Value, out Guid promoCodeId);

        return new GivePromoCodeRequest()
        {
            BeginDate = request.BeginDate,
            EndDate = request.EndDate,
            PartnerId = partnerId,
            PreferenceId = preferenceId,
            PromoCode = request.PromoCode,
            PromoCodeId = promoCodeId,
            ServiceInfo = request.ServiceInfo,
        };
    }
}