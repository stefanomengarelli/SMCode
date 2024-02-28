using SMCode;

namespace SMCodeWin
{
    public partial class Form1 : Form
    {

        private SMCode.SM SM;

        public Form1(SMCode.SM _SM)
        {
            SM = _SM;
            InitializeComponent();
        }
    }
}
