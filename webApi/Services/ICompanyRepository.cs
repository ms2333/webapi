using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApi.Entitys;
using webApi.Helpers;
using webApi.Models;

namespace webApi.Services
{
   public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync();
        Task<Company> GetCompany(Guid companyId);
        Task<PageList<Company>> GetCompanies(CompanyParameters parameters);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);
        Task<bool> CompanyExistsAsync(Guid CompanyId);


        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId);
        Task<Employee> GetEmployee(Guid CompanyId, Guid employeeId);
        Task<IEnumerable<Employee>> GetEmployeeForSearch(Guid CompanyId,string str);
        void AddEmployeeAsync(Guid conpanyId, Employee employee);
        void UpdateEmployee(Employee employee);
        void deleteEmployee(Employee employee);

        Task<bool> saveAsync();

    }
}
