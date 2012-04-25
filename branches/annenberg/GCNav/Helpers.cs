﻿using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows;
using System.Xml;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace GCNav
{
    public class Helpers
    {
        public delegate void MapEventHandler(Object Sender, MapEventArgs e);
        public delegate void ImageLoadedHandler(Object sender, ImageLoadedEventArgs e);
        public delegate void ImageSelectedHandler(Object sender, ImageSelectedEventArgs e);
        
        public static DoubleAnimation makeDoubleAnimation(double from, double to, double seconds)
        {
            DoubleAnimation myAnimation = new DoubleAnimation();
            myAnimation.From = from;
            myAnimation.To = to;
            myAnimation.Duration = new Duration(TimeSpan.FromSeconds(seconds));
            return myAnimation;
        }

        public class MapEventArgs : EventArgs
        {
            private List<ImageData> images;
            public MapEventArgs(List<ImageData> i)
            {
                images = i;
            }
            public List<ImageData> getImages()
            {
                return images;
            }
        }

        public class ImageLoadedEventArgs : EventArgs
        {
            private ImageData data;
            public ImageLoadedEventArgs(ImageData i)
            {
                data = i;
            }
            public ImageData getImage()
            {
                return data;
            }
        }

        public class ImageSelectedEventArgs : EventArgs
        {
            private ImageData data;
            public ImageSelectedEventArgs(ImageData i)
            {
                data = i;
            }
            public ImageData getImage()
            {
                return data;
            }
        }

        public static XmlNodeList LoadNamesFromXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("data/AnnenbergCollection.xml");
            return doc.SelectNodes("/Collection/Image");
        }

        public static void ChangeImageSource(Image image, string uri)
        {
            image.Stretch = Stretch.UniformToFill;
            try
            {
                image.Source = new BitmapImage(new Uri(
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + uri,
                    UriKind.RelativeOrAbsolute));
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                ChangeToDefaultImage(image);
            }
        }

        public static void ChangeToDefaultImage(Image image)
        {
            ChangeImageSource(image, "/data/Images/Thumbnail/00000a_512.jpg");
        }

        public static string RandomString(int size, Random random)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static TextBlock createNameTextBlock(string name)
        {
            TextBlock t = new TextBlock();
            t.FontSize = 20;
            t.FontWeight = FontWeights.Bold;
            t.FontFamily = new FontFamily("Times New Roman");
            t.Text = name;
            return t;
        }

        public static TextBlock createNameTextBlockDisplay(string name, Color color)
        {
            TextBlock t = Helpers.createNameTextBlock(name);
            t.Foreground = new SolidColorBrush(color);
            return t;
        }

        public static Size MeasureTextBlock(string name)
        {
            TextBlock t = Helpers.createNameTextBlock(name);
            t.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Size a = t.DesiredSize;
            return a;
        }
    }
}
