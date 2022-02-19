using System.Runtime.InteropServices;

namespace Celli_Mind
{
    public partial class Form1 : Form
    {
        DisplayController display;
        MindController mind;
        RandomController random;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Setup display controller
            display = new();
            display.Control = this;
            display.EmotionControl = Emotion_Label;
            display.ActionControl = Action_Button;
            display.OutputControl = Output_Box;
            display.Timing = 15;
            display.Small();

            // Setup random controller and language controller
            random = new();
            LanguageController.Random = random;

            // Setup mind controller
            mind = new();
            mind.Display = display;
            mind.Random = random;
            mind.Start();

            /* Position window in botom right corner of screen,
             * using padding and accomodating for taskbar height
             */
            int padding = 10;
            this.Location = new(Screen.PrimaryScreen.WorkingArea.Right - (padding + Width),
                Screen.PrimaryScreen.WorkingArea.Bottom - (padding + Height));
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            // Cancel any actions
            display.Action();

            // Interrupt and finish animation process
            display.Interrupt();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prevent cancel
            e.Cancel = true;

            // Send quit event to the mind controller
            mind.Send(MindContextTrigger.Quit);
        }

        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        private void Output_Box_Click(object sender, EventArgs e)
        {
            // Hide caret using P/Invoke
            HideCaret(Output_Box.Handle);
        }

        private void Output_Box_Validated(object sender, EventArgs e)
        {
            // Hide caret using P/Invoke
            HideCaret(Output_Box.Handle);
        }
    }
}