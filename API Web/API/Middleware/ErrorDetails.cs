
using Newtonsoft.Json;

namespace API.Middleware
{
    public class ErrorDetails
    {
        public string message { get; set; }
        
        public int statusCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}