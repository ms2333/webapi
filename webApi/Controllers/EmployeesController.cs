using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using webApi.Entitys;
using webApi.Models;
using webApi.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace webApi.Controllers
{
    [ApiController]
    [Route(template: "api/companies")]
    public class EmployeesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public EmployeesController(IMapper mapper, ICompanyRepository companyRepository)
        {
            this._mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            this._companyRepository = companyRepository ?? throw new ArgumentException(nameof(companyRepository));
        }

        [HttpGet(template:"Employee/{companyId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany([FromRoute]Guid companyId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employess = await _companyRepository.GetEmployeesAsync(companyId);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employess);
            return Ok(employeeDtos);
        }

        [HttpGet(template:"{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> GetEmployeesForCompany(Guid companyId,Guid employeeId)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }
            var employee = await _companyRepository.GetEmployee(companyId, employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeDto>(employee);
            return Ok(employeeDto);
        }


        //put come true to replace the entirty 
        //null value will been get DefaultValue
        [HttpPut(template: "{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeCompany(Guid companyId, Guid employeeId, EmployeeUpdateDto UpdateDto)
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await _companyRepository.GetEmployee(companyId, employeeId);

            if (employeeId == null)
            {
                return NotFound();
            }

            //update with three step =>
            //1.entity convert to updateDto
            //2.put inputEmployee update to updateDto
            //3.updateDto map to entity
            _mapper.Map(UpdateDto, employeeEntity);//need to configure EmployeeProfile
            _companyRepository.UpdateEmployee(employeeEntity);
            //only need to saveChange ,do net need last method 
            //because of The EntityFramework would to auto tail after dateChanged!
            await _companyRepository.saveAsync();
            return NoContent();
        }

        [HttpPatch(template: "{companyId}/{employeeId}")]
        public async Task<IActionResult> PartiallyUpdateEmloyeeForCompany(
            Guid companyId, [FromRoute]Guid employeeId
            , JsonPatchDocument<EmployeeUpdateDto> patchDocument
            )
        {
            if (!await _companyRepository.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employeeEntity = await _companyRepository.GetEmployee(companyId, employeeId);

            //Created it if resource from patch is not exists 
            if (employeeEntity == null)
            {
                var employeeDto = new EmployeeUpdateDto();
                patchDocument.ApplyTo(employeeDto, ModelState);
                if (!TryValidateModel(employeeDto))
                {
                    return ValidationProblem();
                }
                var employeeToAdd = _mapper.Map<Employee>(employeeDto);
                employeeToAdd.Id = employeeId;
                _companyRepository.AddEmployeeAsync(companyId, employeeToAdd);
               await _companyRepository.saveAsync();  
            }

            var dtoToPatch = _mapper.Map<EmployeeUpdateDto>(employeeEntity);
            // need to handling of the validation error
            // if error happened it will be return correct ModelState to the client :400 
            patchDocument.ApplyTo(dtoToPatch,ModelState);
            //add validation to return true StateCode
            if (!TryValidateModel(dtoToPatch))
            {
                return ValidationProblem(ModelState);
            }
            //put once Object tail after other object=>
            _mapper.Map(source: dtoToPatch, employeeEntity);
            //_companyRepository.UpdateEmployee(employeeEntity);
            await _companyRepository.saveAsync();
            return NoContent();
        }

        //In order to use UserErrorInformationFormat if error happened
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var option = HttpContext.RequestServices.GetRequiredService <IOptions<ApiBehaviorOptions>>();
            return option.Value.InvalidModelStateResponseFactory(ControllerContext) as ActionResult;
        }



    }
}
