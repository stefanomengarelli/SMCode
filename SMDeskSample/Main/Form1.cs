using SMCodeSystem;
using SMDeskSystem;

namespace SMDeskSample
{
    public partial class Form1 : Form
    {

        private SMDesk SM;

        public Form1(SMDesk _SM)
        {
            SM = _SM;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = textBox1.Text;
            SMDictionary d = new SMDictionary();
            textBox2.Text = "";
            d.FromJSON(textBox1.Text);
            for (int i = 0; i < d.Count; i++)
            {
                textBox2.Text += d[i].Key + " = " + SM.Quote2(d[i].Value) + "\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FileInfo[] files = SM.FileList(@"\\192.168.0.10\PhotoShots\*.*", true);
            textBox2.Text = SM.User.ToJSON();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SM.Login(2);
            SM.Login(1);
            textBox1.Text = SM.HashSHA256("5MC0d3-M@5t3rK3y");
            textBox1.Text = SM.ToStr(SM.Easter(2024));
        }
    }
}
