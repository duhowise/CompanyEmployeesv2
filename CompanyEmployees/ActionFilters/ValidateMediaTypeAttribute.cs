using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

namespace CompanyEmployees.ActionFilters;

public class ValidateMediaTypeAttribute:IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var acceptHeaderPresent = context.HttpContext.Request.Headers.ContainsKey("Accept");
        if (!acceptHeaderPresent)
        {
            context.Result = new BadRequestObjectResult("Accept Header is Missing");
            return;
        }

        var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();
        if (!MediaTypeHeaderValue.TryParse(mediaType,out var mediaTypeHeaderValue))
        {
            context.Result =
                new BadRequestObjectResult(
                    "Media type not present. Please add accept header with the required media type");
            return;
        }
        context.HttpContext.Items.Add("AcceptHeaderMediaType",mediaTypeHeaderValue);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}