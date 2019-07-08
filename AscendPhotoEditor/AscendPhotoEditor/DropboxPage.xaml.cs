using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AscendPhotoEditor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DropboxPage : ContentPage
    {
        public DropboxPage(string authToken)
        {
            var client = new Dropbox.Api.DropboxClient(oauth2AccessToken: authToken);
            InitializeComponent();
        }

    }
}