﻿using Blog.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Blog.Utils
{
   public static class StringExtensions
   {
      public static string ReplaceWithPattern(this string value, string pattern, string replacement) =>
         Regex.Replace(value, pattern, replacement);

      public static string ThrowIfNullOrEmpty(this string value, string message = null)
      {
         if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(message);
         return value;
      }

      public static string ThrowIfNullOrEmpty<T>(this string value, string message)
      {
         Exception exc = new ArgumentNullException(message);
         if (typeof(T) == typeof(InvalidOperationException))
            exc = new InvalidOperationException(message);

         if (string.IsNullOrEmpty(value))
            throw exc;

         return value;
      }

      public static void AddModelErrors(this ModelStateDictionary dic, IEnumerable<Problem> problems)
      {
         foreach (var prob in problems)
            dic.AddModelError(prob.Property, prob.Message);
      }
   }
}