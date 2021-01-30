using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // This controller is used to show consistent errror if no endpoint match 
    // we have managed this using middleware.
    [Route("errors/{code}")]
    public class ErrorController:BaseApiController
    {
      public IActionResult Error(int code)
      {
          return new ObjectResult(new ApiResponse(code));
      }


    }
}