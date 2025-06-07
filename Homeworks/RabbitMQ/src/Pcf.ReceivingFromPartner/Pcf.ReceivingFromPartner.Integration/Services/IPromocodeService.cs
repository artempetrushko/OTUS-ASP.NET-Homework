using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Integration.Services;
public interface IPromocodeService
{
    public Task Create(PromoCode promoCode);
}
