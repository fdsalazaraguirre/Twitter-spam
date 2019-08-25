using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TweetinCore.Interfaces;
using Tweetinvi.Utils;

namespace Tweetinvi.TwitterEntities
{
    /// <summary>
    /// Object storing information related with an user mention on Twitter
    /// </summary>
    public class UserMentionEntity : IUserMentionEntity
    {
        #region Private Attributes

        private long? _id;
        private string _id_str;
        private string _screen_name;
        private string _name;
        private IList<int> _indices;

        #endregion

        #region Public Attributes

        public long? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string IdStr
        {
            get { return _id_str; }
            set { _id_str = value; }
        }

        public string ScreenName
        {
            get { return _screen_name; }
            set { _screen_name = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public IList<int> Indices
        {
            get { return _indices; }
            set { _indices = value; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an empty user mention entity
        /// </summary>
        public UserMentionEntity() { }

        /// <summary>
        /// Creates a UserMentionEntity based on information retrieved from Twitter
        /// </summary>
        /// <param name="userMentionEntity">Information retrieved from Twitter</param>
        public UserMentionEntity(Dictionary<String, object> userMentionEntity)
        {
            Id = userMentionEntity.GetProp("id") as long?;
            IdStr = userMentionEntity.GetProp("id_str") as string;
            ScreenName = userMentionEntity.GetProp("screen_name") as string;
            Name = userMentionEntity.GetProp("name") as string;

            Indices = new ObservableCollection<int>();

            var mentionIndices = userMentionEntity.GetProp("indices") as object[];
            if (mentionIndices != null)
            {
                foreach (int indice in mentionIndices)
                {
                    Indices.Add(indice);
                }
            }
        }
        #endregion
    }
}
