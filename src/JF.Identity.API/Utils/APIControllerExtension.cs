using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JF.Identity.API.Utils
{
    public static class APIControllerExtension
    {
        public static IActionResult Error(this ControllerBase controller, string errorCode)
        {
            return controller.StatusCode(StatusCodes.Status400BadRequest, errorCode);
        }
    }
}
