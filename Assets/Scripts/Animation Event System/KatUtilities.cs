using System;

namespace KatLib.Utilities
{
    public static class KatUtilities
    {
        /// <summary>
        /// Use This To Avoid GC From Convert Generic Enum To Int
        /// </summary>
        /// <param name="enumValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static unsafe int GenericEnumConvertInt<T>(T enumValue) where T : unmanaged, Enum
        {
            return *(int*)&enumValue;
        }
    }
}