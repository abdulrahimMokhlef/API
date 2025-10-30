using API_Project.Context;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace API_Project.Validators
{
  public class UniqueNameAttribute : ValidationAttribute
  {
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
      var deptName = value as string;

       if (string.IsNullOrWhiteSpace(deptName))
      {
        return ValidationResult.Success;
      }

       var db = (InstitutionContext?)validationContext.GetService(typeof(InstitutionContext));

      if (db == null)
      {
        return new ValidationResult("Database context is not available.");
      }

       var existingDepartment = db.Departments.FirstOrDefault(d => d.DeptName == deptName);

      if (existingDepartment != null)
      {
        return new ValidationResult($"Department name '{deptName}' already exists!");
      }

      return ValidationResult.Success;
    }
  }
}
