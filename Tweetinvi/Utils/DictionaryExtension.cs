using System;
using System.Collections.Generic;

namespace Tweetinvi.Utils
{
    /// <summary>
    /// Provide Extension Methods to manage Dictionary
    /// </summary>
    public static class DictionaryExtension
    {
        /// <summary>
        /// Provide an extension that do a TryGetValue and return the result
        /// </summary>
        /// <param name="dictionary">Dictionary hosting the KeyValuePair</param>
        /// <param name="prop_name">Key to be looked for</param>
        /// <returns>Value of the key if exists</returns>
        public static object GetProp(this Dictionary<String, object> dictionary, string prop_name)
        {
            object result;
            dictionary.TryGetValue(prop_name, out result);            
            return result;
        }
    }
}
