using BikeStoresApi.Configuration.Logger;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BikeStoresApi.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        [Route("error")]
        public ErrorDetails Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context.Error; // Your exception
            var code = 500; // Internal Server Error by default

         

            Response.StatusCode = code; // You can use HttpStatusCode enum instead

            return new ErrorDetails() { StatusCode = code, Message = exception.Message }; // Your error model
        }
    }
}
