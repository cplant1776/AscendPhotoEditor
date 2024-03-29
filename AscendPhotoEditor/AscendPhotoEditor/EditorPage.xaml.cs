﻿using System;
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
            selectedImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));


            //Create list to store metadata items
            List<MetadataEntry> metadataList = new List<MetadataEntry>();

            // Extract metadata from image
            using (imageStream)
            {
                Console.WriteLine("Opened image stream . . . ");
                string itemDetails = "";
                // Get jpeg data
                var jpegMetadata = ExifReader.ReadJpeg(imageStream);

                // Read metadata properties into list
                PropertyInfo[] properties = typeof(JpegInfo).GetProperties();
                int n = 0;
                foreach (PropertyInfo property in properties)
                {
                    Console.WriteLine("{0} || {1} || {2}", n, property, property.GetValue(jpegMetadata));
                    try
                    {
                        itemDetails = String.Format("{0}", property.GetValue(jpegMetadata));
                        Console.WriteLine("itemDetails => |{0} || {1} |", itemDetails, property.GetValue(jpegMetadata));
                    }
                    catch (InvalidCastException) {
                        Console.WriteLine("INVALID CAST => {0}", property.GetValue(jpegMetadata));
                    }
                    n += 1;

                    metadataList.Add(new MetadataEntry(itemType: property.Name, itemDetails: itemDetails));
                }
            }

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