using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV.Structure;

namespace WebCamTracking
{
    public partial class Form1 : Form
    {
        private readonly PictureBox _drawingArea = new PictureBox();
        private readonly WebCam _webCam;

        public Form1()
        {
            InitializeComponent();
            _webCam = new WebCam();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _drawingArea.Dock = DockStyle.Fill;
            _drawingArea.Paint += DrawingArea_Paint;

            Controls.Add(_drawingArea);
        }

        private void DrawingArea_Paint(object sender, PaintEventArgs e)
        {
            _webCam.NextFrame(); //Catch next frame data
            using (DirectBitmap modifiedFrameBitmap = new DirectBitmap(_webCam.CurrentFrameBitmap))
            {
                //modifiedFrameBitmap.SetAllPixels();
                modifiedFrameBitmap.SetPixel(100,200,new Rgb(Color.DarkRed));
                modifiedFrameBitmap.SetPixel(100, 100, new Rgb(Color.DarkRed));
                modifiedFrameBitmap.Unlock();
                e.Graphics.DrawImage(modifiedFrameBitmap.Bitmap, 0, 0);
                e.Graphics.DrawEllipse(new Pen(Color.DarkRed),new Rectangle(90,90,20,20) );
            }
            
            
            //modifiedFrameBitmap.Dispose();
                
            _drawingArea.Invalidate();
        }
    }
}