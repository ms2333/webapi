using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApi.Entities;
using webApi.Entitys;
using webApi.Models;

namespace webApi.ProFile
{
    public class CompanyProfile:Profile
    {
        //映射的配置文件
        public CompanyProfile()
        {
            //从Company映射到CompanyDto
            //约定：1.原属性和目标属性一样的话，他属性的值就会赋给目标属性
            //2.如果目标的属性再原属性中不存在，直接就忽略了，不会对它进行赋值
            CreateMap<Company, CompanyDto>()
            //执行手动映射(常用)
            .ForMember(
                //Name  映射到 CompanyName
                memberOptions: opt => opt.MapFrom(mapExpression: src => src.Name),
                destinationMember: dest => dest.CompanyName
                );
            CreateMap<CompanyAddDto, Company>();//属性名称一样，这样写就可以了
        }
    }
}
