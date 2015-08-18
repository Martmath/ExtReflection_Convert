using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.ComponentModel;

namespace BackRoom
{
    class ParentChildren : TMapper<ParentChildren>
    {
        private const string kParentPullName = "Parent";
        private const string kParentName = kParentPullName;
        private const string kChildPullName = "Children";
        protected static ExtParentChildren Extensio = new ExtParentChildren();

       // protected partial class ExtParentChildren : ParentChildren
       // { internal ExtParentChildren() { } }

        private ParentChildren()       
        {
            this.add_Mapper(kParentPullName);
            this.add_Mapper(kChildPullName);
        }
        
        public void Add_Children(object Parent, string ChildName, object Child)
        {
            this.add_LinkedObject(kParentPullName, Parent, ChildName, Child, true);
            this.add_LinkedObject(kChildPullName, Child, kParentName, Parent, true);
        }
        public object Get_Parent(object Child)
        {
            return this.Get_LinkedObject(kChildPullName, Child, kParentName);
        }
        public object Get_Child(object Parent, string ChildName)
        {
            return this.Get_LinkedObject(kParentPullName, Parent, ChildName);
        }
        public bool Delete_Link(object Parent, string ChildName)
        {
            object Child = Get_Child(Parent, ChildName);
            return this.Delete_LinkedObject(kParentPullName, Parent, ChildName, true) &&
                   this.Delete_LinkertoObjects(kChildPullName, Child, false, true);
        }
    }
    public partial class ExtParentChildren
    {
        // protected void InitGenericFunctionMap()
        // {    }
    }    
}

