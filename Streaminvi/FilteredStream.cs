using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Streaminvi.Properties;
using TweetinCore.Enum;
using TweetinCore.Interfaces;
using TweetinCore.Interfaces.StreamInvi;

namespace Streaminvi
{
    /// <summary>
    /// Stream filtering the objects from a stream
    /// </summary>
    public class FilteredStream : BaseStream, IFilteredStream
    {
        private readonly HashSet<string> _trackKeywords;

        public FilteredStream()
        {
            _trackKeywords = new HashSet<string>();
        } 

        public void AddTrack(string track)
        {
            _trackKeywords.Add(track);
        }

        public void RemoveTrack(string track)
        {
            _trackKeywords.Remove(track);
        }

        public void ClearTrack()
        {
            _trackKeywords.Clear();
        } 

        public override void StartStream(IToken token, Func<ITweet, bool> processTweetDelegate)
        {
            Func<HttpWebRequest> generateWebRequest = delegate
            {
                StringBuilder queryBuilder = new StringBuilder(Resources.Stream_Filter);

                if (_trackKeywords.Any())
                {
                    queryBuilder.Append("track=");
                    for (int i = 0; i < _trackKeywords.Count - 1; ++i)
                    {
                        queryBuilder.Append(Uri.EscapeDataString(String.Format("{0},", _trackKeywords.ElementAt(i))));
                    }

                    queryBuilder.Append(Uri.EscapeDataString(_trackKeywords.ElementAt(_trackKeywords.Count - 1)));
                }

                return token.GetQueryWebRequest(queryBuilder.ToString(), HttpMethod.POST);
            };

            _streamResultGenerator.StartStream(DefaultObjectProcessor(processTweetDelegate), generateWebRequest);
        }
    }
}
