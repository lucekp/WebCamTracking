using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WebCamTracking
{
    public class DirectBitmap : IDisposable
    {
        private readonly PixelFormat _pixelFormat = PixelFormat.Format24bppRgb;
        private const int BitsPerPixel = 24;
        private const int BytesPerPixel = (BitsPerPixel + 7) / 8;

        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, _pixelFormat, BitsHandle.AddrOfPinnedObject());
        }

        public DirectBitmap(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            Bits = new Int32[Width * Height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = bitmap;
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();
            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);
            return result;
        }
        public unsafe Bitmap FindColor(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, _pixelFormat);
            byte* bitmapDataScan0 = (byte*)bitmapData.Scan0;

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    if (bitmapDataScan0 != null) bitmapDataScan0[0] = 0; // Blue
                    //if (bitmapDataScan0 != null) bitmapDataScan0[1] = 0; // Green
                    //if (bitmapDataScan0 != null) bitmapDataScan0[2] = 0; // Red
                    bitmapDataScan0 += BytesPerPixel;
                }
            }

            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }
        public void Dispose()
        {
            Bitmap?.Dispose();
        }
    }
}