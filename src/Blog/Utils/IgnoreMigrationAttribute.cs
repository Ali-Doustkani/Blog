using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Blog.Utils
{
   public class IgnoreMigrationAttribute : Attribute, IFilterMetadata { }
}
