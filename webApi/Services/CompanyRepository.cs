using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApi.Data;
using webApi.Entitys;
using webApi.Helpers;
using webApi.Models;

namespace webApi.Services
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutingDbcontext _routingDbcontext;
        //inject service
        public CompanyRepository(RoutingDbcontext routingDbcontext)
        {
            //如果注入为NULL的话抛出异常
            this._routingDbcontext = routingDbcontext ?? throw new ArgumentException(nameof(routingDbcontext));
        }
        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentException(nameof(company));
            }
            company.Id = Guid.NewGuid();
            if (company.Employees != null)
            {
                foreach (var employ in company.Employees)
                {
                    employ.Id = Guid.NewGuid();
                }
            }
            _routingDbcontext.companies.Add(company);

        }

        public void AddEmployeeAsync(Guid conpanyId, Employee employee)
        {
            employee.CompanyId = conpanyId;
            _routingDbcontext.employees.Add(employee);
        }

        public async Task<bool> CompanyExistsAsync(Guid CompanyId)
        {
            return await _routingDbcontext.companies.AnyAsync(predicate: x => x.Id == CompanyId);
        }

        public void DeleteCompany(Company company)
        {
            _routingDbcontext.companies.Remove(company);
        }

        public void deleteEmployee(Employee employee)
        {
            _routingDbcontext.employees.Remove(employee);
        }

        public async Task<PageList<Company>> GetCompanies(CompanyParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            var queryExpression = _routingDbcontext.companies as IQueryable<Company>;//do not Search for Sql
            IQueryable<Company> filter = null;
            if (parameters.searchTerm != null)
            {
                   filter = queryExpression
                    .Where(x =>
                    x.Name.Contains(parameters.searchTerm) ||
                    x.Introduction.Contains(parameters.searchTerm));//do not Search for Sql

                //serach for Sqlserver in CountAsync method which method inside CreateAsync
                return await PageList<Company>.CreateAsync((IQueryable<Company>)filter, parameters.PageNumber, parameters.PageSize);
            }
            filter = queryExpression;
            //order by
            if (!string.IsNullOrWhiteSpace(parameters.orderBy))
            {
                if (parameters.orderBy.ToLower() == "name")
                {
                    filter = filter.OrderBy(x=>x.Id);
                }
            }
            return await PageList<Company>.CreateAsync(filter, parameters.PageNumber, parameters.PageSize);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync()
        {
            return await _routingDbcontext.companies.ToListAsync();
        }

        public async Task<Company> GetCompany(Guid companyId)
        {
            if (companyId == null)
            {
                throw new ArgumentException(nameof(companyId));
            }
            return await _routingDbcontext.companies.Where(x => x.Id == companyId).FirstOrDefaultAsync();
        }

        public async Task<Employee> GetEmployee(Guid CompanyId, Guid employeeId)
        {
            return await _routingDbcontext.employees
                .Where(x => x.CompanyId == CompanyId && x.Id == employeeId)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Employee>> GetEmployeeForSearch(Guid CompanyId, string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                str = str.Trim();
                var query = await _routingDbcontext.employees.Where(x => x.CompanyId == CompanyId
                                                              && (
                                                              x.FirstName.Contains(str) ||
                                                              x.LastName.Contains(str)
                                                              )
                                                              ).OrderBy(x => x.CompanyId).ToListAsync();
                if (!string.IsNullOrWhiteSpace(str) && (query != null))
                {
                    return query;
                }
            }
            return await _routingDbcontext.employees.Where(x => x.CompanyId == CompanyId).ToListAsync();

        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId)
        {
            return await _routingDbcontext.employees.Where(c => c.CompanyId == companyId).ToListAsync();
        }

        public async Task<bool> saveAsync()
        {
            return await _routingDbcontext.SaveChangesAsync() >= 0;
        }
        //下面是不需要的，ef框架会Auto实体跟踪的
        public void UpdateCompany(Company company)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }


    }
}
