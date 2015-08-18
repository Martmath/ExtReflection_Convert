using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using BackRoom;
namespace ExtReflection_Convert
{    
    public partial class TestForm : Form,IGetSet
    {
        public TestForm()
        {
            InitializeComponent();
        }
      /*public CLS_Convertable<IExtConvertible> Convertable()
        {return new CLS_Convertable<IExtConvertible>(this);}*/
        private int test1 = 12;
        private List<int> test2 = new List<int> { 12, 56 };
        public string Test<A,B>(string First, A Second,B Third,int Fourth)
        {
            return First.ToString() + "_" + Second.ToString() + "_" + Third.ToString() + "_" + Fourth.ToString();
        }
        public string Test<A>(string First, A Second)
        {
            return First.ToString() + "+" + Second.ToString() ;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string t1 = this.GetSet().GetFieldValue<string>("test2");
            string t2 = this.GetSet().GetFullNameClass("Int32");
            object t3 = this.GetSet().GetFieldValue("test1");
            this.GetSet().SetFieldValue("test1", 4566);
            MethodInfo t4 = this.GetSet().GetMethod(this.GetType(), "Test", "string", "string", "uint", "int");
            object t5 = this.GetSet().InvokeFromString(t4, this, "4r", "oo", "12", "34");
            MethodInfo t6 = this.GetSet().GetMethod(this.GetType(), "Test", "string", "string");
            object t7 = this.GetSet().InvokeFromString(t6, this, "4r", "oo");
            MethodInfo t8 = this.GetSet().GetMethod(this.GetType(), "Test", "string", "string","int");
        }  

        private void button3_Click(object sender, EventArgs e)
        {
          //Load before using findme32.exe or findme32.exe
          IntPtr MainWin = WApi.GetMainWin("findme64", "#32770", "Findme", "Findme");
          MainWin = (MainWin == IntPtr.Zero) ? WApi.GetMainWin("findme32", "#32770", "Findme", "Findme") : MainWin;
          IntPtr ChildWin = WApi.GetChildWindow(MainWin, 2, "SysHeader32", 0, "", 8, 63, "SysListView32", 1000, "List1", 1);
          MessageBox.Show(SysHeader32.GetItemString(ChildWin, 3));
        }
       
        private void button4_Click(object sender, EventArgs e)
        {
            object r = ParentChildren.Instance.Get_Child(this.button4, "ChForm");
            if (r == null)
            {
                r = ParentChildren.Instance.Get_Child(this.button5, "ChForm");
                if (r != null)
                {                    
                    ParentChildren.Instance.Delete_Link(this.button5, "ChForm");
                    ParentChildren.Instance.Add_Children(this.button4, "ChForm", r);
                    ((ChildForm)r).Hide();
                }
                else
                {
                    r = new ChildForm();
                    ParentChildren.Instance.Add_Children(this.button4, "ChForm", r);                    
                }
            }            
            ((ChildForm)r).Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            object r = ParentChildren.Instance.Get_Child(this.button5, "ChForm");
            if (r == null)
            {
                r = ParentChildren.Instance.Get_Child(this.button4, "ChForm");
                if (r != null)
                {
                    ParentChildren.Instance.Delete_Link(this.button4, "ChForm");
                    ParentChildren.Instance.Add_Children(this.button5, "ChForm", r);
                    ((ChildForm)r).Hide();
                }
                else
                {
                    r = new ChildForm();
                    ParentChildren.Instance.Add_Children(this.button5, "ChForm", r);
                }
            }
            ((ChildForm)r).Show();
        }
    }  
}
