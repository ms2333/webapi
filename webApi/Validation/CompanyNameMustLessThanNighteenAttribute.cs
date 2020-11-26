using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using webApi.Entities;
using webApi.Models;

namespace webApi.Validation
{
    //Define user's Validation for Class Level
    public class CompanyNameMustLessThanNighteenAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var addDto = value as CompanyAddDto;//another method:validationContext.ObjectInstance
            if (addDto.Name.Length >= 15)
            {
                return new ValidationResult(errorMessage: "名字太长了");
            }
            return ValidationResult.Success;

        }
    }
}
