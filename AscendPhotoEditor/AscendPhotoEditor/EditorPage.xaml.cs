using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using ExifLib;

namespace AscendPhotoEditor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : ContentPage
    {
        public EditorPage(byte[] imageData, Stream imageStream)
        {
            InitializeComponent();

            // Reconstruct image from byte array
            //selectedImage.Source = imagePath;
            selectedImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));

            // Extract metadata from image
            using (imageStream)
            {
                Console.WriteLine("Opened image stream . . . ");
                // Get jpeg data
                var jpegMetadata = ExifReader.ReadJpeg(imageStream);

                PropertyInfo[] properties = typeof(JpegInfo).GetProperties();
                int n = 0;
                foreach (PropertyInfo property in properties)
                {
                    Console.WriteLine("{0} || {1} || {2}", n, property, property.GetValue(jpegMetadata, null));
                    n += 1;
                }
            }
            
            
            // Get Image metadata
            //List<MetadataEntry> metadataList = new List<MetadataEntry>();

            //Dummy data for testing
            List<MetadataEntry> metadataList = new List<MetadataEntry>();
            metadataList.Add(new MetadataEntry("one", "111"));
            metadataList.Add(new MetadataEntry("two", "222"));
            metadataList.Add(new MetadataEntry("three", "333"));

            // Set metadata list
            metadataListView.ItemsSource = metadataList;
            metadataListView.ItemTemplate = new DataTemplate(typeof(EntryCell));
            metadataListView.ItemTemplate.SetBinding(EntryCell.LabelProperty, "ItemType");
            metadataListView.ItemTemplate.SetBinding(EntryCell.TextProperty, "ItemDetails");
            

        }
    }

    public class MetadataEntry
    {
        public string ItemType { get; set; }
        public string ItemDetails { get; set; }

        public MetadataEntry(string itemType, string itemDetails)
        {
            this.ItemType = itemType;
            this.ItemDetails = itemDetails;
        }
    }
}