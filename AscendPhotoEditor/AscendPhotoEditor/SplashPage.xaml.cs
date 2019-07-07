﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using Dropbox.Api;
using Dropbox.Api.Common;
using Dropbox.Api.Files;
using Dropbox.Api.Team;
using Plugin.FilePicker.Abstractions;

namespace AscendPhotoEditor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {

        private DropboxControl dropboxControl;

        public SplashPage()
        {
            InitializeComponent();
        }

        async void LocalFileClicked(object sender, EventArgs e)
        {
            try
            {
                // Open file selector and store data of selected file
                FileData fileData = await CrossFilePicker.Current.PickFile();
                // User canceled file picking
                if (fileData == null)
                    return;

                // Check file name, path, and data - for debugging purposes
                Console.Write("Attempting to print file info . . . ");
                string fileName = fileData.FileName;
                string filePath = fileData.FilePath;
                string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                Console.WriteLine("File name chosen: " + fileName);
                Console.WriteLine("File path: " + filePath);
                //System.Console.WriteLine("File data: " + contents);

                // Get data needed for editor
                Console.WriteLine("Creating Stream . . .");
                Stream imageStream = fileData.GetStream();
                Console.WriteLine("Creating byte array . . . ");
                byte[] imageData = fileData.DataArray;
                Console.WriteLine("SWITCHING PAGES....");
                await Navigation.PushAsync(new EditorPage(imageData: imageData, imageStream: imageStream));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }


        async void DropBoxClicked(object sender, EventArgs e)
        {
            dropboxControl = new DropboxControl();
            await this.dropboxControl.Authorize();

        }

        public class DropboxControl
        {
            private const string AppKeyDropboxtoken = "MyDropboxToken";
            private const string ClientId = "MyDropboxClientId";
            private const string RedirectUri = "https://www.anysite.se/";
            public Action OnAuthenticated;
            private string oauth2State;
            private string AccessToken { get; set; }

            public DropboxControl()
            {

            }

            /// <summary>
            ///     <para>Runs the Dropbox OAuth authorization process if not yet authenticated.</para>
            ///     <para>Upon completion <seealso cref="OnAuthenticated"/> is called</para>
            /// </summary>
            /// <returns>An asynchronous task.</returns>
            public async Task Authorize()
            {
                if (string.IsNullOrWhiteSpace(this.AccessToken) == false)
                {
                    // Already authorized
                    this.OnAuthenticated?.Invoke();
                    return;
                }

                if (this.GetAccessTokenFromSettings())
                {
                    // Found token and set AccessToken 
                    return;
                }

                // Run Dropbox authentication
                this.oauth2State = Guid.NewGuid().ToString("N");
                var authorizeUri = DropboxOAuth2Helper.GetAuthorizeUri(OAuthResponseType.Token, "", new Uri("http://localhost/"), this.oauth2State);
                Console.WriteLine("URI ======> {0}", authorizeUri);
                var webView = new WebView { Source = new UrlWebViewSource { Url = authorizeUri.AbsoluteUri } };
                webView.Navigating += this.WebViewOnNavigating;
                var contentPage = new ContentPage { Content = webView };
                await Application.Current.MainPage.Navigation.PushModalAsync(contentPage);
            }

            private bool GetAccessTokenFromSettings()
            {
                try
                {
                    if (!Application.Current.Properties.ContainsKey(AppKeyDropboxtoken))
                    {
                        return false;
                    }

                    this.AccessToken = Application.Current.Properties[AppKeyDropboxtoken]?.ToString();
                    if (this.AccessToken != null)
                    {
                        this.OnAuthenticated.Invoke();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            private async void WebViewOnNavigating(object sender, WebNavigatingEventArgs e)
            {
                if (!e.Url.StartsWith(RedirectUri, StringComparison.OrdinalIgnoreCase))
                {
                    // we need to ignore all navigation that isn't to the redirect uri.
                    return;
                }

                try
                {
                    var result = DropboxOAuth2Helper.ParseTokenFragment(new Uri(e.Url));

                    if (result.State != this.oauth2State)
                    {
                        return;
                    }

                    this.AccessToken = result.AccessToken;

                    await SaveDropboxToken(this.AccessToken);
                    this.OnAuthenticated?.Invoke();
                }
                catch (ArgumentException)
                {
                    // There was an error in the URI passed to ParseTokenFragment
                }
                finally
                {
                    e.Cancel = true;
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
            }

            private static async Task SaveDropboxToken(string token)
            {
                if (token == null)
                {
                    return;
                }

                try
                {
                    Application.Current.Properties.Add(AppKeyDropboxtoken, token);
                    await Application.Current.SavePropertiesAsync();
                }
                catch (Exception ex)
                {
                    
                }
            }

        }

        async void GoogleDriveClicked(object sender, EventArgs e)
        {
            //TODO
           // await Navigation.PushAsync(new EditorPage(imagePath: "image_placeholder.png"));
        }
    }
}