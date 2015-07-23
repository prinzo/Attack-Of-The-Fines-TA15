using System;

namespace FineBot.Infrastructure
{
    public static class Guard
    {
        public static void AgainstNull<T>(Func<T> func)
        {
            if(func.Method == null) throw new ArgumentNullException(string.Format("{0} is null", func.Method.Name));
        }
    }
}