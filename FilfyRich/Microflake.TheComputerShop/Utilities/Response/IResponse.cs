using Microflake.TheComputerShop.ViewModel;
using System.Collections.Generic;

namespace Microflake.TheComputerShop.Utilities.Response
{
    public interface IResponse
    {
        ServiceResponse<T> Create<T>(int result, int type = 1, T data = default(T));
        ServiceResponse<T> Create<T>(bool success = true, string message = null, T data = default(T));
        ServiceResponse<T> Create<T>(bool success = true, List<string> message = null, T data = default(T));
    }
}
