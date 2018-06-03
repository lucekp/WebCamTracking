using System;
using System.Drawing;
using Emgu.CV;

namespace WebCamTracking
{
    internal class WebCam : IDisposable
    {
        private readonly VideoCapture _capture;
        public Bitmap CurrentFrameBitmap;
        public Bitmap PreviousFrameBitmap;

        public WebCam()
        {
            _capture = new VideoCapture();
            if (_capture == null) return;
            _capture.Start();
            CurrentFrameBitmap = new Bitmap(_capture.QueryFrame().Bitmap);
            PreviousFrameBitmap = new Bitmap(CurrentFrameBitmap);
            _capture.Stop();
        }

        public void Dispose()
        {
            _capture?.Stop();
            _capture?.Dispose();
        }

        public void NextFrame()
        {
            _capture.Start(); //Starting capture just for capturing the frame
            PreviousFrameBitmap = new Bitmap(CurrentFrameBitmap);
            CurrentFrameBitmap = new Bitmap(_capture.QueryFrame().Bitmap);
            _capture.Stop(); //Stoping capture to prevent memory leak
        }
    }
}