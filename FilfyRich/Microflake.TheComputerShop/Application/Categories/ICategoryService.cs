using Microflake.Core.Domain;
using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Category;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Microflake.Core.Application.Categories
{
    public interface ICategoryService
    {
        Task<ServiceResponse<SelectList>> SelectList(long? Id);

        Task<ServiceResponse<List<ListCategory>>> ToList();
        
        Task<ServiceResponse<ListCategory>> Create(CreateCategory model, string userId);
        
        Task<ServiceResponse<EditCategory>> Edit(long id);
        
        Task<ServiceResponse<ListCategory>> Update(EditCategory model, string userId);
        
        Task<ServiceResponse<long>> Remove(long Id);
    }
}
