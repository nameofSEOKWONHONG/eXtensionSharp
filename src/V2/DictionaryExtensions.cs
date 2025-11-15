using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace eXtensionSharp.V2;

public static class DictionaryExtensions
{
    extension<TK, TV>(Dictionary<TK, TV> source) where TK : notnull
    {
        /// <summary>
        /// Dictionary performance improvement method, (Nick Chapsas - Fixing Your Dictionary Performance Problem in .NETFixing Your Dictionary Problem in .NET -)
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public TV xGetOrAdd(TK key, TV value)            
        {
            ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out var exists);
            if (exists)
            {
                return val;
            }
            val = value;
            return value;
        }

        /// <summary>
        /// Dictionary performance improvement method (Nick Chapsas - Fixing Your Dictionary Performance Problem in .NETFixing Your Dictionary Problem in .NET -)
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public bool xTryUpdate(TK key, TV value)            
        {
            ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(source, key);
            if (Unsafe.IsNullRef(ref val)) return false;

            val = value;
            return true;
        }        
    }
}
