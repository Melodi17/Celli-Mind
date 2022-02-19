using System.Drawing.Drawing2D;
using System.Text;

namespace Celli_Mind
{
    public class DisplayController
    {
        /// <summary>
        /// Whether to inturrupt display
        /// </summary>
        private bool interrupt = false;

        /// <summary>
        /// Control to use as "main"
        /// </summary>
        public Control Control { get; set; }

        /// <summary>
        /// Control for displaying emotions
        /// </summary>
        public Control EmotionControl { get; set; }

        /// <summary>
        /// Control for displaying actions
        /// </summary>
        public Control ActionControl { get; set; }

        /// <summary>
        /// Control for displaying output
        /// </summary>
        public RichTextBox OutputControl { get; set; }

        /// <summary>
        /// Timing to use for animation
        /// </summary>
        public int Timing { get; set; }

        /// <summary>
        /// Whether the display is currently busy
        /// </summary>
        public bool Busy => busy;

        /// <summary>
        /// Whether the display is currently busy
        /// </summary>
        private bool busy = false;

        /// <summary>
        /// Action to trigger on ActionControl raises click event
        /// </summary>
        private Action currentAction;

        /// <summary>
        /// Asynchonous thread for button timeout
        /// </summary>
        private Thread currentThread;
        public DisplayController()
        {
            Timing = 25;
        }
        
        /// <summary>
        /// Tnterrupt display
        /// </summary>
        /// <returns>This (for chaining)</returns>
        public DisplayController Interrupt()
        {
            interrupt = true;
            return this;
        }

        /// <summary>
        /// Animate <paramref name="text"/> to output
        /// </summary>
        /// <param name="text">Text to animate</param>
        /// <returns>This (for chaining)</returns>
        public DisplayController Animate(string text)
        {
            AnimateInterrupt(text);
            return this;
        }

        /// <summary>
        /// Animate <paramref name="text"/> to output and handle interrupts
        /// </summary>
        /// <param name="text">Text to animate</param>
        /// <returns>This (for chaining)</returns>
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

        /// <summary>
        /// Change main control size to small
        /// </summary>
        /// <returns>This (for chaining)</returns>
        public DisplayController Small()
        {
            Control.Invoke(new Action(() =>
            {
                Control.Height = 150;
                Control.Refresh();
            }));

            return this;
        }

        /// <summary>
        /// Change main control size to large
        /// </summary>
        /// <returns>This (for chaining)</returns>
        public DisplayController Large()
        {
            Control.Invoke(new Action(() =>
            {
                Control.Height = 500;
                Control.Refresh();
            }));

            return this;
        }

        /// <summary>
        /// Clear output text
        /// </summary>
        /// <returns>This (for chaining)</returns>
        public DisplayController Clear()
        {
            OutputControl.Invoke(() =>
            {
                OutputControl.Clear();
            });

            return this;
        }

        /// <summary>
        /// Write <paramref name="text"/> to output
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <returns>This (for chaining)</returns>
        public DisplayController Write(string text)
        {
            OutputControl.Invoke(() =>
            {
                OutputControl.Text = text;
                OutputControl.Refresh();
            });

            return this;
        }

        /// <summary>
        /// Wait for <paramref name="time"/> milliseconds before continuing thread
        /// </summary>
        /// <param name="time">Time to wait</param>
        /// <returns>This (for chaining)</returns>
        public DisplayController Sleep(int time)
        {
            Thread.Sleep(time);
            return this;
        }

        /// <summary>
        /// Displays action button with <paramref name="text"/> in <paramref name="color"/>,
        /// clicking will raise specified action, button will dissapear after <paramref name="timeout"/> milliseconds
        /// </summary>
        /// <param name="text">Button text</param>
        /// <param name="color">Button text color</param>
        /// <param name="action">Button click action</param>
        /// <param name="timeout">Button display timeout</param>
        /// <returns>This (for chaining)</returns>
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

        /// <summary>
        /// Remove action button
        /// </summary>
        /// <returns>This (for chaining)</returns>
        public DisplayController Action()
        {
            currentAction = null;
            EmotionControl.Invoke(new Action(() =>
            {
                ActionControl.Visible = false;
            }));

            return this;
        }

        /// <summary>
        /// Action button click event raised
        /// </summary>
        /// <param name="sender">Disposed</param>
        /// <param name="e">Disposed</param>
        private void ActionControl_Click(object? sender, EventArgs e)
        {
            ActionControl.Visible = false;
            currentAction?.Invoke();
        }

        /// <summary>
        /// Display emotion with <paramref name="text"/> in <paramref name="color"/>
        /// </summary>
        /// <param name="text">Text of emotion</param>
        /// <param name="color">Color of emotion</param>
        /// <returns>This (for chaining)</returns>
        public DisplayController Emotion(string text, Color color)
        {
            EmotionControl.Invoke(new Action(() =>
            {
                EmotionControl.Text = text;
                EmotionControl.ForeColor = color;
                EmotionControl.Visible = text.Length > 0;
            }));

            return this;
        }

        /// <summary>
        /// Display emotion from <paramref name="emotion"/>
        /// </summary>
        /// <param name="emotion">Emotion to display</param>
        /// <returns>This (for chaining)</returns>
        public DisplayController Emotion(DisplayEmotion emotion)
        {
            Emotion(emotion.Raw, emotion.Color);
            return this;
        }

        /// <summary>
        /// Clear emotion
        /// </summary>
        /// <returns>This (for chaining)</returns>
        public DisplayController Emotion()
        {
            Emotion("", Color.Black);
            return this;
        }
    }

    public class DisplayEmotion
    {
        /// <summary>
        /// Text of emotion
        /// </summary>
        public string Raw;

        /// <summary>
        /// Color of emotion
        /// </summary>
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

        /// <summary>
        /// Convert <paramref name="emotion"/> to text implicitly
        /// </summary>
        /// <param name="emotion">emotion to convert</param>
        public static implicit operator string(DisplayEmotion emotion) => emotion.Raw;
    }
}