using Blog.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Utils
{
   public class ValidationFilter : ActionFilterAttribute
   {
      public override void OnActionExecuting(ActionExecutingContext context)
      {
         if (context.ActionArguments.Count == 0)
            return;

         if (context.ActionArguments.Count > 1)
            throw new InvalidOperationException("Current validation only supports one argument");

         var builder = new ValidationResponseBuilder();
         builder.BuildFrom(context.ActionArguments.First().Value);
         if (builder.Invalid)
            context.Result = new BadRequestObjectResult(builder.Result);
      }
   }
}
