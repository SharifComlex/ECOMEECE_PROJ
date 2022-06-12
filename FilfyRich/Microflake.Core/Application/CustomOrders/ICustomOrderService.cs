using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.CustomOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.CustomOrders
{
  public  interface ICustomOrderService
    {
        Task<ServiceResponse<EditCustomOrder>> Edit(long id);
        Task<ServiceResponse<ListCustomOrder>> Update(EditCustomOrder model, string userId);

        Task<ServiceResponse<List<ListCustomOrder>>> List();
    }
}
