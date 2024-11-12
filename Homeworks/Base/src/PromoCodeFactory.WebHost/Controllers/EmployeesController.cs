using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.WebHost.Models;
using PromoCodeFactory.WebHost.Models.DTO;

namespace PromoCodeFactory.WebHost.Controllers
{
    /// <summary>
    /// Сотрудники
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<Role> _roleRepository;

        public EmployeesController(IRepository<Employee> employeeRepository, IRepository<Role> roleRepository)
        {
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Получить данные всех сотрудников
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<EmployeeShortResponse>> GetEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();

            var employeesModelList = employees.Select(x =>
                new EmployeeShortResponse()
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                }).ToList();

            return employeesModelList;
        }

        /// <summary>
        /// Получить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployeeByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            var employeeModel = new EmployeeResponse()
            {
                Id = employee.Id,
                Email = employee.Email,
                Roles = employee.Roles.Select(x => new RoleItemResponse()
                {
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                FullName = employee.FullName,
                AppliedPromocodesCount = employee.AppliedPromocodesCount
            };

            return employeeModel;
        }

        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        /// <returns>Id нового сотрудника</returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAsync(EmployeeDTO employeeDTO)
        {
            var employee = new Employee() { Id = Guid.NewGuid() };
            await FillEmployeeInfoAsync(employee, employeeDTO);

            await _employeeRepository.AddAsync(employee);
            return Ok(employee.Id);
        }

        /// <summary>
        /// Обновить данные сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAsync(Guid id, EmployeeDTO employeeDTO)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            await FillEmployeeInfoAsync(employee, employeeDTO);

            await _employeeRepository.UpdateAsync(employee);
            return Ok(null);
        }

        /// <summary>
        /// Удалить сотрудника по Id
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var wasEmployeeDeleted = await _employeeRepository.RemoveAsync(id);
            return wasEmployeeDeleted
                ? Ok(null)
                : NotFound();
        }

        private async Task FillEmployeeInfoAsync(Employee employee, EmployeeDTO employeeDTO)
        {
            employee.FirstName = employeeDTO.FirstName;
            employee.LastName = employeeDTO.LastName;
            employee.Email = employeeDTO.Email;
            employee.AppliedPromocodesCount = employeeDTO.AppliedPromocodesCount;

            var roles = await _roleRepository.GetAllAsync();
            var employeeDTORoleIds = employeeDTO.Roles.Select(role => role.Id).ToList();
            employee.Roles = roles.Where(role => employeeDTORoleIds.Contains(role.Id)).ToList();
        }
    }
}