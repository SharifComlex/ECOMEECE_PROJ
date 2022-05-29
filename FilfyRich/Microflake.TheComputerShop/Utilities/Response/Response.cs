using Microflake.TheComputerShop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microflake.TheComputerShop.Utilities.Response
{
    public class Response : IResponse
    {
        public ServiceResponse<T> Create<T>(int result, int type = 1, T data = default(T))
        {
            var message = "";
            var success = true;

            if (result > 0)
            {
                if (type == 1)
                {
                    message = "Record has been successfully added";
                }
                else if (type == 2)
                {
                    message = "Record has been successfully updated";
                }
                else if (type == 3)
                {
                    message = "You Cannot Update Record After 24 Hours From Now";
                }
                else if (type == 4)
                {
                    success = false;
                    message = "It Is Used by another record";
                }
            }
            else
            {
                success = false;
                message = "An error has been occured.";
            }

            return new ServiceResponse<T>
            {
                Data = data,
                Message = new List<string> { message },
                Success = success
            };
        }

        public ServiceResponse<T> Create<T>(bool success = true, string message = null, T data = default(T))
        {
            return new ServiceResponse<T>
            {
                Data = data,
                Message = new List<string> { message },
                Success = success
            };
        }

        public ServiceResponse<T> Create<T>(bool success = true, List<string> message = null, T data = default(T))
        {
            return new ServiceResponse<T>
            {
                Data = data,
                Message = message,
                Success = success
            };
        }


    }
}
