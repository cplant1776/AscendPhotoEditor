using System;
using System.Collections.Generic;
using System.IO;
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
            //TODO
           // await Navigation.PushAsync(new EditorPage(imagePath: "image_placeholder.png"));

        }

        async void GoogleDriveClicked(object sender, EventArgs e)
        {
            //TODO
           // await Navigation.PushAsync(new EditorPage(imagePath: "image_placeholder.png"));
        }
    }
}