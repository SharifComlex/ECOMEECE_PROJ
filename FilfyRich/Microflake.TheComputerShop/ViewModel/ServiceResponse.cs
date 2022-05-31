using System.Collections.Generic;

namespace Microflake.Core.ViewModel
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public List<string> Message { get; set; } = null;
        public T Data { get; set; } = default(T);
    }
}
