using Buffalo.IOCP;

namespace NetClientDemo
{
    public partial class Form1 : Form, IConnectMessage
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ucFastNet1.Messbox = this;
            ucWebSocket1.Messbox = this;
        }


        public bool ShowLog
        {
            get
            {
                return true;
            }
        }

        public bool ShowError
        {
            get
            {
                return true;
            }
        }

        public bool ShowWarning
        {
            get
            {
                return true;
            }
        }

        public void Log(string message)
        {

            if (messageBox1.ShowLog)
            {
                this.Invoke((EventHandler)delegate
                {
                    messageBox1.Log(message);
                });
            }
        }
        public void LogError(string message)
        {

            if (messageBox1.ShowError)
            {
                this.Invoke((EventHandler)delegate
                {
                    messageBox1.LogError(message);
                });
            }
        }
        public void LogWarning(string message)
        {

            if (messageBox1.ShowWarning)
            {
                this.Invoke((EventHandler)delegate
                {
                    messageBox1.LogWarning(message);
                });
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ucFastNet1.CloseConnect();
        }

        private void ucWebSocket1_Load(object sender, EventArgs e)
        {

        }

        private void ucFastNet1_Load(object sender, EventArgs e)
        {

        }
    }
}