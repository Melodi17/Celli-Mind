using System.Drawing.Drawing2D;
using System.Text;

namespace Celli_Mind
{
    public class DisplayController
    {
        private bool interrupt = false;
        public Control Control { get; set; }
        public Control EmotionControl { get; set; }
        public Control ActionControl { get; set; }
        public RichTextBox OutputControl { get; set; }
        public int Size { get; set; }
        public int Width { get; set; }
        public int Timing { get; set; }
        public bool Busy => busy;
        private bool busy = false;

        private int offset = 0;
        private Action currentAction;
        private Thread currentThread;
        public DisplayController()
        {
            Timing = 25;
            Size = 15;
        }
        public DisplayController Interrupt()
        {
            interrupt = true;
            return this;
        }
        public DisplayController Animate(string text)
        {
            AnimateInterrupt(text);
            return this;
        }
        private DisplayController AnimateInterrupt(string text)
        {
            busy = true;
            Clear();
            foreach (char item in text)
            {
                if (interrupt)
                {
                    Write(text);
                    interrupt = false;
                    return this;
                }

                OutputControl.Invoke(new Action(() =>
                {
                    OutputControl.Text += item;
                    OutputControl.Refresh();
                }));
                Thread.Sleep(Timing);
            }
            busy = false;

            return this;
        }
        public DisplayController Small()
        {
            Control.Invoke(new Action(() =>
            {
                Control.Height = 150;
                Control.Refresh();
            }));

            return this;
        }
        public DisplayController Large()
        {
            Control.Invoke(new Action(() =>
            {
                Control.Height = 500;
                Control.Refresh();
            }));

            return this;
        }
        public DisplayController Clear()
        {
            OutputControl.Invoke(() =>
            {
                OutputControl.Clear();
            });

            return this;
        }
        public DisplayController Write(string text)
        {
            OutputControl.Invoke(() =>
            {
                OutputControl.Text = text;
                OutputControl.Refresh();
            });

            return this;
        }
        public DisplayController Sleep(int time)
        {
            Thread.Sleep(time);
            return this;
        }
        public DisplayController Action(string text, Color color, Action action, int timeout = 10000)
        {
            currentAction = action;
            if (currentThread != null)
            {
                try
                {
                    currentThread.Abort();
                }
                catch { /* Don't Care */ }
            }

            ActionControl.Invoke(new Action(() =>
            {
                ActionControl.Text = text;
                ActionControl.ForeColor = color;
                ActionControl.Click += ActionControl_Click;
                ActionControl.Visible = true;
            }));

            currentThread = new(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                Thread.Sleep(timeout);
                Action();
            });

            currentThread.Start();

            return this;
        }
        public DisplayController Action()
        {
            currentAction = null;
            EmotionControl.Invoke(new Action(() =>
            {
                ActionControl.Visible = false;
            }));

            return this;
        }
        private void ActionControl_Click(object? sender, EventArgs e)
        {
            ActionControl.Visible = false;
            currentAction?.Invoke();
        }
        public DisplayController Emotion(string text, Color color)
        {
            EmotionControl.Invoke(new Action(() =>
            {
                EmotionControl.Text = text;
                EmotionControl.ForeColor = color;
                EmotionControl.Visible = text.Length > 0;
            }));

            offset = text.Length > 0 ? (EmotionControl?.Bottom ?? 0) : 0;

            return this;
        }
        public DisplayController Emotion(DisplayEmotion emotion)
        {
            Emotion(emotion.Raw, emotion.Color);
            return this;
        }
        public DisplayController Emotion()
        {
            Emotion("", Color.Black);
            return this;
        }
        public static List<string> WrapText(string text, double pixels, Font font)
        {
            string[] originalLines = text.Split(new string[] { " " },
                StringSplitOptions.None);

            List<string> wrappedLines = new List<string>();

            StringBuilder actualLine = new();
            double actualWidth = 0;

            foreach (var item in originalLines)
            {
                int w = TextRenderer.MeasureText(item + " ", font).Width / 2;
                actualWidth += w;

                if (actualWidth > pixels)
                {
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualWidth = w;
                }

                actualLine.Append(item + " ");
            }

            if (actualLine.Length > 0)
                wrappedLines.Add(actualLine.ToString());

            return wrappedLines;
        }
    }

    public class DisplayEmotion
    {
        public string Raw;
        public Color Color;
        public DisplayEmotion(string raw, Color color)
        {
            Raw = raw;
            Color = color;
        }

        public static DisplayEmotion Happy;
        public static DisplayEmotion Excited;
        public static DisplayEmotion Blush;
        public static DisplayEmotion Sad;
        public static DisplayEmotion Normal;
        static DisplayEmotion()
        {
            Happy = new("(＾◡＾✿)", Color.White);
            Excited = new("(((o(*°▽°*)o)))", Color.FromArgb(252, 219, 3));
            Blush = new("(〃>◡<〃)❤", Color.FromArgb(252, 3, 248));
            Sad = new("(·•︵•̀)", Color.FromArgb(3, 161, 252));
            Normal = new("(• ◡ •)", Color.White);
        }

        public static implicit operator string(DisplayEmotion emotion) => emotion.Raw;
    }
}