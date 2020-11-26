using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using webApi.Entities;
using webApi.Entitys;
using webApi.Helpers;
using webApi.Models;
using webApi.Services;

namespace webApi.Controllers
{
    [ApiController]//会自动检查方法的输入参数，如果为空，或者类型不正确会自动返回400
    [Route(template: "api/companies")]
    public class CompanierController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanierController(ICompanyRepository companyRepository, IMapper mapper)
        {
            this._companyRepository = companyRepository ?? throw new ArgumentException(nameof(companyRepository));
            this._mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _companyRepository.GetCompaniesAsync();
            // var companyDto = new List<CompanyDto>();
            //foreach (var item in companies)
            //{
            //性能太低，一多就完了
            //    companyDto.Add(new CompanyDto { Id=item.Id,Name=item.Name});
            //}

            //使用对象映射器
            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        [HttpGet(template: "{companyId}", Name = nameof(GetCompanie))]// =api/companies/{companyId}

        public async Task<IActionResult> GetCompanie(Guid CompanyId)
        {
            var company = await _companyRepository.GetCompany(CompanyId);
            //序列化
            if (company != null)
            {
                var companyDto = _mapper.Map<CompanyDto>(company);
                if (companyDto != null)
                {
                    return Ok(companyDto);
                }
            }
            return NotFound();
        }

        [HttpGet(template: "search/{companyId}/{str}")]
        public async Task<IActionResult> GetCompaniesSearch([FromRoute] Guid companyId, string str)
        {
            var companies = await _companyRepository.GetEmployeeForSearch(companyId, str);
            var dto = _mapper.Map<IEnumerable<EmployeeDto>>(companies);
            return Ok(dto);

        }


      

        //key1 = value1,key2 = value2
        //PageTest
        [HttpGet(template: "Page",Name =nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection(
            [FromQuery]CompanyParameters parameters)//[ModelBinder(BinderType =typeof(ArrayModelBinder))]
        {
            if (parameters == null)
            {
                return BadRequest();
            }
            var entitys = await _companyRepository.GetCompanies(parameters);
            //creat PreviousLink ans nextLink if it exists
            var previouLink = entitys.HasPrevious ? CreateCompanyResourseUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextLink = entitys.HasNext ? CreateCompanyResourseUri(parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = entitys.TotalCount,
                pageSize = entitys.PageSize,
                currentPage = entitys.CurrentPage,
                totalPages = entitys.TotalPages,
                previouLink,
                nextLink
            };
            //JsonSerialize then Add to the ResponseHeader 
            Response.Headers.Add("X-pagination", JsonSerializer.Serialize(paginationMetadata, new JsonSerializerOptions
            {
                //解决不安全字符自动转义的问题
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            })); 
            var dto = _mapper.Map<IEnumerable<CompanyDto>>(entitys);
            return Ok(dto);
        }

        private string CreateCompanyResourseUri(CompanyParameters parameters,ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanyCollection), values: new {
                        pageNumber = parameters.PageNumber-1,
                        pageSize = parameters.PageSize,
                        companyId = parameters.CompanyId,
                        searchTerm = parameters.searchTerm
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanyCollection), values: new { 
                        pageNumber = parameters.PageNumber + 1 ,
                        pageSize = parameters.PageSize,
                        companyId = parameters.CompanyId,
                        searchTerm = parameters.searchTerm
                    });
                default:
                    return Url.Link(nameof(GetCompanyCollection), values: new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        companyId = parameters.CompanyId,
                        searchTerm = parameters.searchTerm
                    });

            }    
        }
         
    }
    
}
