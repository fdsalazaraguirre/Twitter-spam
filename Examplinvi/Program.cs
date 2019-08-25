using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using Streaminvi;
using TweetinCore.Enum;
using TweetinCore.Events;
using TweetinCore.Interfaces;
using Tweetinvi;
using TwitterToken;
using UILibrary;
using System.Windows;
using System.Threading;
using System.IO;
using System.Collections;

namespace Examplinvi
{
    class Program
    {
        // ReSharper disable UnusedMember.Local
        #region Token

        #region Execute Query
        /// <summary>
        /// Simple function that uses ExecuteQuery to retrieve information from the Twitter API
        /// </summary>
        /// <param name="token"></param>

        static void ExecuteQuery(IToken token)
        {
            // Retrieving information from Twitter API through Token method ExecuteRequest
            Dictionary<string, object>[] timeline = token.ExecuteGETQueryReturningCollectionOfObjects("https://api.twitter.com/1.1/statuses/home_timeline.json");

            // Working on each different object sent as a response to the Twitter API query
            for (int i = 0; i < timeline.Length; ++i)
            {
                Dictionary<String, object> post = timeline[i];
                Console.WriteLine("{0} : {1}\n", i, post["text"]);
            }
        }

        /// <summary>
        /// Function that execute cursor query and send information for each query executed
        /// </summary>
        /// <param name="token"></param>
        static void ExecuteCursorQuery(IToken token)
        {
            // The delegate is a function that will be called for each cursor
            DynamicResponseDelegate del = delegate(Dictionary<string, object> jsonResponse, long previousCursor, long nextCursor)
            {
                Console.WriteLine(previousCursor + " -> " + nextCursor + " : " + jsonResponse.Count);

                return jsonResponse.Count;
            };

            token.ExecuteCursorQuery("https://api.twitter.com/1.1/friends/ids.json?user_id=700562792", del);
        }
        #endregion

        #region ErrorHandling

        /// <summary>
        /// Testing the 3 ways to handle errors
        /// </summary>
        /// <param name="token"></param>
        static void TestErrorFunctions(IToken token)
        {
            integrated_error_handler(token);
            token_integrated_error_handler(token);
            execute_query_error_handler(token);
        }

        /// <summary>
        /// Initiating auto error handler
        /// You will not receive error information if handled by default error handler
        /// </summary>
        /// <param name="token"></param>
        static void integrated_error_handler(IToken token)
        {
            token.IntegratedExceptionHandler = true;

            // Error is not automatically handled

            try
            {
                // Calling a method that does not exist
                token.ExecuteGETQuery("https://api.twitter.com/1.1/users/contributors.json?user_id=700562792");
            }
            catch (WebException wex)
            {
                Console.WriteLine("An error occured!");
                Console.WriteLine(wex);
            }
        }

        /// <summary>
        /// When assigning an error_handler to a Token think that it will be kept alive 
        /// until you specify it does not exist anymore by specifying :
        /// 
        /// token.Integrated_Exception_Handler = false;
        /// 
        /// You can assign null value if you do not want anything to be performed for you
        /// </summary>
        /// <param name="token"></param>
        static void token_integrated_error_handler(IToken token)
        {
            token.ExceptionHandler = delegate(WebException wex)
            {
                Console.WriteLine("You received a Token generated error!");
                Console.WriteLine(wex.Message);
            };

            // Calling a method that does not exist
            token.ExecuteGETQuery("https://api.twitter.com/1.1/users/contributors.json?user_id=700562792");

            // Reset to basic Handler
            token.IntegratedExceptionHandler = false;
            // OR
            token.ResetExceptionHandler();
        }

        /// <summary>
        /// Uses the handler for only one query / work also for cursor queries
        /// </summary>
        /// <param name="token"></param>
        static void execute_query_error_handler(IToken token)
        {
            WebExceptionHandlingDelegate del = delegate(WebException wex)
            {
                Console.WriteLine("You received an execute_query_error!");
                Console.WriteLine(wex.Message);
            };

            token.ExecuteGETQuery("https://api.twitter.com/1.1/users/contributors.json?user_id=700562792", null, del);
        }

