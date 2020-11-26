using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApi.Entitys;
using webApi.Models;

namespace webApi.ProFile
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>()
                .ForMember(
                    dest => dest.Id,
                    option => option.MapFrom(src => src.Id)
                )
                .ForMember(
                dest => dest.Name,
                src => src.MapFrom(s => s.FirstName)
                )
                .ForMember(
                dest => dest.GenderDisplay,
                option => option.MapFrom(s => s.Gender.ToString())
                )
                .ForMember(
                dest => dest.age,
                from => from.MapFrom(s => DateTime.Now.Year - s.DateOfBirth.Year)
                );
                CreateMap<EmployeeUpdateDto, Employee>();
                CreateMap<Employee, EmployeeUpdateDto>();
        }
    }
}
