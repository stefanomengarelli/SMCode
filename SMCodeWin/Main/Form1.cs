using SMCode;

namespace SMCodeWin
{
    public partial class Form1 : Form
    {

        private SMCode.SMApplication SM;

        public Form1(SMCode.SMApplication _SMApplication)
        {
            SM = _SMApplication;
            InitializeComponent();
        }
    }
}
