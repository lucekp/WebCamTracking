using System;
using System.Drawing;
using System.Drawing.Imaging;
using Emgu.CV;

namespace WebCamTracking
{
    class WebCam : IDisposable
    {
        private readonly VideoCapture _capture;

        public Bitmap CurrentFrameBitmap;
        public Bitmap PreviousFrameBitmap;

        private PixelFormat PixelFormat = PixelFormat.Format24bppRgb;
        private const int BitsPerPixel = 24;
        private const int BytesPerPixel = (BitsPerPixel + 7) / 8;

        public WebCam()
        {
            _capture = new VideoCapture();
        }

        public void NextFrame()
        {
            int stride = 4 * ((_capture.Width * BytesPerPixel + 3) / 4);
            PreviousFrameBitmap = CurrentFrameBitmap;
            _capture?.Start(); //Starting capture just for capturing the frame
            CurrentFrameBitmap = _capture.QueryFrame().Bitmap;
            _capture?.Stop(); //Stoping capture to prevent memory leak
        }

        public void Dispose()
        {
            _capture?.Stop();
            _capture?.Dispose();
        }
    }
}