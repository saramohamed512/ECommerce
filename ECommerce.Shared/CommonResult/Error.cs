using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.CommonResult
{
    public class Error
    {
        public string Code { get; }
        public string Description { get;  }
        public ErrorType Type { get; }
        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }
        public static Error Failure(string code="General Failure", string description = "General Failure Occured!")
        {
            return new Error(code, description, ErrorType.Failure);
        }
        public static Error Validation(string code = "Validation Failure", string description = "Validation Failure Occured!")
        {
            return new Error(code, description, ErrorType.Validation);
        }
        public static Error NotFound(string code = "Not Found", string description = "Requested Resource Not Found!")
        {
            return new Error(code, description, ErrorType.NotFound);
        }
        public static Error Unauthorized(string code = "Unauthorized", string description = "You are not authorized to access this resource!")
        {
            return new Error(code, description, ErrorType.Unauthorized);
        }
        public static Error Forbidden(string code = "Forbidden", string description = "Access to this resource is forbidden!")
        {
            return new Error(code, description, ErrorType.Forbidden);
        }
        public static Error InvalidCredentials(string code = "Invalid Credentials", string description = "The provided credentials are invalid!")
        {
            return new Error(code, description, ErrorType.InvalidCredentials);
        }



    }
}