        #endregion

        #region Rate-Limit

        /// <summary>
        /// Enable you to Get all information from Token and how many query you can execute
        /// Each time a query is executed the XRateLimitRemaining is updated.
        /// To improve efficiency, the other values are NOT.
        /// If you need these please call the function GetRateLimit()
        /// </summary>
        /// <param name="token"></param>
        static void GetRateLimit(IToken token)
        {
            int remaining = token.GetRateLimit();
            Console.WriteLine("Used : " + remaining);
            Console.WriteLine("Used : " + token.XRateLimitRemaining);
            Console.WriteLine("Total per hour : " + token.XRateLimit);
            Console.WriteLine("Time before reset : " + token.XRateLimitResetTimeInSeconds);
        }

        #endregion

        #endregion

        #region Stream

        private static readonly List<ITweet> _streamList = new List<ITweet>();
        private static void ProcessTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                return;
            }

            if (_streamList.Count % 125 != 124)
            {
                Console.WriteLine("{0} : \"{1}\"", tweet.Creator.Name, tweet.Text);
                _streamList.Add(tweet);
            }
            else
            {
                Console.WriteLine("Processing data");
                _streamList.Clear();
            }
        }

        private static void StreamingExample(IToken token)
        {
            // Creating the stream and specifying the delegate
            SimpleStream myStream = new SimpleStream("https://stream.twitter.com/1.1/statuses/sample.json");
            // Starting the stream by specifying credentials thanks to the Token
            myStream.StartStream(token, x => ProcessTweet(x));
        }

        private static void StreamFilterExample(IToken token)
        {
            FilteredStream stream = new FilteredStream();

            // Adding Tracks filters
            stream.AddTrack("Tweetinvi Linvi");
            // stream.AddTrack("Linvi");

            stream.StartStream(token, x => ProcessTweet(x));
        }

        #endregion

        #region User

        #region CreateUser
        public static void CreateUser(IToken token, long id = 700562792)
        {
            IUser user = new User(id, token);
            Console.WriteLine(user.ScreenName);
        }

        public static void CreateUser(IToken token, string screenName = null)
        {
            IUser user = new User(screenName, token);
            Console.WriteLine(user.Id);
        }

        public static void CreateUserV2(IToken token, long id = 700562792)
        {
            IUser user = new User(id);
            // Here we need to specify the token to retrieve the information
            // otherwise the information won't be filled
            user.PopulateUser(token);
        }
        #endregion

        #region Get Friends

        public static void GetFriends(IToken token)
        {
            TokenUser u = new TokenUser(token);
            List<IUser> friends = u.GetFriends();

            foreach (var friend in friends)
            {
                Console.WriteLine(friend.Name);
            }
        }

        private static void GetFriendIds(IToken token, long id = 700562792)
        {
            User user = new User(id);
            var res = user.GetFriendIds(token);
            Console.WriteLine("List of friends from " + id);

            foreach (long friend_id in res)
                Console.WriteLine(friend_id);
        }

        private static void GetFriendIdsUsingUsername(IToken token, string username)
        {
            IUser user = new User(username);
            var res = user.GetFriendIds(token);
            Console.WriteLine("List of friends from " + username);

            foreach (long friend_id in res)
                Console.WriteLine(friend_id);
        }
        #endregion

        #region Get Followers

        private static void GetFollowerIds(IToken token, long? id = 700562792)
        {
            IUser user = new User(id);
            var res = user.GetFollowerIds(token);

            Console.WriteLine(res.Count());
            foreach (long follower_id in res)
            {
                Console.WriteLine(follower_id);
            }
        }

        private static void GetFollowerIdsUsingUsername(IToken token, string username)
        {
            IUser user = new User(username);
            var res = user.GetFollowerIds(token);

            Console.WriteLine(res.Count());
            foreach (long followerId in res)
            {
                Console.WriteLine(followerId);
            }
        }

        public static void GetFollowers(IToken token)
        {
            TokenUser u = new TokenUser(token);
            List<IUser> followers = u.GetFollowers();

            foreach (var follower in followers)
            {
                Console.WriteLine(follower.Name);
            }
        }

        #endregion

        #region Get Profile Image

        static void GetProfileImage(IToken token)
        {
            User ladygaga = new User("ladygaga", token);
            string filePath = ladygaga.DownloadProfileImage(ImageSize.original);

            System.Diagnostics.Process.Start(filePath);
        }

        #endregion

        #region Get Contributors
        static void GetContributors(IToken token, long? id = 700562792, string screen_name = null, bool createContributorList = false)
        {
            IUser user;

            if (id != null)
            {
                user = new User(id, token);
            }
            else
            {
                user = new User(screen_name, token);
            }

            IList<IUser> contributors = user.GetContributors(createContributorList);
            IList<IUser> contributorsAttribute = user.Contributors;
            if (createContributorList && contributors != null)
            {
                if (contributorsAttribute == null ||
                    !contributors.Equals(contributorsAttribute))
                {
                    Console.WriteLine("The object attribute should be identical to the method result");
                }
            }
            if (contributors != null)
            {
                foreach (User c in contributors)
                {
                    Console.WriteLine("contributor id = " + c.Id + " - screen_name = " + c.ScreenName);
                }
            }
        }
        #endregion

        #region Get Contributees

        static void GetContributees(IToken token, long? id = 700562792, string screen_name = null, bool createContributeeList = false)
        {
            IUser user;
            if (id != null)
            {
                user = new User(id, token);
            }
            else
            {
                user = new User(screen_name, token);
            }
            IList<IUser> contributees = user.GetContributees(createContributeeList);
            IList<IUser> contributeesAttribute = user.Contributees;
            if (createContributeeList)
            {
                if ((contributees == null && contributeesAttribute != null) ||
                    (contributees != null && contributeesAttribute == null) ||
                    (contributees != null && !contributees.Equals(contributeesAttribute)))
                {
                    Console.WriteLine("The object attribute should be identical to the method result");
                }
            }
            if (contributees != null)
            {
                foreach (User c in contributees)
                {
                    Console.WriteLine("contributee id = " + c.Id + " - screen_name = " + c.ScreenName);
                }
            }
        }
        #endregion

        #region Get Direct Messages Sent
        static void GetDirectMessagesSent(IToken token)
        {
            ITokenUser user = new TokenUser(token);
            IList<IMessage> dmSent = user.GetLatestDirectMessagesSent();
            IList<IMessage> dmSentAttribute = user.LatestDirectMessagesSent;


            if ((dmSent == null && dmSentAttribute != null) ||
                (dmSent != null && dmSentAttribute == null) ||
                (dmSent != null && !dmSent.Equals(dmSentAttribute)))
            {
                Console.WriteLine("The object's attribute should be identical to the method result");
            }

            if (dmSent != null)
            {
                foreach (Message m in dmSent)
                {
                    Console.WriteLine("message id = " + m.Id + " - text = " + m.Text);
                }
            }
        }
        #endregion

        #region Get Direct Received
        static void GetDirectMessagesReceived(IToken token)
        {
            ITokenUser user = new TokenUser(token);
            IList<IMessage> dmReceived = user.GetLatestDirectMessagesReceived();
            IList<IMessage> dmReceivedAtrribute = user.LatestDirectMessagesReceived;

            if ((dmReceived == null && dmReceivedAtrribute != null) ||
                (dmReceived != null && dmReceivedAtrribute == null) ||
                (dmReceived != null && !dmReceived.Equals(dmReceivedAtrribute)))
            {
                Console.WriteLine("The object's attribute should be identical to the method result");
            }

            if (dmReceived != null)
            {
                foreach (Message m in dmReceived)
                {
                    Console.WriteLine("message id = " + m.Id + " - text = " + m.Text);
                }
            }
        }
        #endregion

        #region GetHomeTimeline

        static void GetHomeTimeline(IToken token)
        {
            ITokenUser u = new TokenUser(token);
            IList<ITweet> homeTimeline = u.GetHomeTimeline(20, true, true);

            Console.WriteLine(u.LatestHomeTimeline.Count);

            foreach (ITweet tweet in homeTimeline)
            {
                Console.WriteLine("\n\n{0}", tweet.Text);
            }
        }

        #endregion

        #region Get Timeline
        static void GetTimeline(IToken token, long id = 700562792, bool createTimeline = false)
        {
            IUser user = new User(id, token);
            IList<ITweet> timeline = user.GetUserTimeline(createTimeline);
            IList<ITweet> timelineAttribute = user.Timeline;
            if (createTimeline)
            {
                if ((timeline == null && timelineAttribute != null) ||
                    (timeline != null && timelineAttribute == null) ||
                    (timeline != null && !timeline.Equals(timelineAttribute)))
                {
                    Console.WriteLine("The object's attribute should be identical to the method result");
                }
            }
            if (timeline != null)
            {
                foreach (Tweet t in timeline)
                {
                    Console.WriteLine("tweet id = " + t.Id + " - text = " + t.Text + " - is retweet = " + t.Retweeted);
                }
            }
        }
        #endregion

        #region Get Mentions
        static void GetMentions(IToken token)
        {
            ITokenUser user = new TokenUser(token);
            IList<IMention> mentions = user.GetLatestMentionsTimeline();
            IList<IMention> mentionsAttribute = user.LatestMentionsTimeline;


            if ((mentions == null && mentionsAttribute != null) ||
                (mentions != null && mentionsAttribute == null) ||
                (mentions != null && !mentions.Equals(mentionsAttribute)))
            {
                Console.WriteLine("The object's attribute should be identical to the method result");
            }

            if (mentions != null)
            {
                foreach (Mention m in mentions)
                {
                    Console.WriteLine("tweet id = " + m.Id + " - text = " + m.Text + " - annotations = " + m.Annotations);
                }
            }
        }

        #endregion

        #region Get Blocked users
        static void GetBlockedUsers(IToken token, bool createBlockedUsers = true, bool createdBlockedUsersIds = true)
        {
            TokenUser user = new TokenUser(token);

            IList<IUser> blockedUsers = user.GetBlockedUsers(createBlockedUsers, createdBlockedUsersIds);
            if (blockedUsers == null)
            {
                return;
            }
            if (createBlockedUsers)
            {
                if (blockedUsers != user.BlockedUsers)
                {
                    Console.WriteLine("The object's attribute should be identical to the method result");
                }
            }

            foreach (IUser bu in blockedUsers)
            {
                Console.WriteLine("user id = " + bu.Id + " - user screen name = " + bu.ScreenName);
            }
        }

        static void GetBlockedUsersIds(IToken token, bool createdBlockedUsersIds = true)
        {
            TokenUser user = new TokenUser(token);

            IList<long> ids = user.GetBlockedUsersIds(createdBlockedUsersIds);
            if ((createdBlockedUsersIds) && (ids != user.BlockedUsersIds))
            {
                Console.WriteLine("The object's attribute should be identical to the method result");
            }

            foreach (long id in ids)
            {
                Console.WriteLine("user id = " + id);
            }
        }
        #endregion

        #region Get Suggested User (list and members)
        static void GetSuggestedUserList(IToken token, bool createSuggestedUserList = true)
        {
            ITokenUser user = new TokenUser(token);

            IList<ISuggestedUserList> suggUserList = user.GetSuggestedUserList(createSuggestedUserList);
            if ((createSuggestedUserList) && (!suggUserList.Equals(user.SuggestedUserList)))
            {
                Console.WriteLine("The object's attribute should be identical to the method result");
            }

            foreach (ISuggestedUserList sul in suggUserList)
            {
                Console.WriteLine("name = " + sul.Name + " ; slug = " + sul.Slug + " ; size = " + sul.Size);
            }

        }

        static void GetSuggestedUserListDetails(IToken token, string slug)
        {
            SuggestedUserList sul = new SuggestedUserList("fake", slug, 0);
            sul.RefreshAll(token);

            Console.WriteLine("name = " + sul.Name + " ; slug = " + sul.Slug + " ; size = " + sul.Size);
            foreach (User su in sul.Members)
            {
                Console.WriteLine("Suggested user: id = " + su.Id + " ; screen name = " + su.ScreenName);
            }
        }

        static void GetSuggestedUserListMembers(IToken token, string slug)
        {
            SuggestedUserList sul = new SuggestedUserList("fake", slug, 0);
            sul.RefreshMembers(token);

            foreach (User su in sul.Members)
            {
                Console.WriteLine("Suggested user: id = " + su.Id + " ; screen name = " + su.ScreenName);
            }
        }
        #endregion

        #endregion

        #region Tweet

        #region Publish Tweet

        public static void PublishTweet(IToken token, string message)
        {
            ITweet t = new Tweet(message);
            // token.Integrated_Exception_Handler = true;
            Console.WriteLine("Tweet has{0}been published", t.Publish(token) ? " " : " not ");
        }

        public static void PublishTweetWithGeo(IToken token)
        {
            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi With Geo {0}", DateTime.Now));

            double latitude = 37.7821120598956;
            double longitude = -122.400612831116;

            // Send the Tweet
            Console.WriteLine("Tweet has{0}been published",
                tweet.PublishWithGeo(latitude, longitude, true, token) ? " " : " not ");
        }

        public static void PublishInReplyTo(IToken token)
        {
            // Create Tweet locally
            ITweet tweet = new Tweet(String.Format("Hello Tweetinvi {0}", DateTime.Now), token);

            // Send the Tweet
            bool result = tweet.Publish();

            if (result)
            {
                ITweet reply = new Tweet(String.Format("Nice speech Tweetinvi {0}", DateTime.Now), token);

                result &= reply.PublishInReplyTo(tweet);
            }

            Console.WriteLine(result);
        }

        #endregion

        #region Retrieve an existing Tweet
        private static void GetTweetById(IToken token)
        {
            // This tweet has classic entities
            Tweet tweet1 = new Tweet(127512260116623360, token);
            Console.WriteLine(tweet1.Text);

            // This tweet has media entity
            try
            {
                Tweet tweet2 = new Tweet(112652479837110270, token);
                Console.WriteLine(tweet2.Text);
            }
            catch (WebException)
            {
                Console.WriteLine("Tweet has not been created!");
            }
        }
        #endregion

        #region Publish Retweet

        private static void PublishAndDestroyRetweet(IToken token)
        {
            IUser tweetinviApi = new User("tweetinviApi", token);
            List<ITweet> tweets = tweetinviApi.GetUserTimeline();

            // Retweeting the last tweet of TweetinviApi
            ITweet t = tweets[0];

            // Retweet is the tweet posted on the TokenUser timeline
            ITweet retweet = t.PublishRetweet();

            // Destroying the retweet
            retweet.Destroy();
        }

        #endregion

        #region Get Retweets of Tweet

        private static void Get_retweet_of_tweet(IToken token, long id)
        {
            ITweet tweet1 = new Tweet(id, token);
            IList<ITweet> retweets = tweet1.GetRetweets();
            foreach (Tweet r in retweets)
            {
                Console.WriteLine("tweet id  = " + r.Id + " - text = " + r.Text);
            }
        }
        #endregion

        #region Favourites

        private static void CreateFavouriteTweet(IToken token)
        {
            ITweet newTweet = new Tweet(String.Format("Favouriting tweet {0}", DateTime.Now), token);
            newTweet.Publish();
            newTweet.Favourited = true;
        }

        private static void GetFavouriteTweet(IToken token)
        {
            IUser user = new User("ladygaga", token);
            List<ITweet> tweets = user.GetFavourites();

            foreach (var tweet in tweets)
            {
                Console.WriteLine(tweet);
            }
        }

        private static void GetFavouriteSince(IToken token)
        {
            string text = String.Format("Favouriting tweet {0}", DateTime.Now);

            // Create and favourite a first tweet
            ITweet tweet1 = new Tweet(text, token);
            tweet1.Publish();
            tweet1.Favourited = true;

            ITweet tweet2 = new Tweet(text + " - 2", token);
            tweet2.Publish();
            tweet2.Favourited = true;

            ITweet tweet3 = new Tweet(text + " - 3", token);
            tweet3.Publish();
            tweet3.Favourited = true;

            IUser creator = tweet1.Creator;

            List<ITweet> favouritesSinceId = creator.GetFavouritesSinceId(tweet1.Id);

            // Should return the last 2 tweets

            foreach (var tweet in favouritesSinceId)
            {
                Console.WriteLine(tweet.ToString());
            }
        }

        #endregion

        #endregion

        #region Direct Message

        #region Message creation

        // Create a message and retrieve it from Twitter
        private static void Get_message(IToken token, long messageId)
        {
            IMessage m = new Message(messageId, token);

            Console.WriteLine("message text = " + m.Text + " ; receiver = " + m.Receiver.ScreenName + " ; sender = " + m.Sender.ScreenName);
        }

        // Create a new message
        private static IMessage createNewMessage()
        {
            IUser receiver = new User(543118219);
            IMessage msg = new Message(
                String.Format("Hello from Tweetinvi! ({0})", DateTime.Now.ToShortTimeString()),
                receiver);

            return msg;
        }

        #endregion

        #region Send Message

        private static void SendMessage(IToken token)
        {
            IMessage msg = createNewMessage();
            msg.Send(token);
        }

        #endregion

        #endregion

        #region Tweetinvi API

        #endregion

        #region Search

        #region User

        private static void SearchUser(IToken token)
        {
            string searchQuery = "tweetinvi";

            IUserSearchEngine searchEngine = new UserSearchEngine(token);
            List<IUser> searchResult = searchEngine.Search(searchQuery);

            foreach (var user in searchResult)
            {
                Console.Write(user.ScreenName);
            }
        }

        #endregion

        #endregion

        // BRAND NEW - GENERATE YOUR TOKEN!
        #region Token Generator

        public static int GetCaptchaFromConsole(string validationUrl)
        {
            int result = -1;

            Thread enterCaptchaThread = new Thread(() =>
            {
                Application app = new Application();
                result = app.Run(new ValidateApplicationCaptchaWindow(validationUrl, true));
            });

            enterCaptchaThread.SetApartmentState(ApartmentState.STA);
            enterCaptchaThread.Start();
            enterCaptchaThread.Join();

            return result;
        }

        public static int GetCaptchaFromWPF(string validationUrl)
        {
            int result = -1;

            Thread enterCaptchaThread = new Thread(() =>
            {
                Application app = new Application();
                ValidateApplicationCaptchaWindow window = new ValidateApplicationCaptchaWindow(validationUrl, true);
                window.Closed += (sender, args) =>
                {
                    result = window.VerifierKey;
                };

                app.Run(window);
            });

            enterCaptchaThread.SetApartmentState(ApartmentState.STA);
            enterCaptchaThread.Start();
            enterCaptchaThread.Join();

            return result;
        }

        public static IToken GenerateToken(IToken consumerToken, RetrieveCaptchaDelegate getCaptchaDelegate)
        {
            Console.WriteLine("Starting Token Generation...");
            ITokenCreator creator = new TokenCreator(consumerToken.ConsumerKey,
                                                     consumerToken.ConsumerSecret);

            Console.WriteLine("Please enter the verifier key...");
            IToken newToken = creator.CreateToken(getCaptchaDelegate);

            if (newToken != null)
            {
                Console.WriteLine("Token generated!");
                Console.WriteLine("Token Information : ");

                Console.WriteLine("Consumer Key : {0}", newToken.ConsumerKey);
                Console.WriteLine("Consumer Secret : {0}", newToken.ConsumerSecret);
                Console.WriteLine("Access Token : {0}", newToken.AccessToken);
                Console.WriteLine("Access Token Secret : {0}", newToken.AccessTokenSecret);

                ITokenUser loggedUser = new TokenUser(newToken);
                Console.WriteLine("Your name is {0}!", loggedUser.ScreenName);

                return newToken;
            }

            Console.WriteLine("Token could not be generated. Please login and specify your verifier key!");
            return null;
        }

        public static IToken GenerateTokenFromConsole(IToken consumerToken)
        {
            return GenerateToken(consumerToken, GetCaptchaFromConsole);
        }

        public static IToken GenerateTokenFromWPF(IToken consumerToken)
        {
            return GenerateToken(consumerToken, GetCaptchaFromWPF);
        }

        #endregion

        /// <summary>
        /// Run a basic application to provide a code example
        /// </summary>
        static void Main()
        {
            // Initializing a Token with Twitter Credentials contained in the App.config
            IToken token = new Token(
                ConfigurationManager.AppSettings["token_AccessToken"],
                ConfigurationManager.AppSettings["token_AccessTokenSecret"],
                ConfigurationManager.AppSettings["token_ConsumerKey"],
                ConfigurationManager.AppSettings["token_ConsumerSecret"]);

            TokenSingleton.Token = token;
        #region Codigo propio
            Random random = new Random();
            int randomNumber = random.Next(1, 5);
            int randompost = random.Next(0, 50);
            string[] comentario = new string[26]{"multas alos que avandonan"
,"A Televisa le condonan 3,000 millones de pesos de impuestos ..."
,"las mascotas perros y gatos son parte de nuestra familia y el echo de comprarles croketas no es indicativo de familias pudientes "
,"A perooo eso siii le quitaron el IVA a a los depositos Bancarios que rebasen los $100 mil pesoss... inche bola de ratassssss."
,"Que pasa con los medios de comunicacion, solo informan pero no ayudannnnn.... y tu vas a ir a dar el gritoooo de VIVA MEXICO."
,"Si quieren recaudar mas fondos,que los politicos dejen de cobrar millones y q se mantengan con su sueldo no con los recursos del pueblo"
,"Que cobren impuestos a las empresas que son los verdaderos ricos,(Televisoras,Fam.Slim,Periodicos) estas no pagan impuestos "
,"Mejor hay que bajarles sueldos y prestaciones a diputados y senadores, como ven?"
,"Mejor q eliminen unos 300 diputados y unos 50 senadores, si de verdad les interesa el bien nacional."
,"Ahora lo unico que generaran son menos impuestos por comida para perros y mas restos de lo que nos alimentamos para los canes"
,"Con esto solo van a contribuir a que mas animalitos queden en las calles"
,"Impuestos ala compra de animales solo hara que haya mas venta clandestina y mas pobres animales sufriendo"
,"Impuestos ala cerveza ,cigarros y licor no hay problema pero ala comida de animales !!!!!"
,"Fomentara la Compra de Empresas con Perdidas en ISR para FUSIONAR las PERDIDAS y evadir pagar Impuestos"
,"No alos impuestos de las Mascotas"
,"No fomenten el abandono de animales"
,"La ayuda a perros abandonados sera mas dificil."
,"Ni chicles les vamos a poder dar alos perros y eso que les caen mal"
,"Ahora ya ni perro que me ladre "
,"Mi mascota ahora va a compartir mi comida."
,"Al perrito le duele la muela, le dolio por morder la cazuela"
,"Mi comida es mas barata que la de mi mascota"
,"Ahora si adopta, no compres y no pagues impuesto"
,"Yo no doy mas dinero al gobierno,mejor adopto y no compro"
,"Mi comida es ahora menos cara que la de mi mascota"
,"No impuestos en renta, hipotecas escuela y a la comida de mi perro"
};
           // Es un lujo un perro que ayuda a un ciego, es un lujo un perro que cuida una casa, es un lujo un perro que acompaña a una persona de la tercera edad, noooooo, es un lujo tener politicos buenos para nada ese si es un lujo
            //ExecuteQuery(token);
            //StreamReader objReader = new StreamReader("c:\\HOME\\Parte1v1.txt");
            string sLine = "";
            ArrayList arrText = new ArrayList();
            string[] strcat= new string[20];
            string[] strcat2 = new string[20];
          
            for (int i = 991; i < 4000; i++)
            {
                try
                {
                    randompost = random.Next(0, 19);
                    randomNumber = random.Next(1, 19);
                    PublishTweet(token,i.ToString() + " #NoIVAalimentoAnimales " + comentario[randompost]);
                    Console.WriteLine("-------- " + i.ToString() + " de 2000---------");
                    Console.WriteLine(comentario[randompost]);

                    System.Threading.Thread.Sleep(1 * 52 * 1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    System.Threading.Thread.Sleep(15 * 60 * 1000);
                }
            }
        #endregion
            #region Generate a Token with Access Token for the User
            // var t = GenerateTokenFromWPF(token);
            // var t2 = GenerateTokenFromConsole(token);
            //if (t != null)
            //{
            //    token = t;
            //}
            #endregion

            #region User Examples

            // User
            //createUser(token, "StevensDev");

            // Friends
            //GetFriends(token);
            //GetFriendIds(token, 579529593);
            //GetFriendIdsUsingUsername(token, "StevensDev");

            // Followers
            // UserGetFollowers(token);
            // GetFollowerIds(token, 579529593);
            // GetFollowerIdsUsingUsername(token, "StevensDev");

            // Contributors
            //GetContributors(token, 30973, null, true);
            //GetContributors(token, null, "twitterapi", true);
            //GetContributees(token, 15483731, null, true);
            //GetContributees(token, null, "LeeAdams", true);

            // TimeLines
            //GetTimeline(token, 579529593, true);
            //GetMentions(token);

            // Tweets
            //Get_retweet_of_tweet(token, 173198765052792833);
            //GetFavouriteSince(token);

            // List
            //GetSuggestedUserListDetails(token, "us-election-2012");
            //GetSuggestedUserListMembers(token, "us-election-2012");

            // Images
            //GetProfileImage(token);

            #endregion

            #region TokenUser Examples

            //GetHomeTimeline(token);
            //GetDirectMessagesReceived(token);
            //GetDirectMessagesSent(token);
            //GetBlockedUsers(token, true, true);
            //GetBlockedUsersIds(token, true);
            //GetSuggestedUserList(token, true);

            #endregion

            #region Tweet Examples
           
            //GetTweetById(token);
            //PublishTweet(token, "#ENLLAMAS Ya quiero ver Los Juegos del Hambre 2!");
            
            //PublishTweetWithGeo(token);
            //PublishInReplyTo(token);
            //PublishAndDestroyRetweet(token);
            //CreateFavouriteTweet(token);
            //GetFavouriteTweet(token);

            #endregion

            #region Message Examples

            //Get_message(token, 347015339323842560);
            //SendMessage(token);
            //GetDirectMessagesSent(token);
            //GetDirectMessagesReceived(token);

            #endregion

            #region Streaming Examples

            // StreamingExample(token);
            // StreamFilterExample(token);

            #endregion

            #region SearchUser Examples

            // SearchUser(token);

            #endregion

            #region Powered Users

            // ExecuteQuery(token);
            // ExecuteCursorQuery(token);

            #endregion

            Console.WriteLine("End");
            Console.ReadKey();
        }
        }
   
}