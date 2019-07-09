using System;

namespace Blog.Utils
{
    public class Its
    {
        public static T NotNull<T>(T parameter, string name = null)
        {
            if (parameter == null)
                throw new ArgumentNullException(name);
            return parameter;
        }
    }
}
