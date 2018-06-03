using System;
using System.Drawing;
using System.Windows.Forms;

namespace WebCamTracking
{
    public partial class Form1 : Form
    {
        private readonly WebCam _webCam;
        private readonly PictureBox _drawingArea = new PictureBox();

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
            e.Graphics.DrawImage(_webCam.CurrentFrameBitmap, 0, 0);
            _drawingArea.Invalidate();
        }
    }
}