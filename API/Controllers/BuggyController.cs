using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            this._context = context;
        }

        [HttpGet("notfound")]
        public ActionResult GetNotFound()
        {
            var thing = _context.Products.Find(525);
            if(thing == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        
        [HttpGet("servererror")]
        public ActionResult servererror()
        {
             var thing = _context.Products.Find(525);
             var ret = thing.ToString();
            return Ok();
        }

        
        [HttpGet("badrequest")]
        public ActionResult badrequest()
        {
            return badrequest();
        }

         [HttpGet("badrequest/{id}")]
        public ActionResult badrequest(int id)
        {
            return badrequest();
        }


          [HttpGet("testauth")]
          [Authorize]
        public ActionResult<string> GetSecreatText()
        {
            return "Your secreat text";
        }

    }
}