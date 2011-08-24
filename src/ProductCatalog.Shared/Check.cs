using System;

namespace ProductCatalog.Shared
{
    public static class Check
    {
        public static void IsNotNull(object o, string name)
        {
            if (o == null)
            {
                throw new NullReferenceException(string.Format("{0} cannot be null.", name));
            }
        }

        public static void IsGreaterThanZero(int v, string name)
        {
            if (v <= 0)
            {
                throw new ArgumentException(string.Format("{0} must be greater than zero.", name));
            }
        }
    }
}