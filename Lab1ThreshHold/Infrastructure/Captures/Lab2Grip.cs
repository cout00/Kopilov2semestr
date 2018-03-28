using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using Lab1ThreshHold.Infrastructure.FrameCapture;
using System.Windows;

namespace Lab1ThreshHold.Infrastructure.Captures
{
    public class Lab2Grip :Grip
    {
        public event EventHandler<CaptureArgs<List<Point>>> OnHistogramPoints;
        public event EventHandler<CaptureArgs<List<Point>>> OnHistogramResultPoints;
        public event EventHandler<CaptureArgs<Image<Bgr,float>>> OnResultImage;

        List<Point> resPoints;

        void InitPointList(double[] data)
        {
            resPoints = new List<System.Windows.Point>();
            for (int i = 0; i < data.Length; i++)
            {
                resPoints.Add(new System.Windows.Point(i, data[i]));
            }
        }

        void DataMatrixRenuming(ref byte[,,] renumData,Func<byte,byte> callBack)
        {
            for (int i = 0; i < renumData.GetLength(0); i++)
            {
                for (int j = 0; j < renumData.GetLength(1); j++)
                {
                    renumData[i, j, 0]=callBack(renumData[i,j,0]);
                }
            }}


        protected override void Process(Image<Gray, byte> queryImage)
        {
            //создаем гистограмму
            double[] points = new double[byte.MaxValue + 1];

            
            var data = queryImage.Data;
            DataMatrixRenuming(ref data,b =>
            {
                points[b]++;
                return b;
            });
            //стандартизируем ее
            points = points.Select(d => d / (data.GetLength(0) * data.GetLength(1))).ToArray();
            //преобразуем к виду для отображения
            InitPointList(points);
            OnHistogramPoints?.Invoke(this, new CaptureArgs<List<Point>>(resPoints));
            //производим нарастающую сумму
            for (int i = 1; i < points.Length; i++)
            {
                points[i] += (points[i - 1]);
            }
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = points[i]*255;
            }
            //преобразуем к виду для отображения
            InitPointList(points);
            OnHistogramResultPoints?.Invoke(this, new CaptureArgs<List<Point>>(resPoints));
            var resultData = data;
            //применяем функцию на стандартное изображение
            DataMatrixRenuming(ref resultData, b =>
            {
                return (byte)(points[b]);
            });
            OnResultImage?.Invoke(this, new CaptureArgs<Image<Bgr, float>>(new Image<Gray, byte>(resultData).Convert<Bgr, float>()));
        }
    }
}
