using System;
using System.Threading;
using System.Web;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Authentication.OAuth2;
using Google.Apis.Moderator.v1;
using Google.Apis.Moderator.v1.Data;
using Google.Apis.Samples.Helper;
using Google.Apis.Util;

namespace JaxDug.Models
{
    public class GoogleServiceModel
    {
        private static ModeratorService _service; // We don't need individual service instances for each client.
        private static OAuth2Authenticator<WebServerClient> _authenticator;
        private IAuthorizationState _state;
        public static long SeriesId;

        public GoogleServiceModel(string seriesId = null)
        {
            if (_service == null)
            {
                _service = new ModeratorService(_authenticator = CreateAuthenticator());
                HttpContext.Current.Session["ModeratorService"] = _service;
            }

            // Check if we received OAuth2 credentials with this request; if yes: parse it.
            if (HttpContext.Current.Request["code"] != null)
            {
                _authenticator.LoadAccessToken();
            }

            if (seriesId != null)
            {
                SeriesId = Id2Base(seriesId);
            }
        }

        /// <summary>
        /// Returns the authorization state which was either cached or set for this session.
        /// </summary>
        private IAuthorizationState AuthState
        {
            get
            {
                return _state ?? HttpContext.Current.Session["AUTH_STATE"] as IAuthorizationState;
            }
        }

        public OAuth2Authenticator<WebServerClient> CreateAuthenticator()
        {
            // Register the authenticator.
            var provider = new WebServerClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = ClientCredentials.ClientID;
            provider.ClientSecret = ClientCredentials.ClientSecret;
            var authenticator =
                new OAuth2Authenticator<WebServerClient>(provider, GetAuthorization) { NoCaching = true };
            return authenticator;
        }

        public IAuthorizationState GetAuthorization(WebServerClient client)
        {
            // If this user is already authenticated, then just return the auth state.
            IAuthorizationState state = AuthState;
            if (state != null)
            {
                return state;
            }

            // Check if an authorization request already is in progress.
            state = client.ProcessUserAuthorization(new HttpRequestInfo(HttpContext.Current.Request));
            if (state != null && (!string.IsNullOrEmpty(state.AccessToken) || !string.IsNullOrEmpty(state.RefreshToken)))
            {
                // Store and return the credentials.
                HttpContext.Current.Session["AUTH_STATE"] = _state = state;
                return state;
            }

            // Otherwise do a new authorization request.
            string scope = ModeratorService.Scopes.Moderator.GetStringValue();
            OutgoingWebResponse response = client.PrepareRequestUserAuthorization(new[] { scope });
            response.Send(); // Will throw a ThreadAbortException to prevent sending another response.
            return null;
        }

        /// <summary>
        /// Fetches the TasksLists of the user.
        /// </summary>
        public SeriesList FetchSerieslists()
        {
            try
            {
                // Fetch all TasksLists of the user asynchronously.
                return _service.Series.List().Fetch();
            }
            catch (ThreadAbortException)
            {
                // User was not yet authenticated and is being forwarded to the authorization page.
                throw;
            }
            catch (Exception ex)
            {
                var error = ex.ToHtmlString();
            }
            return null;
        }

        public TopicList FetchTopiclists()
        {
            try
            {
                // Fetch all TasksLists of the user asynchronously.
                return _service.Topics.List(SeriesId).Fetch();
            }
            catch (ThreadAbortException)
            {
                // User was not yet authenticated and is being forwarded to the authorization page.
                throw;
            }
            catch (Exception ex)
            {
                var error = ex.ToHtmlString();
            }
            return null;
        }

        public Submission FetchSubmission(string submissionId)
        {
            try
            {
                // Fetch all TasksLists of the user asynchronously.
                return _service.Submissions.Get(SeriesId, Id2Base(submissionId)).Fetch();
            }
            catch (ThreadAbortException)
            {
                // User was not yet authenticated and is being forwarded to the authorization page.
                throw;
            }
            catch (Exception ex)
            {
                var error = ex.ToHtmlString();
            }
            return null;
        }

        public SubmissionList FetchSubmissionList(string topicId)
        {
            try
            {
                // Fetch all submissions of requested topic
                return _service.Topics.Submissions.List(SeriesId, long.Parse(topicId)).Fetch();
            }
            catch (ThreadAbortException)
            {
                // User was not yet authenticated and is being forwarded to the authorization page.
                throw;
            }
            catch (Exception ex)
            {
                var error = ex.ToHtmlString();
            }
            return null;
        }

        static Int64 Id2Base(string stringId)
        {
            return Int64.Parse(stringId, System.Globalization.NumberStyles.HexNumber);
        }

    }

    /// <summary>
    /// This class provides the client credentials for all the samples in this solution.
    /// In order to run all of the samples, you have to enable API access for every API 
    /// you want to use, enter your credentials here.
    /// 
    /// You can find your credentials here:
    ///  https://code.google.com/apis/console/#:access
    /// 
    /// For your own application you should find a more secure way than just storing your client secret inside a string,
    /// as it can be lookup up easily using a reflection tool.
    /// </summary>
    internal static class ClientCredentials
    {
        /// <summary>
        /// The OAuth2.0 Client ID of your project.
        /// </summary>
        public static readonly string ClientID = "477913250789.apps.googleusercontent.com";

        /// <summary>
        /// The OAuth2.0 Client secret of your project.
        /// </summary>
        public static readonly string ClientSecret = "a1rWEE6ftSETKvr5dlj1tgSH";

        /// <summary>
        /// Your Api/Developer key.
        /// </summary>
        public static readonly string ApiKey = "AIzaSyBpVLJlQWKaFMlmsNx3s42yUz4Wdx7TEXw";

        #region Verify Credentials
        static ClientCredentials()
        {
            ReflectionUtils.VerifyCredentials(typeof(ClientCredentials));
        }
        #endregion
    }
}