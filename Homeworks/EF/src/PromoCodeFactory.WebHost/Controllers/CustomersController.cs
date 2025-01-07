using System;
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
    /// Клиенты
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Preference> _preferenceRepository;

        public CustomersController(IRepository<Customer> customerRepository, IRepository<Preference> preferenceRepository)
        {
            _customerRepository = customerRepository;
            _preferenceRepository = preferenceRepository;
        }

        /// <summary>
        /// Получить список всех клиентов
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CustomerShortResponse>>> GetCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers
                .Select(c => new CustomerShortResponse()
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
                })
                .ToList();
        }

        /// <summary>
        /// Получить клиента по id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerAsync(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            return new CustomerResponse()
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PromoCodes = customer.PromoCodes
                    .Select(p => new PromoCodeShortResponse()
                    {
                        Id = p.Id,
                        Code = p.Code,
                        ServiceInfo = p.ServiceInfo,
                        BeginDate = p.BeginDate.ToString(),
                        EndDate = p.EndDate.ToString(),
                        PartnerName = p.PartnerName
                    })
                    .ToList(),
                Preferences = customer.CustomerPreferences
                    .Select(cp => new PreferenceResponse()
                    {
                        Id = cp.Preference.Id,
                        Name = cp.Preference.Name,
                    })
                    .ToList()
            };
        }

        /// <summary>
        /// Создать нового клиента
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync(CreateOrEditCustomerRequest request)
        {
            var customer = new Customer()
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                CustomerPreferences = new()
            };

            await SetCustomerPreferencesFromRequestAsync(customer, request);

            _customerRepository.Add(customer);
            await _customerRepository.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Обновить данные клиента
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditCustomersAsync(Guid id, CreateOrEditCustomerRequest request)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.FirstName = request.FirstName;
            customer.LastName = request.LastName;
            customer.Email = request.Email;

            customer.CustomerPreferences.Clear();
            await SetCustomerPreferencesFromRequestAsync(customer, request);

            _customerRepository.Update(customer);
            await _customerRepository.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Удалить клиента по id
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            _customerRepository.Delete(customer);
            await _customerRepository.SaveChangesAsync();
            return Ok();
        }

        private async Task SetCustomerPreferencesFromRequestAsync(Customer customer, CreateOrEditCustomerRequest request)
        {
            if (request.PreferenceIds != null && request.PreferenceIds.Count != 0)
            {
                foreach (var preferenceId in request.PreferenceIds)
                {
                    var preference = await _preferenceRepository.GetByIdAsync(preferenceId);
                    if (preference != null)
                    {
                        customer.CustomerPreferences.Add(new CustomerPreference()
                        {
                            CustomerId = customer.Id,
                            PreferenceId = preference.Id
                        });
                    }
                }
            }
        }
    }
}