using System;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV.Structure;

namespace WebCamTracking
{
    public unsafe class DirectBitmap : IDisposable
    {
        private const PixelFormat PixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
        private const int BitsPerPixel = 24;
        private const int BytesPerPixel = (BitsPerPixel + 7) / 8;
        private readonly BitmapData _bitmapData;
        private byte* _bitmapDataScan0;
        public Bitmap Bitmap;

        public DirectBitmap(Bitmap bitmap)
        {
            Bitmap = bitmap;
            _bitmapData = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height),
                ImageLockMode.ReadWrite, PixelFormat);
            _bitmapDataScan0 = (byte*) _bitmapData.Scan0;
        }

        public void Dispose()
        {
        }

        public void Unlock()
        {
            Bitmap.UnlockBits(_bitmapData);
        }

        public void SetPixel(int x, int y, Rgb color)
        {
            var shift = GetShift(x, y);
            _bitmapDataScan0 += shift;

            if (_bitmapDataScan0 != null) _bitmapDataScan0[0] = (byte) color.Blue; // Blue
            if (_bitmapDataScan0 != null) _bitmapDataScan0[1] = (byte) color.Green; // Green
            if (_bitmapDataScan0 != null) _bitmapDataScan0[2] = (byte) color.Red; // Red

            _bitmapDataScan0 -= shift;
        }

        private int GetShift(int x, int y)
        {
            return (y * Bitmap.Width + x) * BytesPerPixel;
        }

        public Rgb GetPixel(int x, int y)
        {
            var shift = GetShift(x, y);
            _bitmapDataScan0 += shift;
            var color = new Rgb(_bitmapDataScan0[0], _bitmapDataScan0[1], _bitmapDataScan0[2]);
            _bitmapDataScan0 -= shift;
            return color;
        }

        public Point? ClosestAvg(Rgb color)
        {
            return null;
        }

        private double ColorDistance(Rgb c1, Rgb c2)
        {
            return Math.Pow(c1.Red - c2.Red, 2) + Math.Pow(c1.Green - c2.Green, 2) + Math.Pow(c1.Blue - c2.Blue, 2);
        }
        public void SetAllPixels()
        {
            for (var i = 0; i < Bitmap.Height; i++)
            for (var j = 0; j < Bitmap.Width; j++)
            {
                if (_bitmapDataScan0 != null) _bitmapDataScan0[0] = 0; // Blue
                //if (_bitmapDataScan0 != null) _bitmapDataScan0[1] = 0; // Green
                //if (_bitmapDataScan0 != null) _bitmapDataScan0[2] = 0; // Red
                _bitmapDataScan0 += BytesPerPixel;
            }
        }
    }
}