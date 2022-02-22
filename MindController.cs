using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.InteropServices;
using IronOcr;

namespace Celli_Mind
{
    public class MindController
    {
        /// <summary>
        /// Methods with MindController attribute
        /// </summary>
        public List<MethodInfo> MethodInfos;

        /// <summary>
        /// Display controller to use
        /// </summary>
        public DisplayController Display;

        /// <summary>
        /// Random controllerto use
        /// </summary>
        public RandomController Random;

        /// <summary>
        /// Whether mind controller is running
        /// </summary>
        private bool running = false;
        public MindController()
        {
            // Find all methods with a MindController attribute and save them
            MethodInfos = typeof(MindController).GetMethods()
                .Where(x => x.GetCustomAttribute<MindContextAttribute>() != null)
                .ToList();
        }

        /// <summary>
        /// Start mind controller
        /// </summary>
        public void Start()
        {
            running = true;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                Send(MindContextTrigger.Start);

                while (running)
                {
                    Send(MindContextTrigger.Loop);

                    Thread.Sleep(3000);
                }
            }).Start();
        }

        /// <summary>
        /// Send <paramref name="context"/> to all methods availabe for <paramref name="trigger"/>
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="context"></param>
        public void Send(MindContextTrigger trigger, object context = null)
        {
            foreach (var item in MethodInfos)
                if (item.GetCustomAttribute<MindContextAttribute>().Trigger.HasFlag(trigger))
                    item.Invoke(this, new object[] { context });
        }

        /// <summary>
        /// Stop mind controller
        /// </summary>
        public void Stop()
        {
            running = false;
        }

        #region Processing Triggers

        [MindContext(MindContextTrigger.Start)]
        public void Ctx_Start(object ctx)
        {
            Display.Emotion(Random.Chance(30) ? new DisplayEmotion[]
            { DisplayEmotion.Happy, DisplayEmotion.Normal, DisplayEmotion.Blush, DisplayEmotion.Excited }
                .Random() : DisplayEmotion.Normal);

            Display.Animate(LanguageController.Statement.Greeting);
        }


        [MindContext(MindContextTrigger.Quit)]
        public void Ctx_Quit(object ctx)
        {
            Display.Emotion(DisplayEmotion.Sad);
            Display.Action("Quit", Color.Red, () => Environment.Exit(0));
            Display.Animate(LanguageController.Question.Quit);
        }


        [MindContext(MindContextTrigger.Loop)]
        public void Ctx_Time(object ctx)
        {
            if (Random.Chance(3) && !Display.Busy)
            {
                Display.Emotion(DisplayEmotion.Normal);
                Display.Animate(Random.Chance(20)
                    ? LanguageController.Response.Day
                    : LanguageController.Response.Time);
            }
        }


        [MindContext(MindContextTrigger.Start)]
        public void Ctx_ScreenContext(object ctx)
        { 
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                while (true)
                {
                    // Screenshot screen, scan result and convert to text
                    Image img = PrintScreen.CaptureScreen();
                    OcrResult result = new IronTesseract().Read(img);
                    string text = result.Text;

                    
                    Thread.Sleep(10000);
                }
            }).Start();
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class MindContextAttribute : Attribute
    {
        public readonly MindContextTrigger Trigger;

        // This is a positional argument
        public MindContextAttribute(MindContextTrigger trigger)
        {
            this.Trigger = trigger;
        }
    }

    [Flags]
    public enum MindContextTrigger
    {
        Start = 1,
        Quit = 2,
        Application = 4,
        Loop = 8,
        Custom = 16,
    }

    public static class PrintScreen
    {
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest, hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);

            return img;
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        /// <summary>
        /// Helper class containing Gdi32 API functions
        /// </summary>
        private class GDI32
        {

            public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

            [DllImport("gdi32.dll")]
            public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
                int nWidth, int nHeight, IntPtr hObjectSource,
                int nXSrc, int nYSrc, int dwRop);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
                int nHeight);
            [DllImport("gdi32.dll")]
            public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteDC(IntPtr hDC);
            [DllImport("gdi32.dll")]
            public static extern bool DeleteObject(IntPtr hObject);
            [DllImport("gdi32.dll")]
            public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        }

        /// <summary>
        /// Helper class containing User32 API functions
        /// </summary>
        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetDesktopWindow();
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowDC(IntPtr hWnd);
            [DllImport("user32.dll")]
            public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        }
    }
}