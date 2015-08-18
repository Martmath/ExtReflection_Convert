using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace BackRoom
{
    public class Magnifier<T, V>
      //  where T : class
        where V : class
    {
        protected Magnifier() { }
        //public class TConvert : Magnifier<TConvert, object>
        private static V _Parent;
        public V Parent
        {
            get { return _Parent; }
            protected internal set { _Parent = value; }
        }
        private static T _Lens;
        public static T GetLens(V obj)//установка линзы (статический Parent) и возврат себя
        {
            if (_Lens == null)
            {
                ConstructorInfo ee = typeof(T).GetConstructor(
                     BindingFlags.Instance | BindingFlags.NonPublic| BindingFlags.Public, null, new Type[0], new ParameterModifier[0]);
                
                _Lens = (T)typeof(T).GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null, new Type[0], new ParameterModifier[0]).Invoke(null);
            }   

            if (!object.ReferenceEquals(_Lens.GetType().GetProperty("Parent").GetValue(_Lens, null), obj)) 
                _Lens.GetType().GetProperty("Parent").SetValue(_Lens, obj, null);
            return _Lens;
        }
    }
}
