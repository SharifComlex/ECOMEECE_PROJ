using Microflake.Core.ViewModel;
using Microflake.Core.ViewModel.ContactUss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.Core.Application.ContactUs
{
  public  interface IContactUsService
    {

        Task<ServiceResponse<long>> Remove(long Id);
        Task<ServiceResponse<List<ListContactUs>>> List();
    }
}
