using Microflake.TheComputerShop.ViewModel;
using Microflake.TheComputerShop.ViewModel.ContactUss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Application.ContactUs
{
  public  interface IContactUsService
    {

        Task<ServiceResponse<List<ListContactUs>>> List();
    }
}
