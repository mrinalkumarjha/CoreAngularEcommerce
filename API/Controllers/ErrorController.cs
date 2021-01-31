using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // This controller is used to show consistent errror if no endpoint match 
    // we have managed this using middleware.
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)] // for swagger . swagger wil not produce this endpoint in its documentation
    public class ErrorController:BaseApiController
    {
      public IActionResult Error(int code)
      {
          return new ObjectResult(new ApiResponse(code));
      }


    }
}