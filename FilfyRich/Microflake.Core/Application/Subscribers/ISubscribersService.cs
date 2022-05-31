using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.Subscribers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.Subscribers
{
   public interface ISubscribersService
    {

        Task<ServiceResponse<List<ListSubscriber>>> ToList();
    }
}
