using Microflake.TheComputerShop.Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.ViewModel.Category.Validations
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
                    var id = (long)obj.GetType().GetProperty("Id").GetValue(obj);

                    if (db.Categories.Where(x => x.English == name && x.Id != id).FirstOrDefault() == null)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult(Lang.Validations.Name_already_in_use_);
                    }
                }
                catch (Exception)
                {

                    if (db.Categories.Where(x => x.English == name).FirstOrDefault() == null)
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
