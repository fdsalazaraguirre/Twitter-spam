using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Tweetinvi.Utils
{
    /// <summary>
    /// Extension methods for IEnumerable
    /// </summary>
    public static class IEnumerableExtension
    {
        /// <summary>
        /// Transform a IEnumerable into an ObservableCollection
        /// </summary>
        /// <typeparam name="T">Type of object hosted by the collection</typeparam>
        /// <param name="enumerableList">Current collection</param>
        /// <returns>New observable collection</returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerableList)
        {
            if (enumerableList != null)
            {
                var observableCollection = new ObservableCollection<T>();

                foreach (var item in enumerableList)
                {
                    observableCollection.Add(item);
                }

                return observableCollection;
            }

            return null;
        }
    }
}
