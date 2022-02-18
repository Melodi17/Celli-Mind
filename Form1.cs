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

            display = new();
            display.Control = this;
            display.EmotionControl = Emotion_Label;
            display.ActionControl = Action_Button;
            display.OutputControl = Output_Box;
            display.Timing = 15;
            display.Small();

            random = new();
            LanguageController.Random = random;

            mind = new();
            mind.Display = display;
            mind.Random = random;
            mind.Start();

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

            mind.Send(MindContextTrigger.Quit);
        }

        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);

        private void Output_Box_Click(object sender, EventArgs e)
        {
            HideCaret(Output_Box.Handle);
        }

        private void Output_Box_Validated(object sender, EventArgs e)
        {
            HideCaret(Output_Box.Handle);
        }
    }
}