using Microflake.Core.Persistence;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Microflake.Core.ViewModel.Category.CustomCategories
{
    public class CategoryDuplicateName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string name = value.ToString();

                var db = new ApplicationDbContext();
                try
                {
                    var obj = validationContext.ObjectInstance;
                    var idProperty = obj.GetType().GetProperty("Id");

                    if (idProperty != null)
                    {
                        var id = (long)idProperty.GetValue(obj);

                        if (db.CustomCategories.Where(x => x.Name == name && x.Id != id).FirstOrDefault() == null)
                        {
                            return ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult(Lang.Validations.Name_already_in_use_);
                        }
                    }
                    else {
                        
                        if (db.CustomCategories.Where(x => x.Name == name).FirstOrDefault() == null)
                        {
                            return ValidationResult.Success;
                        }
                        else
                        {
                            return new ValidationResult(Lang.Validations.Name_already_in_use_);
                        }
                    }
                   
                }
                catch (Exception)
                {

                    if (db.CustomCategories.Where(x => x.Name == name).FirstOrDefault() == null)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(Lang.Validations.Name_already_in_use_);
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
