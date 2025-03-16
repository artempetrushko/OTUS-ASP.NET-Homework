using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Administration.DataAccess;
using Pcf.Administration.DataAccess.Repositories;

namespace Pcf.Administration.WebHost.Services
{
    public class EmployeeService
    {
        private readonly IRepository<Employee> _employeeRepository;

        public EmployeeService(MongoDBContext mongoDBContext)
        {
            _employeeRepository = new MongoRepository<Employee>(mongoDBContext.Database, MongoDBSettings.EMPLOYEES_COLLECTION_NAME);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeRepository.GetAllAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(Guid id)
        {
            return await _employeeRepository.GetByIdAsync(id);
        }
    }
}
