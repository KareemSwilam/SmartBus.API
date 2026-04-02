using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBus.Application.Result
{
    public class CustomError
    {
        public string Message { get;  private set; }
        public String Code { get; private  set; }
        private CustomError (string message, String code)
        {
            Message = message;
            Code = code;
        }
        protected static readonly string _invalidInput = "Not Valid Input";
        protected static readonly string _notFound = "Record Not Found";
        protected static readonly string _serverError = "Error In Server";
        public static CustomError InvalidInput(string message) => new CustomError(message, _invalidInput);
        public static CustomError NotFound(string message) => new CustomError(message, _invalidInput);
        public static CustomError ServerError(string message) => new CustomError(message, _invalidInput);
        public override string ToString()
        {
            return $"Code: {Code},\n Message: {Message}";
        }
    }
}
