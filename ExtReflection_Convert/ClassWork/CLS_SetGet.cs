using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BackRoom;
using System.Collections.ObjectModel;
using ExtReflection_Convert;

    public interface IGetSet
    {


    }
    public static class CLS_ExtGetSet
    {
        internal static CLS_GetSet GetSet(this IGetSet obj)
        {
            return CLS_GetSet.GetLens(obj);
        }
    }

    public class CLS_GetSet : Magnifier<CLS_GetSet, IGetSet>, IConvert
    {
        public CLS_GetSet()
        {

        }
        public void SetFieldValue(string[] Names, params object[] Objs)
        {
            for (int i = 0; i < Names.Length; i++)  this.SetFieldValue(Names[i], Objs[i]);

        }
       public int SetFieldValue<A>(string name, A obj)
        {

            if (this.Parent == null) return -1; //object is null
            Type type = this.Parent.GetType();
            FieldInfo info = type.GetField(name,ALL);
            if (info == null) return -2; //not found Field
            string T = info.FieldType.Name;
            object r = this.ClassConvert().ConvertData(obj, info.FieldType);
            try
            {
                info.SetValue(this.Parent, r);
            }
            catch (Exception e) { return -4; }//convert error 
          /*IConvertible convertible = info.GetValue(this.Parent) as IConvertible;
            try
            {
                if (convertible != null)
                {
                    info.SetValue(this.Parent, Convert.ChangeType(i.ToString(), info.FieldType));
                }
                else
                {
                    switch (T)
                    {
                        case "IntPtr": info.SetValue(this.Parent, new IntPtr(Convert.ToUInt32(i.ToString()))); break;
                        case "Char[]": info.SetValue(this.Parent, i.ToString().ToCharArray()); break;
                        case "Char": info.SetValue(this.Parent, i.ToString().ToCharArray()[0]); break;
                        case "StringBuilder": info.SetValue(this.Parent, new StringBuilder(i.ToString())); break;
                        case "UIntPtr": info.SetValue(this.Parent, new UIntPtr(Convert.ToUInt32(i.ToString()))); break;
                        default: return -3;  //don't found type
                    }
                }
            }
            catch (Exception e) { return -4; }//convert error    */
            return 0;
        }

        public static string GetListDataFromField<From>(From obj, string Delim = "_")
        {
            FieldInfo[] FS = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            StringBuilder s = new StringBuilder(100);
            foreach (FieldInfo FI in FS)
            {
                s.Append(FI.Name); s.Append(":"); s.Append(FI.GetValue(obj).ToString()); s.Append(Delim);
            }
            string S = s.ToString();
            return S.Substring(0, S.Length - Delim.Length);
        }
        private const BindingFlags ALL = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        public object GetFieldValue(string name)
        {
            if (this.Parent == null) return null;
            Type type = this.Parent.GetType();
            FieldInfo info = type.GetField(name, ALL);
            if (info == null) return null;
            object obj = null;
            obj = info.GetValue(this.Parent);
            return obj;
        }
        public To GetFieldValue<To>(string name)
        {
            object retval = GetFieldValue(name);
            if (retval == null) { return default(To); }
            return (To)this.ClassConvert().ConvertData(retval, typeof(To));
        }

        public object GetPropValue(string name)
        {
            if (this.Parent == null) return null;
            Type type = this.Parent.GetType();
            PropertyInfo info = type.GetProperty(name, ALL);
            if (info == null) return null;
            object obj = null;
            obj = info.GetValue(this.Parent, null);
            return obj;
        }
        public To GetPropValue<To>(string name)
        {
            object retval = GetPropValue(name);
            if (retval == null) { return default(To); }
            return (To)this.ClassConvert().ConvertData(retval, typeof(To));
        }
        #region Invoke
        public object InvokeFromString(MethodInfo M, object P, params string[] T)
        {
            List<Type> RealType = M.GetParameters().Select(X => X.ParameterType).ToList();
            List<object> L = new List<object>();
            for (int i = 0; i < RealType.Count; i++) L.Add(this.ClassConvert().ConvertData(T[i], RealType[i]));
            return M.Invoke(P, L.ToArray());
        }

        public MethodInfo GetMethod(string Cl, string M, params string[] T)
        {
            return GetMethod(Type.GetType(GetFullNameClass(Cl)), M, T);
        }

        public MethodInfo GetMethod(Type Cl, string M, params string[] T)
        {
            MethodInfo MI = null;
            string DataName = Cl.FullName + "." + M;
            string[] RealName = T.Clone() as string[];
            for (int i = 0; i < RealName.Length; i++) RealName[i] = GetFullNameClass(RealName[i]);
            //RealType=new Type[RealName.Length];  for (int i = 0; i < RealType.Length; i++) RealType[i]=Type.GetType(RealName[i]); 
            int DataParams = RealName.Aggregate((X, x) => X = X + x).GetHashCode();
            SortedDictionary<int, MethodInfo> HelperDict = null;
            Dictionary<string, string> GenericMap;
            if (!GenericMethodsMap.TryGetValue(DataName, out HelperDict))
            {
                HelperDict = new SortedDictionary<int, MethodInfo>();
                GenericMethodsMap.Add(DataName, HelperDict);
            }
            if (!HelperDict.TryGetValue(DataParams, out MI))
            {
                MI = GetOriginalMethod(Cl, M, out GenericMap, T);
                if (MI != null)
                {
                    MI = GetGenericMethod(MI, GenericMap.Select(d => d.Value).ToArray());
                    HelperDict.Add(DataParams, MI);
                }
            }
            return MI;
        }
        #endregion

        #region HelperForInvoke
        private SortedDictionary<string, SortedDictionary<int, MethodInfo>> GenericMethodsMap = new SortedDictionary<string, SortedDictionary<int, MethodInfo>>();
        private MethodInfo GetGenericMethod(MethodInfo M, params string[] T)
        {
            List<Type> L = new List<Type>();
            for (int i = 0; i < T.Length; i++) L.Add(Type.GetType(T[i]));
            return M.MakeGenericMethod(L.Cast<Type>().ToArray());
        }

        private bool Similar(MethodInfo method, params Type[] TypeType)
        {
            Dictionary<string, string> NullableD = null;
            return Similar(method, true, ref NullableD, TypeType.Select(x => x.ToString()).ToArray());
        }

        private static char[] Delim = { ',', '[', ']' };
        private static Func<string, string[]> SimpleTypes = (ParamName =>
            ParamName.Split(Delim, StringSplitOptions.RemoveEmptyEntries)//.Select(x=>x.Substring(0,dt(x)).Substring(x.LastIndexOf("[")+1)).ToArray();
        );

        private static Dictionary<string, string> CH = new Dictionary<string, string>() { { "SYSTEM.COLLECTIONS.GENERIC.", "" }, { "SYSTEM.", "" },
        {"`1[", "["},{"`2[", "["}};
        private bool Similar(MethodInfo method, bool Strict, ref Dictionary<string, string> GenericMap, params string[] TypeName)
        {
            Func<string, string> NoStrict = ParamName =>
            {
                string S = ParamName.ToUpper();
                foreach (var C in CH) S = S.Replace(C.Key, C.Value);
                // Replace("SYSTEM.COLLECTIONS.GENERIC.", "").Replace("SYSTEM.", "").Replace("`1[", "[").Replace("`2[", "[")
                return S;
            };
            Func<string, string> CheckStrict = ParamName => Strict ? ParamName : NoStrict(ParamName);
            string[] Name = method.GetParameters().Select(x => CheckStrict(x.ParameterType.ToString())).ToArray();
            int L = Name.Length; bool Result = false;
            if (L == TypeName.Length)
            {
                Dictionary<string, string> GenericName =
                    (Dictionary<string, string>)method.GetGenericArguments().Select(x => x.ToString()).ToDictionary(x => Strict ? x : x.ToUpper(), x => string.Empty);
                string[] typeName;
                if (Strict) typeName = TypeName; else typeName = TypeName.Select(x => NoStrict(x)).ToArray();

                Func<string, string, bool> SameText = ((NameType, NameGeneric) =>
                {
                    if (Strict) return (NameType == NameGeneric);
                    return (NameGeneric.Contains(NameType));
                });
                Func<string, string, bool> PossibleTranslateInGeneric = ((NameType, NameGeneric) =>
                {
                    if (!GenericName.ContainsKey(NameGeneric)) return false;
                    string TT = GenericName[NameGeneric];
                    if (TT == NameType) return true;
                    else if (TT == string.Empty) { GenericName[NameGeneric] = NameType; return true; }
                    else return false;
                });
                Func<string, string> DT3 = (NameGenericMod =>
                {
                    GenericName.Select(x => x.Key).ToList().ForEach(y =>
                    {
                        NameGenericMod = NameGenericMod.Replace(y + ",", GenericName[y] + ",");
                    });
                    return NameGenericMod;
                });
                Result = typeName.Select((t, i) => new
                {
                    ParamsType = SimpleTypes(t),
                    ParamsGen = SimpleTypes(Name[i])
                }).Where(t => (
                        (t.ParamsType.Length != t.ParamsGen.Length)
                        || !(t.ParamsType.Where((x, i) => (!(SameText(x, t.ParamsGen[i])
                        || PossibleTranslateInGeneric(x, t.ParamsGen[i])))).FirstOrDefault() == null)
                    //|| (t.s1 != DT3(t.s2))
                    )).FirstOrDefault() == null;
                if (Result)
                {
                    if (Strict)
                    {
                        GenericMap = GenericName;
                    }
                    else
                    {
                        GenericMap = method.GetGenericArguments().ToDictionary(x => x.ToString(), x => string.Empty);
                        for (int i = 0; i < GenericMap.Count; i++)
                        {
                            GenericMap[GenericMap.ElementAt(i).Key] = this.GetFullNameClass(GenericName[GenericName.ElementAt(i).Key]);
                        }
                    }
                }
            }
            return Result;
        }

        public string GetFullNameClass(string s, bool ItsUpperAuto = false, bool ItsUpper = true)
        {
            bool sUpper = ItsUpperAuto ? (!string.IsNullOrEmpty(s) && s.All(c => (char.IsUpper(c) || char.IsDigit(c) || (c == '.')))) : ItsUpper;
            if (sUpper)
            {
                s = s.ToUpper();
                Func<string, string, bool> Needed = (x, ss) => x.Contains(ss);
                int i = FullBigNameClasses.FindIndex(x => Needed(x, s));
                return i == -1 ? null : FullNameClasses[i];
            }
            return FullNameClasses.Find(x => x.Contains(s));
        }

        private static List<string> FullNameClasses = GetFullNameClasses();
        private static List<string> FullBigNameClasses = GetFullNameClasses(true);

        private static List<string> GetFullNameClasses(Boolean Big = false)
        {
            Func<Type[], IEnumerable<Type>> GoodTypes = TypeTypes => TypeTypes.Where(x => (
                x.IsPublic && !x.ContainsGenericParameters && !x.IsAbstract &&
                (x.IsPrimitive || x.IsClass) && ((x.BaseType == typeof(object)) || x.IsValueType)));
            Func<string, string> Needed = (s => Big ? s.ToUpper() : s);
            var L =
                GoodTypes(Assembly.GetExecutingAssembly().GetTypes()).Select(x => Needed(x.ToString())).ToList();
            L.AddRange(GoodTypes(Assembly.GetAssembly(typeof(System.Boolean)).GetTypes()).Select(x => Needed(x.ToString())));
            return L;
        }

        private MethodInfo GetOriginalMethod(Type T, string MethodName, out Dictionary<string, string> GenericMap, params string[] TypeName)
        {
            MethodInfo MI = GetOriginalMethod(T, MethodName, false, false, true, out GenericMap, TypeName);
            return MI;
        }

        private MethodInfo GetOriginalMethod(Type T, string MethodName, bool SearchOnlyInGeneric, bool Strict, bool UsePotential, out Dictionary<string, string> GenericMap, params string[] TypeName)
        {
            Func<string, string, bool> CheckCaseSensitive; Func<string, string, bool> Check; Func<MethodInfo, bool> CheckParams;

            string[] typeName; string methodName; MethodInfo[] methods; MethodInfo Result;
            if (SearchOnlyInGeneric)
            { methods = T.GetMethods().Where(S => S.ContainsGenericParameters).ToArray(); }
            else
            { methods = T.GetMethods(); }
            if (Strict)
            {
                methodName = MethodName; typeName = TypeName;
                CheckCaseSensitive = (ParamName, CheckName) => ParamName == CheckName;
                Check = (ParamName, CheckName) => ParamName != CheckName;
            }
            else
            {
                methodName = MethodName.ToUpper(); typeName = TypeName.Select(x => x.ToUpper()).ToArray();
                CheckCaseSensitive = (ParamName, CheckName) =>
                    ParamName.ToUpper() == CheckName;
                Check = (ParamName, CheckName) =>
                    !ParamName.ToUpper().Contains(CheckName);
            }
            Dictionary<string, string> GM = null;
            if (UsePotential)
            {
                CheckParams = M =>
                    Similar(M, Strict, ref GM, typeName);
            }
            else
            {
                CheckParams = M =>
                    M.GetParameters().Where((P, i) => Check(P.ParameterType.FullName, typeName[i])).FirstOrDefault() == null;
            }
            Result = methods.Select(M => new { Method = M, Prs = M.GetParameters() }).Where(M => (
                (CheckCaseSensitive(M.Method.Name, methodName))
                && (M.Prs.Length == typeName.Length)
                && CheckParams(M.Method)
                )).Select(M => M.Method).FirstOrDefault();
            GenericMap = GM;
            return Result;
        }
        #endregion

        #region Hash
        private static long GetHashCodeLong(string input)
        {
            var s1 = input.Substring(0, input.Length / 2);
            var s2 = input.Substring(input.Length / 2);
            var x = ((long)s1.GetHashCode()) << 0x20 | s2.GetHashCode();
            return x;
        }

        private static int MethodParamsFromIndex(long a)
        {
            int a1 = (int)(a & uint.MaxValue);
            int a2 = (int)(a >> 32);
            return (int)(a >> 32);
        }

        private static int MethodNameFromIndex(long a)
        {
            return (int)(a & uint.MaxValue);
        }

        private long GetOriginalMethodIndex(MethodInfo MI)
        {
            string ff = MI.DeclaringType.Name;
            string one = string.Join(null, MI.GetParameters().Select(Param => Param.ParameterType.Name));
            return MethodIndexFromNameParams(MI.Name.GetHashCode(), one.GetHashCode());
        }

        private long MethodIndexFromNameParams(int Name, int Params)
        {
            long b = Params;
            b = b << 0x20;
            b = b | (uint)Name;
            return b;
        }
        #endregion
    }

