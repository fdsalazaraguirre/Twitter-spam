using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Streaminvi.Properties;
using TweetinCore.Interfaces.StreamInvi;
using Tweetinvi.Utils;

namespace Streaminvi
{
    /// <summary>
    /// Extract objects from any kind of stream
    /// </summary>
    public class StreamResultGenerator : IStreamResultGenerator
    {
        private bool _isRunning;

        private StreamState _streamState;
        public StreamState StreamState
        {
            get { return _streamState; }
            set
            {
                if (_streamState != value)
                {
                    _streamState = value;
                    _isRunning = _streamState == StreamState.Resume || _streamState == StreamState.Pause;
                }
            }
        }

        private StreamReader InitWebRequest(WebRequest webRequest)
        {
            StreamReader reader = null;
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            try
            {
                Stream responseStream = webResponse.GetResponseStream();

                if (responseStream != null)
                {
                    reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding("utf-8"));
                }
            }
            catch (ArgumentException ex)
            {
                if (ex.Message == "Stream was not readable.")
                {
                    webRequest.Abort();
                }
                else
                {
                    throw;
                }
            }

            return reader;
        }

        private StreamReader _currentReader;

        public void StartStream(
            Func<string, bool> processObject,
            Func<HttpWebRequest> generateWebRequest)
        {
            if (_isRunning)
            {
                throw new OperationCanceledException(Resources.Stream_IllegalMultipleStreams);
            }

            if (processObject == null)
            {
                throw new NullReferenceException(Resources.Stream_ObjectDelegateIsNull);
            }

            #region Variables

            StreamState = StreamState.Resume;
            HttpWebRequest webRequest = generateWebRequest();
            webRequest.Timeout = -1;
            _currentReader = InitWebRequest(webRequest);

            int errorOccured = 0;
            #endregion

            while (_streamState != StreamState.Stop)
            {
                try
                {
                    string jsonResponse = _currentReader.ReadLine();

                    #region Error Checking

                    if (jsonResponse == null)
                    {
                        if (errorOccured == 0)
                        {
                            ++errorOccured;
                        }
                        else if (errorOccured == 1)
                        {
                            ++errorOccured;
                            webRequest.Abort();
                            _currentReader = InitWebRequest(webRequest);
                        }
                        else if (errorOccured == 2)
                        {
                            ++errorOccured;
                            webRequest.Abort();
                            webRequest = generateWebRequest();
                            _currentReader = InitWebRequest(webRequest);
                        }
                        else
                        {
                            Console.WriteLine("Twitter API is not accessible");
                            Trace.WriteLine("Twitter API is not accessible");
                            break;
                        }
                    }
                    else
                    {
                        errorOccured = 0;
                    }

                    #endregion

                    if (!processObject(jsonResponse))
                    {
                        StreamState = StreamState.Stop;
                        break;
                    }
                }
                catch (IOException ex)
                {
                    // Verify the implementation of the Exception handler
                    #region IOException Handler
                    if (ex.Message == "Unable to read data from the transport connection: The connection was closed.")
                        _currentReader = InitWebRequest(webRequest);

                    try
                    {
                        _currentReader.ReadLine();
                    }
                    catch (IOException ex2)
                    {
                        if (ex2.Message ==
                            "Unable to read data from the transport connection: The connection was closed.")
                        {
                            Trace.WriteLine("Streamreader was unable to read from the stream!");

                            processObject(null);
                            break;
                        }
                    }
                    catch (ObjectDisposedException)
                    {
                        // StopStream has been called
                        break;
                    }

                    #endregion
                }
            }

            #region Clean
            webRequest.Abort();
            _currentReader.Dispose();
            _streamState = StreamState.Stop;
            #endregion
        }

        public void ResumeStream()
        {
            StreamState = StreamState.Resume;
        }

        public void PauseStream()
        {
            StreamState = StreamState.Pause;
        }

        public void StopStream()
        {
            StreamState = StreamState.Stop;
            if (_currentReader != null)
            {
                _currentReader.Close();
            }
        }
    }
}
