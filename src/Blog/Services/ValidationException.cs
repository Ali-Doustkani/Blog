using System;

namespace Blog.Services
{
    public class ValidationException : Exception
    {
        public ValidationException(string key, string message)
            : base(message)
        {
            Key = key;
        }

        public string Key { get; }
    }
}
