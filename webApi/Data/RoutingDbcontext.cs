using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApi.Entitys;

namespace webApi.Data
{
    public class RoutingDbcontext : DbContext
    {
        //调用父类把options配置传过去才能工作
        public RoutingDbcontext(DbContextOptions<RoutingDbcontext> options) : base(options)
        {

        }
        //映射数据库的两个表
        public DbSet<Company> companies { set; get; }
        public DbSet<Employee> employees { set; get; }

        //-------------------  add Limit  ----------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);//添加限制

            //this specified for between two class of relationship
            modelBuilder.Entity<Employee>().HasOne(navigationExpression: x => x.Company)//导航属性时company(一个员工在一个公司)
                .WithMany(navigationExpression: x => x.Employees)//反过来时Emploees (一个公司拥有多个员工的集合)
                 //DeleteBehavior.Restrict=>外键时CompanyId，删除的时候公司有员工的话是无法删除的
                 //seed data 种子数据
            .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);//级联删除，删除公司的同时，该公司的员工也会被删除
            
            modelBuilder.Entity<Company>().HasData(new Company
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Microsoft",
                Introduction = "Great Company",
            }, new Company
            {
                Id = Guid.Parse("30000000-0000-0000-0000-000000000001"),
                Name = "Aicrosoft",
                Introduction = "Great Company",
            }
            );
            modelBuilder.Entity<Employee>().HasData(
                    new Employee
                    {
                        Id= Guid.Parse("11000000-0000-0000-0000-000000000001"),
                        CompanyId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        DateOfBirth = new DateTime(year: 2020, month: 6, day: 4),
                        EmployeeNo = "G456",
                        FirstName = "Li",
                        LastName = "huahua",
                        Gender = Gender.男
                    },
                    new Employee
                    {
                        Id = Guid.Parse("11000000-0000-0000-0000-000000000002"),
                        CompanyId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                        DateOfBirth = new DateTime(year: 2005, month: 8, day: 6),
                        EmployeeNo = "G457",
                        FirstName = "xiao",
                        LastName = "ming",
                        Gender = Gender.男
                    });


        }
    }
}
