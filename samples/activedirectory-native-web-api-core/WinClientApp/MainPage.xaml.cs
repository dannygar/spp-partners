using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WinClientApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //Set the API Endpoint to Graph 'me' endpoint
        string _sppAPIEndpoint = "https://localhost:44379/api/v1/auth/user";
        //string _sppAPIEndpoint = "https://sppapi-dev.azurewebsites.net/api/v1/auth/user";
        //string _sppAPIEndpoint = "https://localhost:44310/api/auth";

        string _sppTodoListEndpoint = "https://localhost:44321";
        private string _adminPortalEndpoint = "https://localhost:44399";
        string _graphAPIEndpoint = "https://graph.microsoft.com/v1.0/me";

        //Set the scope for API call to user.read
        string[] _scopes = new string[] { "user.read" };
        string[] _apiscopes = new string[] { "api://436b73b7-8f59-45e7-9449-580fb9a5d90d/access_as_user" };
        string[] _todoscopes = new string[] { "user.read" };

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void CallGraphButton_Click(object sender, RoutedEventArgs e)
        {
            var authResult = await ProcessADv2APICall(_scopes);
            if (authResult != null)
            {
                ResultText.Text = await GetHttpContentWithToken(_graphAPIEndpoint, authResult.AccessToken);
                DisplayBasicTokenInfo(authResult);
                this.SignOutButton.Visibility = Visibility.Visible;
            }

        }



        private async void CallSPPApiButton_Click(object sender, RoutedEventArgs e)
        {
            var authResult = await ProcessADv2APICall(_scopes);
            if (authResult != null)
            {
                ResultText.Text = await GetUser(_sppAPIEndpoint, authResult);
                DisplayBasicTokenInfo(authResult);
                this.SignOutButton.Visibility = Visibility.Visible;
            }
        }



        private async void CallTodoListButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult authResult = null;
            try
            {
                authResult = await App.PublicClientApp.AcquireTokenSilentAsync(_todoscopes, App.PublicClientApp.Users.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await App.PublicClientApp.AcquireTokenAsync(_todoscopes);
                }
                catch (MsalException msalex)
                {
                    ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                    return;
                }
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode == "failed_to_acquire_token_silently")
                {
                    ResultText.Text = "Please sign in first";
                }
                else
                {
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }
                    ResultText.Text = message;
                }

                return;
            }

            var content = await GetTodoList(_sppTodoListEndpoint + "/api/todolist", authResult.IdToken);

            ResultText.Text = "";

            if (!string.IsNullOrEmpty(content))
            {
                ResultText.Text = content;
                DisplayBasicTokenInfo(authResult);
                this.SignOutButton.Visibility = Visibility.Visible;
            }

            
            //Add a new item
            HttpContent httpContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Title", $"Todo Item {new Random().Next(1, 100)}")
            });
            if (await AddTodoItem(authResult.IdToken, httpContent))
            {
                //Read item again
                ResultText.Text += await GetTodoList(_sppTodoListEndpoint + "/api/todolist", authResult.IdToken);
                DisplayBasicTokenInfo(authResult);
                this.SignOutButton.Visibility = Visibility.Visible;
            }


        }



        private async Task<string> GetTodoList(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    return "An error occurred : " + response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                return (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
            }
        }



        private async Task<bool> AddTodoItem(string token, HttpContent data)
        {
            var httpClient = new System.Net.Http.HttpClient();
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await httpClient.PostAsync(_sppTodoListEndpoint + "/api/todolist", data);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                ResultText.Text = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
                return false;
            }
        }



        private async void CallAdminPortalButton_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationResult authResult = null;
            try
            {
                authResult = await App.PublicClientApp.AcquireTokenSilentAsync(_scopes, App.PublicClientApp.Users.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await App.PublicClientApp.AcquireTokenAsync(_scopes);
                }
                catch (MsalException msalex)
                {
                    ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                    return;
                }
            }
            catch (MsalException ex)
            {
                if (ex.ErrorCode == "failed_to_acquire_token_silently")
                {
                    ResultText.Text = "Please sign in first";
                }
                else
                {
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }
                    ResultText.Text = message;
                }

                return;
            }

            var authHeader = authResult?.CreateAuthorizationHeader();
            var content = await GetWeather(_adminPortalEndpoint + "/api/SampleData/WeatherForecasts", authResult?.IdToken, authHeader);
            if (content == null)
                return;

            ResultText.Text = "";
            var s = new StringBuilder();
            s.AppendLine("Date            Temp.(C)        Temp.(F)        Summary");

            foreach (var forecast in content)
            {
                s.AppendLine(
                    $"{forecast.DateFormatted}      {forecast.TemperatureC:D3}              {forecast.TemperatureF:D3}              {forecast.Summary}");
            }

            ResultText.Text = s.ToString();

            if(authResult != null) DisplayBasicTokenInfo(authResult);
            this.SignOutButton.Visibility = Visibility.Visible;


        }



        private async Task<IEnumerable<WeatherForecast>> GetWeather(string url, string token, string authHeader)
        {
            var httpClient = new System.Net.Http.HttpClient();
            if(!string.IsNullOrEmpty(token))
                //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authHeader);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                //var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(content);
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
            }

            return default(IEnumerable<WeatherForecast>);
        }


        private async Task<string> GetUser(string url, AuthenticationResult authResult)
        {
            var httpClient = new System.Net.Http.HttpClient();
            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResult.IdToken);
                //var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                //request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    return "An error occurred : " + response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                return (ex.InnerException != null) ? ex.InnerException.Message : ex.Message;
            }
        }


        private async Task<AuthenticationResult> ProcessADv2APICall(string[] scopes)
        {
            AuthenticationResult authResult = null;
            ResultText.Text = string.Empty;
            TokenInfoText.Text = string.Empty;

            try
            {
                authResult = await App.PublicClientApp.AcquireTokenSilentAsync(scopes, 
                    App.PublicClientApp.Users.FirstOrDefault());
                return authResult;
            }
            catch (MsalUiRequiredException ex)
            {
                // A MsalUiRequiredException happened on AcquireTokenSilentAsync. This indicates you need to call AcquireTokenAsync to acquire a token
                System.Diagnostics.Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");

                try
                {
                    authResult = await App.PublicClientApp.AcquireTokenAsync(scopes);
                    return authResult;
                }
                catch (MsalException msalex)
                {
                    ResultText.Text = $"Error Acquiring Token:{System.Environment.NewLine}{msalex}";
                }
            }
            catch (Exception ex)
            {
                ResultText.Text = $"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}";
                return null;
            }

            return null;
        }

        
        /// <summary>
        /// Sign out the current user
        /// </summary>
        private void SignOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.PublicClientApp.Users.Any())
            {
                try
                {
                    App.PublicClientApp.Remove(App.PublicClientApp.Users.FirstOrDefault());
                    this.ResultText.Text = "User has signed-out";
                    this.CallGraphButton.Visibility = Visibility.Visible;
                    this.SignOutButton.Visibility = Visibility.Collapsed;
                }
                catch (MsalException ex)
                {
                    ResultText.Text = $"Error signing-out user: {ex.Message}";
                }
            }
        }


        /// <summary>
        /// Perform an HTTP GET request to a URL using an HTTP Authorization header
        /// </summary>
        /// <param name="url">The URL</param>
        /// <param name="token">The token</param>
        /// <returns>String containing the results of the GET operation</returns>
        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        /// <summary>
        /// Display basic information contained in the token
        /// </summary>
        private void DisplayBasicTokenInfo(AuthenticationResult authResult)
        {
            TokenInfoText.Text = "";
            if (authResult != null)
            {
                TokenInfoText.Text += $"Name: {authResult.User.Name}" + Environment.NewLine;
                TokenInfoText.Text += $"Username: {authResult.User.DisplayableId}" + Environment.NewLine;
                TokenInfoText.Text += $"Token Expires: {authResult.ExpiresOn.ToLocalTime()}" + Environment.NewLine;
                TokenInfoText.Text += $"Access Token: {authResult.AccessToken}" + Environment.NewLine;
                TokenInfoText.Text += $"Id Token: {authResult.IdToken}" + Environment.NewLine;
            }
        }


        private void ClearCookiesButton_Click(object sender, RoutedEventArgs e)
        {
            const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_END_BROWSER_SESSION, IntPtr.Zero, 0);
        }


        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

    }



    public class WeatherForecast
    {
        public string DateFormatted { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
        public int TemperatureF { get; set; }
    }
}
