using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MyGDIFramework
{

    public class GdiSystem : IDisposable
    {
        Form thisWindow;

        /// <summary>
        /// 在FormLoad中调用这个方法
        /// </summary>
        /// <param name="attachForm"></param>
        public GdiSystem(Form attachForm)
        {
            thisWindow = attachForm;
            
            if (attachForm.Handle != IntPtr.Zero)
            {
                int oldWindowLong = Win32.GetWindowLong(attachForm.Handle, Win32.GWL_EXSTYLE);
                Win32.SetWindowLong(attachForm.Handle, Win32.GWL_EXSTYLE, oldWindowLong | Win32.WS_EX_LAYERED);
            }
            else
            {
                throw new AccessViolationException("窗口没有初始化");
            }
            oldBits = IntPtr.Zero;
            screenDC = Win32.GetDC(IntPtr.Zero);
            hBitmap = IntPtr.Zero;
            memDc = Win32.CreateCompatibleDC(screenDC);
            blendFunc.BlendOp = Win32.AC_SRC_OVER;
            blendFunc.SourceConstantAlpha = 255;
            blendFunc.AlphaFormat = Win32.AC_SRC_ALPHA;
            blendFunc.BlendFlags = 0;

            initBitmaps();

        }

        private void initBitmaps()
        {
            thisBitmap = new Bitmap(thisWindow.Width, thisWindow.Height);
            thisGraphics = Graphics.FromImage(thisBitmap);
            bitMapSize = new Win32.Size(thisBitmap.Width, thisBitmap.Height);
        }
        /// <summary>
        /// 获取一个可以绘制的画布对象，在上面绘制窗体内容
        /// </summary>
        /// <param name="clearGraphics"></param>
        /// <returns></returns>
        public Graphics Graphics
        {
            get
            {
                return thisGraphics;
            }
        }
        /// <summary>
        /// 画完之后提交绘制内容
        /// </summary>
        public void UpdateWindow()
        {
            SetBits(thisBitmap);
        }
        IntPtr oldBits;
        IntPtr screenDC;
        IntPtr hBitmap;
        IntPtr memDc;
        Win32.BLENDFUNCTION blendFunc = new Win32.BLENDFUNCTION();

        Win32.Point topLoc = new Win32.Point(0, 0);
        Win32.Size bitMapSize;
        Win32.Point srcLoc = new Win32.Point(0, 0);
        private void SetBits(Bitmap bitmap)
        {

            if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("The picture must be 32bit picture with alpha channel.");
            try
            {
                topLoc.x = thisWindow.Left;
                topLoc.y = thisWindow.Top;
                hBitmap = thisBitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = Win32.SelectObject(memDc, hBitmap);
                Win32.UpdateLayeredWindow(thisWindow.Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32.ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBits);
                    Win32.DeleteObject(hBitmap);
                }
            }
        }

        public void Dispose()
        {
            Win32.ReleaseDC(IntPtr.Zero, screenDC);
            Win32.DeleteDC(memDc);
        }

        private Bitmap thisBitmap;
        private Graphics thisGraphics;


        public class Win32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Size
            {
                public Int32 cx;
                public Int32 cy;

                public Size(Int32 x, Int32 y)
                {
                    cx = x;
                    cy = y;
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct BLENDFUNCTION
            {
                public byte BlendOp;
                public byte BlendFlags;
                public byte SourceConstantAlpha;
                public byte AlphaFormat;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Point
            {
                public Int32 x;
                public Int32 y;

                public Point(Int32 x, Int32 y)
                {
                    this.x = x;
                    this.y = y;
                }
            }

            public const byte AC_SRC_OVER = 0;
            public const Int32 ULW_ALPHA = 2;
            public const byte AC_SRC_ALPHA = 1;
            public const int GWL_EXSTYLE = -20;
            public const int WS_EX_LAYERED = 0x80000;

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("gdi32.dll", ExactSpelling = true)]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

            [DllImport("user32.dll", ExactSpelling = true)]
            public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern int DeleteDC(IntPtr hDC);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern int DeleteObject(IntPtr hObj);

            [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

            [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
            public static extern IntPtr ExtCreateRegion(IntPtr lpXform, uint nCount, IntPtr rgnData);

            [DllImport("user32.dll", EntryPoint = "GetWindowLongA")]
            public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
            public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        }
    }

    public class DrawUtils
    {
        #region drawAlphaImage
        public static void drawAlphaImage(Graphics g, Image image, float x, float y, float w, float h, float alpha)
        {
            if (alpha >= 0.99)
            {
                g.DrawImage(image, x, y, w, h);
                return;
            }
            g.DrawImage(image, new Rectangle((int)x, (int)y, (int)w, (int)h), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, alphaImage(alpha));
        }
        private static ImageAttributes alphaAttrs = new ImageAttributes();
        private static ColorMatrix cmx = new ColorMatrix(new float[][]{
                new float[5]{ 1,0,0,0,0 },
                new float[5]{ 0,1,0,0,0 },
                new float[5]{ 0,0,1,0,0 },
                new float[5]{ 0,0,0,0.5f,0 },
                new float[5]{ 0,0,0,0,0 }
            });
        private static ImageAttributes alphaImage(float alpha)
        {
            cmx.Matrix33 = alpha;
            alphaAttrs.SetColorMatrix(cmx, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return alphaAttrs;
        }
        #endregion

        #region drawRotateImg
        public static void drawRotateImg(Graphics g, Bitmap img, float angle, float centerX, float centerY)
        {
            drawRotateImg(g, (Image)img, angle, centerX, centerY);
        }
        public static void drawRotateImg(Graphics g, Bitmap img, float angle, float centerX, float centerY, float imgW, float imgH)
        {
            drawRotateImg(g, (Image)img, angle, centerX, centerY, imgW, imgH);
        }
        public static void drawRotateImg(Graphics g, Image img, float angle, float centerX, float centerY)
        {
            drawRotateImg(g, img, angle, centerX, centerY, img.Width, img.Height);
        }

        public static void drawRotateImg(Graphics g, Image img, float angle, float centerX, float centerY, float imgW, float imgH)
        {
            float width = imgW;
            float height = imgH;
            Matrix mtrx = new Matrix();
            mtrx.RotateAt(angle, new PointF((width / 2), (height / 2)), MatrixOrder.Append);
            //得到旋转后的矩形
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(new RectangleF(0f, 0f, width, height));
            RectangleF rct = path.GetBounds(mtrx);
            Point Offset = new Point((int)(rct.Width - width) / 2, (int)(rct.Height - height) / 2);
            //构造图像显示区域：让图像的中心与窗口的中心点一致
            RectangleF rect = new RectangleF(-width / 2 + centerX, -height / 2 + centerY, (int)width, (int)height);
            PointF center = new PointF((int)(rect.X + rect.Width / 2), (int)(rect.Y + rect.Height / 2));
            g.TranslateTransform(center.X, center.Y);
            g.RotateTransform(angle);
            //恢复图像在水平和垂直方向的平移
            g.TranslateTransform(-center.X, -center.Y);
            g.DrawImage(img, rect);
            //重至绘图的所有变换
            g.ResetTransform();
        }

        #endregion
    }

    /* 快速图像处理，需要勾选 Properties - 生成 - 允许不安全代码 才能使用，使用时去掉这行即可
    public class FastBitmap
    {
        Bitmap source = null;
        IntPtr Iptr = IntPtr.Zero;
        BitmapData bitmapData = null;

        public int Depth { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public FastBitmap(Bitmap source)
        {
            this.source = source;
        }

        public void LockBits()
        {
            try
            {
                // Get width and height of bitmap
                Width = source.Width;
                Height = source.Height;

                // get total locked pixels count
                int PixelCount = Width * Height;

                // Create rectangle to lock
                Rectangle rect = new Rectangle(0, 0, Width, Height);

                // get source bitmap pixel format size
                Depth = System.Drawing.Bitmap.GetPixelFormatSize(source.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (Depth != 8 && Depth != 24 && Depth != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                // Lock bitmap and return bitmap data
                bitmapData = source.LockBits(rect, ImageLockMode.ReadWrite,
                                             source.PixelFormat);

                //得到首地址
                unsafe
                {
                    Iptr = bitmapData.Scan0;
                    //二维图像循环

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UnlockBits()
        {
            try
            {
                source.UnlockBits(bitmapData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Color GetPixel(int x, int y)
        {
            unsafe
            {
                byte* ptr = (byte*)Iptr;
                ptr = ptr + bitmapData.Stride * y;
                ptr += Depth * x / 8;
                Color c = Color.Empty;
                if (Depth == 32)
                {
                    int a = ptr[3];
                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];
                    c = Color.FromArgb(a, r, g, b);
                }
                else if (Depth == 24)
                {
                    int r = ptr[2];
                    int g = ptr[1];
                    int b = ptr[0];
                    c = Color.FromArgb(r, g, b);
                }
                else if (Depth == 8)
                {
                    int r = ptr[0];
                    c = Color.FromArgb(r, r, r);
                }
                return c;
            }
        }

        public void SetPixel(int x, int y, Color c)
        {
            unsafe
            {
                byte* ptr = (byte*)Iptr;
                ptr = ptr + bitmapData.Stride * y;
                ptr += Depth * x / 8;
                if (Depth == 32)
                {
                    ptr[3] = c.A;
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
                else if (Depth == 24)
                {
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
                else if (Depth == 8)
                {
                    ptr[2] = c.R;
                    ptr[1] = c.G;
                    ptr[0] = c.B;
                }
            }
        }
    }
    /**/
}
