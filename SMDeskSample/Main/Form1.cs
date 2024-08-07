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
            d.FromJSON64("eyJzZF9mcm0iOiJURVNUIiwic2RfZG9jIjoiMTAwMCIsInNkX2VsZSI6IjEzNTkiLCJzZF9ybGMiOjB9");
            textBox2.Text = "";
            d.FromParameters(textBox1.Text);
            for (int i = 0; i < d.Count; i++)
            {
                textBox2.Text += d[i].Key + " = " + SM.Quote2(d[i].Value) + "\r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SM.Databases.Keep("MAIN");
            SM.Login("Email='test@users.com'");
        }
    }
}
