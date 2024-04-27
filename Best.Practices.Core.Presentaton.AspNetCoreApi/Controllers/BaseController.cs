using Best.Practices.Core.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Best.Practices.Core.Presentaton.AspNetCoreApi.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected IActionResult OutputConverter<Response>(UseCaseOutput<Response> useCaseOutput)
        {
            ObjectResult result;

            if (useCaseOutput.HasErros)
            {
                result = new ObjectResult(useCaseOutput.Errors)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

                return result;
            }
            else
            {
                result = new ObjectResult(useCaseOutput.OutputObject)
                {
                    StatusCode = GetStatusCodeFromMethodVerb(HttpContext.Request.Method.ToUpper(), useCaseOutput)
                };
            }

            return result;
        }

        protected string GetAuthorizedUser()
        {
            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name);

            return user?.Value ?? string.Empty;
        }

        private static int GetStatusCodeFromMethodVerb<Response>(string methodVerb, UseCaseOutput<Response> useCaseResponse)
        {
            //https://restfulapi.net/http-methods/#:~:text=4.1.&text=A%20successful%20response%20of%20DELETE,the%20action%20has%20been%20queued.
            return methodVerb switch
            {
                "POST" => (useCaseResponse.OutputObject is null) ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.Created,
                "PUT" => (useCaseResponse.OutputObject is null) ? (int)HttpStatusCode.NoContent : (int)HttpStatusCode.Created,
                "PATCH" => (int)HttpStatusCode.Accepted,
                "DELETE" => (int)HttpStatusCode.OK,
                "GET" => (int)HttpStatusCode.OK,
                _ => (int)HttpStatusCode.OK
            };
        }
    }
}
