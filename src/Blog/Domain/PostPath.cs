using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public static class PostPath
    {
        public static string Increment(string filename)
        {
            var reg = Regex.Match(Path.GetFileNameWithoutExtension(filename), @"^(?<name>.+)-(?<num>\d+)$");
            var name = reg.Groups["name"].Value;
            var num = reg.Groups["num"].Value;
            name = string.IsNullOrEmpty(name) ? Path.GetFileNameWithoutExtension(filename) : name;
            num = string.IsNullOrEmpty(num) ? "0" : num;
            var newNum = Convert.ToInt32(num) + 1;
            return $"{name}-{newNum}{Path.GetExtension(filename)}";
        }
    }
}
