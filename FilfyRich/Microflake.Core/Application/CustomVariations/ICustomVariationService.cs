using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.CustomVariations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microflake.Core.Application.CustomVariations
{
    public interface ICustomVariationService
    {
        Task<ServiceResponse<List<ListVariation>>> List();
       
        Task<ServiceResponse<ListVariation>> Create(CreateVariation model, string userId);

        Task<ServiceResponse<EditVariation>> Edit(long id);

        Task<ServiceResponse<ListVariation>> Update(EditVariation model, string userId);


        Task<ServiceResponse<long>> Delete(long Id);
       
    }
}
