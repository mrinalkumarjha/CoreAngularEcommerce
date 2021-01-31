using System.Collections.Generic;

namespace API.Errors
{
    // This is for handling 400 error with error list
    public class ApiValidationErrorResponse : ApiResponse
    {
        public ApiValidationErrorResponse() : base(400)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}