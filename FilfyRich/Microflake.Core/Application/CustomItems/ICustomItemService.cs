using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.CustomItems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microflake.Core.Application.CustomItems
{
    public interface ICustomItemService
    {
        Task<ServiceResponse<List<ListProduct>>> List();

        Task<ServiceResponse<List<ListProduct>>> List(long Id);

        Task<ServiceResponse<ListProduct>> Create(CreateProduct model, string userId);

        Task<ServiceResponse<EditProduct>> Edit(long id);

        Task<ServiceResponse<ListProduct>> Update(EditProduct model, string userId);


        Task<ServiceResponse<long>> Delete(long Id);
       
    }
}
