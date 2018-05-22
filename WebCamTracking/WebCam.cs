using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;

namespace WebCamTracking
{
    class WebCam:IDisposable
    {
        private readonly VideoCapture capture;
        public WebCam()
        {
            capture = new VideoCapture();
        }
        public Bitmap CurrentFrame()
        {
            capture?.Start(); //Starting capture just for capturing the frame
            Bitmap frame = capture.QueryFrame().Bitmap;
            capture?.Stop(); //Stoping capture to prevent memory leak
            return frame;            
        }

        public void Dispose()
        {
            capture?.Dispose();
            capture?.Stop();
        }
    }
}