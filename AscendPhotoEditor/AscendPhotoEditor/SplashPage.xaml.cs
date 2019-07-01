using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

namespace AscendPhotoEditor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
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
                string fileName = fileData.FileName;
                string filePath = fileData.FilePath;
                string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);
                System.Console.WriteLine("File name chosen: " + fileName);
                System.Console.WriteLine("File path: " + filePath);
                System.Console.WriteLine("File data: " + contents);

                // Move to Editor Page
                await Navigation.PushAsync(new EditorPage(imagePath: filePath));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception choosing file: " + ex.ToString());
            }
        }

        async void DropBoxClicked(object sender, EventArgs e)
        {
            //TODO - currently just goes to Editor Page
            await Navigation.PushAsync(new EditorPage(imagePath: "image_placeholder.png"));

        }

        async void GoogleDriveClicked(object sender, EventArgs e)
        {
            //TODO - currently just goes to Editor Page
            await Navigation.PushAsync(new EditorPage(imagePath: "image_placeholder.png"));
        }
    }
}