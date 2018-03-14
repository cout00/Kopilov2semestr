using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Lab1ThreshHold
{
    public static class Extensions
    {
        public static BitmapSource Convert(this Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);
            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }



        public static T FindChild<T>(this DependencyObject parent, Func<T, bool> predicate)
            where T : DependencyObject
        {
            if (parent == null) return null;
            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, predicate);
                    if (foundChild != null) break;
                }
                var frameworkElement = child as FrameworkElement;
                if (frameworkElement != null && predicate(frameworkElement as T))
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static Image<Gray, byte> ApplyFunc(this Image<Gray, byte> src, Func<byte, byte> func)
        {
            var data = src.Data;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j, 0] = func(data[i, j, 0]);
                }
            }
            return new Image<Gray, byte>(data);
        }

        public static Image<Gray, byte> ApplyFunc(this Image<Gray, byte> src, List<System.Windows.Point> func)
        {
            double[] points = func.Select(a => (double)a.Y).ToArray();
            var data = src.Data;
            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    data[i, j, 0] *= (byte)points[data[i, j, 0]];
                }
            }
            return new Image<Gray, byte>(data);
        }
        
        //public static List<System.Windows.Point> GetFunctionPoints(this Image<Gray, byte> src)
        //{
        //    //double[] points = src.GetHistogramPoints().Select(a => (double)a.Y).ToArray();
        //    //for (int i = 1; i < points.Length; i++)
        //    //{
        //    //    points[i] += points[i - 1];
        //    //}
        //    //List<System.Windows.Point> point = new List<System.Windows.Point>();
        //    //for (int i = 0; i < points.Length; i++)
        //    //{
        //    //    point.Add(new System.Windows.Point(i, points[i]));
        //    //}
        //    //return point;
        //}
    }
}
