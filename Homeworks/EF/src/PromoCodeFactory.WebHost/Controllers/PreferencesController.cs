using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Предпочтения
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PreferencesController : ControllerBase
    {
        private IRepository<Preference> _preferencesRepository;

        public PreferencesController(IRepository<Preference> preferencesRepository)
        {
            _preferencesRepository = preferencesRepository;
        }

        /// <summary>
        /// Получить все предпочтения
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<PreferenceResponse>>> GetPreferencesAsync()
        {
            var preferences = await _preferencesRepository.GetAllAsync();
            return preferences
                .Select(p => new PreferenceResponse()
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToList();
        }
    }
}