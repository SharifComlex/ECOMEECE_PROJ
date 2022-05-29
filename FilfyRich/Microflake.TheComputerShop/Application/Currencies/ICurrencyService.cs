using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Currency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Application.Currencies
{
   public interface ICurrencyService
    {
        Task<ServiceResponse<EditCurrency>> Edit(long id);
        Task<ServiceResponse<ListCurrency>> Update(EditCurrency model, string userId);

        Task<ServiceResponse<List<ListCurrency>>> List();
    }
}
