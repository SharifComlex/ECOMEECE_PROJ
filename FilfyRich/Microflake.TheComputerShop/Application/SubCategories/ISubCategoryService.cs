using Microflake.TheComputerShop.Domain;
using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Category;
using Microflake.TheComputerShop.ViewModel.SubCategories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Microflake.TheComputerShop.Application.SubCategories
{
   public interface ISubCategoryService
    {
        Task<ServiceResponse<List<ListSubCategory>>> List();

        Task<ServiceResponse<SelectList>> SelectList(long? Id);

        Task<ServiceResponse<SubCategory>> Get(long Id);

        Task<ServiceResponse<ListSubCategory>> Create(CreateSubCategory model, string userId);

        Task<ServiceResponse<EditSubCategory>> Edit(long id);

        Task<ServiceResponse<ListSubCategory>> Update(EditSubCategory model, string userId);


        Task<ServiceResponse<long>> Delete(long Id);
    }
}
