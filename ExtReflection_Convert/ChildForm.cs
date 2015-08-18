using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BackRoom;

namespace ExtReflection_Convert
{
    public partial class ChildForm : Form
    {
        public ChildForm()
        {
            InitializeComponent();
        }
       
        private void ChildForm_VisibleChanged(object sender, EventArgs e)
        {
            this.label1.Text = ((Button)ParentChildren.Instance.Get_Parent(this)).Text;
        }
    }
}
