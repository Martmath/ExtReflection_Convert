using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BackRoom
{
    public class Singleton<T> where T : class
    {
        //protected Singleton() { } 
        private sealed class SingletonCreator<S> where S : class
        {
            private static readonly S instance= (S)typeof(S).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                null,new Type[0],new ParameterModifier[0]).Invoke(null);            
            public static S CreatorInstance
            { 
                get { return instance; }  
            }
        }

        public static T Instance
        {
            get {return SingletonCreator<T>.CreatorInstance; }
        }
    }
  

}
