using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.Orders
{
   public interface IOrderService
    {
        Task<ServiceResponse<EditOrder>> Edit(long id);
        Task<ServiceResponse<OrderDetail>> Detail(long id);
        Task<ServiceResponse<ListOrder>> Update(EditOrder model, string userId);

        Task<ServiceResponse<List<ListOrder>>> List();
    }
}
