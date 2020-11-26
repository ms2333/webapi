using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using webApi.Validation;

namespace webApi.Entities
{
    [CompanyNameMustLessThanNighteenAttribute]
    public class CompanyAddDto:IValidatableObject
    {
        //ID不需要的，它要在api后台自动生成，当然客户端提供也是可以的
        //公司类，公司输出类，公司添加类，分别创建（便于以后改动！）
        [Display(Name="Company Name!")]
        [Required(ErrorMessage ="{0} is Required!!!!!")] //{0} is name
        public string Name { get; set; }
        public string Introduction { get; set; }

        //add user Validation with complex 
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Length >= 15)
            {
                yield return new ValidationResult(errorMessage: "~~~~", memberNames: new[] { nameof(Name) });
            }
        }

        //the other validation : FluentValidation(recommendation)
        //1.easy create complex validation
        //2.validation rules seprate form Model
        //3.to unit Test
        
    }
}
