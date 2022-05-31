using Microflake.Core.Domain;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Category;
using Microflake.Core.ViewModel.SubCategories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Microflake.Core.Application.SubCategories
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
