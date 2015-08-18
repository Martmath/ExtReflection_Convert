using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BackRoom;
using System.Reflection;
using System.Collections;
using System.Diagnostics;

    public interface IConvert
    {
    }
    public static class CLS_Convert
    {
        public static TConvert TC;
        public static TConvert ClassConvert(this IConvert obj)
        {
            TC = (TC == null) ? new TConvert() : TC;
            return TC;
        }
    }

    public class TConvert
    {  
        enum SupIC
        {
            IDict,
            IArr,
            IList,
            Nullable
        }
        private static Type GType(string S)
        {
            return Type.GetType(S) ?? AppDomain.CurrentDomain.GetAssemblies().Select(assembly => assembly.GetType(S))
                       .Where(type => type != null).FirstOrDefault();
        }
        public object ConvertData(string obj, Type OutType, string Delim = "")
        {
            string S = System.Reflection.MethodBase.GetCurrentMethod().Name;
            MethodInfo M = this.GetType().GetMethods().
                FirstOrDefault(x => ((x.Name == S) && (x.GetGenericArguments().Length == 2)));
            M = M.MakeGenericMethod(OutType, typeof(string));
            return M.Invoke(this, new object[] { obj, Delim });
        }
        public To ConvertData<To>(object obj, string Delim = "")
        {           
           string S= System.Reflection.MethodBase.GetCurrentMethod().Name;
           MethodInfo M = this.GetType().GetMethods().
               FirstOrDefault(x => ((x.Name == S) && (x.GetGenericArguments().Length == 2)));
            M= M.MakeGenericMethod(typeof(To), obj.GetType()); 
            return (To)M.Invoke(this, new object[] { obj, Delim });
        }
        public object ConvertData(object obj,Type OutType, string Delim = "")
        {
            string S = System.Reflection.MethodBase.GetCurrentMethod().Name;
            MethodInfo M = this.GetType().GetMethods().
                FirstOrDefault(x => ((x.Name == S) && (x.GetGenericArguments().Length == 2)));
            M = M.MakeGenericMethod(OutType, obj.GetType());
            return M.Invoke(this, new object[] { obj, Delim });
        }
        public To ConvertData<To, From>(From obj, string Delim = "")
        {
            SupIC FromT = SupIC.Nullable, ToT = SupIC.Nullable;
            Func<Type, Type, bool> getInterface = ((T, t) => t.IsAssignableFrom(T) ? true : false);
            if (getInterface(typeof(From), typeof(ICollection)))
            {
                if (getInterface(typeof(From), typeof(Array))) FromT = SupIC.IArr;
                else if (getInterface(typeof(From), typeof(IDictionary))) FromT = SupIC.IDict;
                else if (getInterface(typeof(From), typeof(IList))) FromT = SupIC.IList;
                else return default(To);
            }
            if (getInterface(typeof(To), typeof(ICollection)))
            {
                if (getInterface(typeof(To), typeof(Array))) ToT = SupIC.IArr;
                else if (getInterface(typeof(To), typeof(IDictionary))) ToT = SupIC.IDict;
                else if (getInterface(typeof(To), typeof(IList))) ToT = SupIC.IList;
                else return default(To);
            }
            if ((FromT == SupIC.Nullable) && (ToT == SupIC.Nullable)) return ConvertSimpleData<To, From>(obj);
            MethodInfo MI = System.Reflection.MethodBase.GetCurrentMethod() as MethodInfo;
            Type TO = typeof(To);
            Type FROM = typeof(From);
            Func<Array, List<int>> AGetRange = X =>
            {
                List<int> Result = new List<int>();
                for (int j = 0; j < X.Rank; j++) Result.Add(X.GetLength(j));
                return Result;
            };
            Func<int, List<int>, List<int>> AGetNumbers = ((Num, Range) =>
            {
                List<int> Result = new List<int>();
                for (int j = 1; j < Range.Count; j++)
                {
                    int Helper = 1;
                    for (int k = j; k < Range.Count; k++) Helper = Helper * Range[k];
                    Result.Add(Num / Helper);
                    Num = Num % Helper;
                }
                Result.Add(Num);
                return Result;
            });
            if (FromT == SupIC.IArr)
            {
                Array f = obj as Array;
                IList l = obj as IList;
                if (ToT == SupIC.IArr)
                {
                    int N = typeof(To).Name.Count(x => x == ',') + 1;
                    if (N == f.Rank)
                    {
                        List<int> ARange = AGetRange(f);
                        Array r = (Array)Activator.CreateInstance(TO, AGetRange(f).Cast<object>().ToArray());
                        MI = MI.MakeGenericMethod(r.GetType().GetElementType(), f.GetType().GetElementType());
                        int j = 0;
                        foreach (var e in l)
                        {
                            r.SetValue(MI.Invoke(this, new object[] { e, Delim }), AGetNumbers(j, ARange).ToArray());
                            j = j + 1;
                        }
                        return (To)(object)r;
                    }
                    else return default(To);
                }
                else if (ToT == SupIC.IDict)
                {
                    IDictionary r = (IDictionary)NewInstance.GetInstance(GType(TO.FullName));
                    int j = 0;
                    MethodInfo MI0 = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[0], j.GetType());
                    MethodInfo MI1 = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[1], f.GetType().GetElementType());
                    foreach (var e in l)
                    {
                        r.Add(MI0.Invoke(this, new object[] { j, Delim }), MI1.Invoke(this, new object[] { e, Delim }));
                        j = j + 1;
                    }
                    return (To)(object)r;
                }
                else if (ToT == SupIC.IList)
                {
                    IList r = (IList)NewInstance.GetInstance(GType(TO.FullName));
                    int j = 0;
                    MI = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[0], f.GetType().GetElementType());
                    foreach (var e in l) r.Add(MI.Invoke(this, new object[] { e, Delim }));
                    return (To)(object)r;
                }
                else
                {
                    string S = "";
                    MethodInfo MI0 = MI.MakeGenericMethod(typeof(string), f.GetType().GetElementType());
                    MI = MI.MakeGenericMethod(TO, typeof(string));
                    foreach (var e in l) S = S + (string)MI0.Invoke(this, new object[] { e, Delim }) + Delim;
                    S = S.Substring(0, S.Length - Delim.Length);
                    return (To)MI.Invoke(this, new object[] { S, Delim });
                }
            }
            else if (FromT == SupIC.IDict)
            {
                IDictionary f = obj as IDictionary;

                if (ToT == SupIC.IArr)
                {
                    int N = typeof(To).Name.Count(x => x == ',') + 1;
                    List<int> ARange = new List<int>(N);
                    for (int i = 0; i < N - 1; i++) ARange.Add(1);
                    ARange.Add(f.Count);
                    Array r = (Array)Activator.CreateInstance(TO, ARange.Cast<object>().ToArray());
                    MI = MI.MakeGenericMethod(r.GetType().GetElementType(), f.GetType().GetGenericArguments()[1]);
                    int j = 0;
                    foreach (var e in f)
                    {
                        object E = e.GetType().GetProperty("Value").GetValue(e, null);
                        r.SetValue(MI.Invoke(this, new object[] { E, Delim }), AGetNumbers(j, ARange).ToArray());
                        j = j + 1;
                    }
                    return (To)(object)r;
                }
                else if (ToT == SupIC.IDict)
                {
                    IDictionary r = (IDictionary)NewInstance.GetInstance(GType(TO.FullName));
                    MethodInfo MI0 = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[0], f.GetType().GetGenericArguments()[0]);
                    MethodInfo MI1 = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[1], f.GetType().GetGenericArguments()[1]);
                    foreach (var e in f)
                    {
                        object E0 = e.GetType().GetProperty("Key").GetValue(e, null);
                        object E1 = e.GetType().GetProperty("Value").GetValue(e, null);
                        r.Add(MI0.Invoke(this, new object[] { E0, Delim }), MI1.Invoke(this, new object[] { E1, Delim }));
                    }
                    return (To)(object)r;
                }
                else if (ToT == SupIC.IList)
                {
                    IList r = (IList)NewInstance.GetInstance(GType(TO.FullName));
                    int j = 0;
                    MI = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[0], f.GetType().GetGenericArguments()[1]);
                    foreach (var e in f)
                    {
                        object E = e.GetType().GetProperty("Value").GetValue(e, null);
                        r.Add(MI.Invoke(this, new object[] { E, Delim }));
                        j = j + 1;
                    }
                    return (To)(object)r;
                }
                else
                {
                    string S = "";
                    MethodInfo MI0 = MI.MakeGenericMethod(typeof(string), f.GetType().GetGenericArguments()[1]);
                    MI = MI.MakeGenericMethod(TO, typeof(string));
                    foreach (var e in f)
                    {
                        object E = e.GetType().GetProperty("Value").GetValue(e, null);
                        S = S + (string)MI0.Invoke(this, new object[] { E, Delim }) + Delim;
                    }
                    S = S.Substring(0, S.Length - Delim.Length);
                    return (To)MI.Invoke(this, new object[] { S, Delim });
                }
            }
            else if (FromT == SupIC.IList)
            {
                IList l = obj as IList;
                if (ToT == SupIC.IArr)
                {
                    int N = typeof(To).Name.Count(x => x == ',') + 1;
                    List<int> ARange = new List<int>(N);
                    for (int i = 0; i < N - 1; i++) ARange.Add(1);
                    ARange.Add(l.Count);
                    Array r = (Array)Activator.CreateInstance(TO, ARange.Cast<object>().ToArray());
                    MI = MI.MakeGenericMethod(r.GetType().GetElementType(), l.GetType().GetGenericArguments()[0]);
                    int j = 0;
                    foreach (var e in l)
                    {
                        r.SetValue(MI.Invoke(this, new object[] { e, Delim }), AGetNumbers(j, ARange).ToArray());
                        j = j + 1;
                    }
                    return (To)(object)r;
                }
                else if (ToT == SupIC.IDict)
                {
                    IDictionary r = (IDictionary)NewInstance.GetInstance(GType(TO.FullName));
                    int j = 0;
                    MethodInfo MI0 = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[0], j.GetType());
                    MethodInfo MI1 = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[1], l.GetType().GetGenericArguments()[0]);
                    foreach (var e in l)
                    {
                        r.Add(MI0.Invoke(this, new object[] { j, Delim }), MI1.Invoke(this, new object[] { e, Delim }));
                        j = j + 1;
                    }
                    return (To)(object)r;
                }
                else if (ToT == SupIC.IList)
                {
                    IList r = (IList)NewInstance.GetInstance(GType(TO.FullName));
                    MI = MI.MakeGenericMethod(r.GetType().GetGenericArguments()[0], l.GetType().GetGenericArguments()[0]);
                    foreach (var e in l) r.Add(MI.Invoke(this, new object[] { e, Delim }));
                    return (To)(object)r;
                }
                else
                {
                    string S = "";
                    MethodInfo MI0 = MI.MakeGenericMethod(typeof(string), l.GetType().GetGenericArguments()[0]);
                    MI = MI.MakeGenericMethod(TO, typeof(string));
                    foreach (var e in l) S = S + (string)MI0.Invoke(this, new object[] { e, Delim }) + Delim;
                    S = S.Substring(0, S.Length - Delim.Length);
                    return (To)MI.Invoke(this, new object[] { S, Delim });
                }
            }
            return default(To);
        }

        private To ConvertSimpleData<To, From>(From obj)
        {
            To t = default(To);
            Type C = typeof(To).GetInterface("IConvertible");
            if ((C != null) && (obj is IConvertible))
            {
                t = (To)Convert.ChangeType(obj, typeof(To));
            }
            else
            {
                MethodInfo M = obj.GetType().GetMethod("ToString", Type.EmptyTypes);
                if (M != null)
                {
                    string S = (string)M.Invoke(obj, null);
                    if (C != null)
                    {
                        t = (To)Convert.ChangeType(S, typeof(To));
                    }
                    else
                    {
                        ConstructorInfo CI = GetFromStringConstructor(typeof(To));
                        object[] o = null;
                        if (CI != null)
                        {
                            o = new object[1] { (object)S };
                        }
                        else
                        {
                            CI = GetIConvertableConstructor(typeof(To));
                            Type tt = CI.GetParameters()[0].ParameterType;
                            o = new object[1] { Convert.ChangeType(S, tt) };
                        }
                        t = (To)CI.Invoke(o);
                    }
                }
            }
            return t;
        }

        public string ConvertData<From>(From obj)
        {
            return ConvertData<string, From>(obj);
        }

        private To ConvertData<To>(string obj)
        {
            return ConvertData<To, string>(obj);
        }

        public static bool ItGoodConstructorLight(ConstructorInfo C)
        {
            ParameterInfo[] ps = C.GetParameters();
            var cs = from p in ps
                     let prms = p.ParameterType.GetConstructor
                     (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null)
                     let pms = p.ParameterType.IsValueType
                     where ((prms != null) || pms)
                     select p;
            return (cs.Count() == ps.Length);
        }

        public static object ObjectFromConstructor(ConstructorInfo C)
        {
            ParameterInfo[] ps = C.GetParameters();
            List<object> o = new List<object>();
            for (int i = 0; i < ps.Length; i++)
            {
                // if (ps[i].DefaultValue != null) o.Add(ps[i].DefaultValue); else 
                if (ps[i].ParameterType.IsValueType) o.Add(Activator.CreateInstance(ps[i].ParameterType));
                else
                    o.Add(ps[i].ParameterType.GetConstructor
                         (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null).Invoke(null));
            }
            return C.Invoke(o.ToArray());
        }

        public static bool ItIConvertableConstructor(ConstructorInfo C)
        {
            ParameterInfo[] ps = C.GetParameters();
            if ((ps.Length == 1) && (NewInstance.WithoutRef(ps[0].ParameterType).GetInterface("IConvertible") != null)) return true; else return false;
        }

        public static bool ItFromStringConstructor(ConstructorInfo C)
        {
            ParameterInfo[] ps = C.GetParameters();
            if ((ps.Length == 1) && (NewInstance.WithoutRef(ps[0].ParameterType).Name == "String")) return true; else return false;
        }

        public static ConstructorInfo GetIConvertableConstructor(Type type)
        {
            ConstructorInfo[] C = type.GetConstructors();
            return C.FirstOrDefault(a => ItIConvertableConstructor(a));
        }

        public static ConstructorInfo GetFromStringConstructor(Type type)
        {
            ConstructorInfo[] C = type.GetConstructors();
            return C.FirstOrDefault(a => ItFromStringConstructor(a));
        }

        public static ConstructorInfo GetDefaultConstructor(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            var constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor == null)
            {
                var ctors =
                    from ctor in type.GetConstructors()
                    let prms = ctor.GetParameters()
                    where prms.All(p => p.IsOptional)
                    orderby prms.Length
                    select ctor;
                constructor = ctors.FirstOrDefault();
            }
            return constructor;
        }


        /*   private static To ConvertData<To, From>(List<From> obj)
        {
            To t = default(To);
            Type C = typeof(From);
            MethodInfo M = C.GetMethod("ToString", Type.EmptyTypes);
            if (M != null)
            {
                StringBuilder S = new StringBuilder(100);
                for (int i = 0; i < obj.Count; i++)
                {
                    S.Append((string)M.Invoke(obj[i], null));
                }
                string s = S.ToString();
                t = ConvertData<To>(s); 
            }
            List<int> u=new List<int>();
           string vb= ConvertData(u);
            return t;
        }

        private static To ConvertData<To, From>(From[] obj)
        {
            List<From> L = obj.ToList<From>();
            return ConvertData<To, From>(L);
        }
         
        private static string ConvertData<From>(List<From> obj)
        {
            string t = string.Empty;
            Type C = typeof(From);
            MethodInfo M = C.GetMethod("ToString", Type.EmptyTypes);
            if (M != null)
            {
                StringBuilder S = new StringBuilder(100);
                for (int i = 0; i < obj.Count; i++)
                {
                    S.Append((string)M.Invoke(obj[i], null));
                }
                t = S.ToString();
            }
            return t;
        }
          
        private static string ConvertData<From>(IList<From> obj, string Delim = "")
        {
            string t = string.Empty;
            Type C = typeof(From);
            MethodInfo M = C.GetMethod("ToString", Type.EmptyTypes);
            if (M != null)
            {
                StringBuilder S = new StringBuilder(100);
                for (int i = 0; i < obj.Count; i++)
                {
                    S.Append((string)M.Invoke(obj[i], null)); S.Append(Delim);
                }
                t = S.ToString().Substring(0, S.Length - Delim.Length);
            }
            return t;
        }
         
        private static List<To> ConvertDatatoList<To, From>(From obj)
        {
            List<To> t = new List<To>();
            Type C = typeof(From);
            MethodInfo M = C.GetMethod("ToString", Type.EmptyTypes);
            if (M != null)
            {
                string S = (string)M.Invoke(obj, null);
                for (int i = 0; i < S.Length; i++)
                {
                    t.Add(ConvertData<To, char>(S[i]));
                }
            }
            return t;
        }
         
        private static List<To> ConvertDatatoList<To, From>(IList<From> obj)
        {
            List<To> t = new List<To>();
            for (int i = 0; i < obj.Count; i++)
            {
                t.Add(ConvertData<To, From>(obj[i]));
            }

            return t;
        }
        private static List<To> ConvertDatatoList<To, From>(From[] obj)
        {
            List<To> t = new List<To>();
            for (int i = 0; i < obj.Length; i++)
            {
                t.Add(ConvertData<To, From>(obj[i]));
            }
            return t;
        }

        public static To[] ConvertDatatoArray<To, From>(From obj)
        {
            List<To> t = ConvertDatatoList<To, From>(obj);
            return t.ToArray();
        }
          
        public static To[] ConvertDatatoArray<To, From>(IList<From> obj)
        {
            List<To> t = ConvertDatatoList<To, From>(obj);
            return t.ToArray();
        }

        public static To[] ConvertDatatoArray<To, From>(From[] obj)
        {
            List<To> t = ConvertDatatoList<To, From>(obj);
            return t.ToArray();
        }*/


    }


