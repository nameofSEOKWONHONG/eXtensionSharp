using System;
using System.Collections;
using System.Linq.Expressions;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eXtensionSharp.V2;

public static class CollectionStringExtentions
{
    extension(IEnumerable<string> source)
    {
        public string xToMySqlRLikeString()
        {
            if(source.xIsEmpty()) return string.Empty;
            return source.xJoin("|");
        }

        public string xJoin(string seperate = ",")
        {
            if(source.xIsEmpty()) return string.Empty;
            return string.Join(seperate, source);
        }
    }
}

