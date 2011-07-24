using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Shamrock_WebSite.Infrastructure
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this IEnumerable<T> collection)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var count = collection.Count();
            return collection.ElementAt(random.Next(0, count));
        }
    }
}