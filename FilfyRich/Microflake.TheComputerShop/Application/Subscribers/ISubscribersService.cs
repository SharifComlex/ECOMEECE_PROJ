using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Application.Subscribers
{
   public interface ISubscribersService
    {

        Task<ServiceResponse<List<ListSubscriber>>> ToList();
    }
}
