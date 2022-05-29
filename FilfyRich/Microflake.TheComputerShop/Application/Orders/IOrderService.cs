using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Application.Orders
{
   public interface IOrderService
    {
        Task<ServiceResponse<EditOrder>> Edit(long id);
        Task<ServiceResponse<ListOrder>> Update(EditOrder model, string userId);

        Task<ServiceResponse<List<ListOrder>>> List();
    }
}
