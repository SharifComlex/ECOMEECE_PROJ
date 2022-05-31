using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.WhishList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.Whislists
{
  public  interface IWhislistService
    {
        Task<ServiceResponse<long>> AddProduct(long Id , string userId);

        Task<ServiceResponse<List<ListWhishlist>>> List();

        Task<ServiceResponse<int>> WhishListCount(string userId);

    }
}
