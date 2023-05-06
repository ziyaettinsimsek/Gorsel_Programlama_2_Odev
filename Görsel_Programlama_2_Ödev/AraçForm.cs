using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Görsel_Programlama_2_Ödev
{
    public partial class AraçForm : Form
    {
        public AraçForm()
        {
            InitializeComponent();
        }
        public Car Car = null;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            propertyGrid1.SelectedObject = Car;
        }

        private void OKClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
