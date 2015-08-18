using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
namespace BackRoom
{
    public class TMapper<T> : Singleton<T> where T : class
    {
        protected TMapper(){  }
        private static Dictionary<string, Dictionary<object, SortedList<string, object>>> MapperList = new Dictionary<string, Dictionary<object, SortedList<string, object>>>();
        //private Dictionary<object, List<object>> Childs = new Dictionary<object, List<object>>();
        #region Work_Mapper  
        protected Dictionary<object, SortedList<string, object>> add_Mapper(string MapperName)
        {
            Dictionary<object, SortedList<string, object>> AddDic = new Dictionary<object, SortedList<string, object>>();
            MapperList.Add(MapperName, AddDic);
            return AddDic;
        }
        protected Dictionary<object, SortedList<string, object>> Get_Mapper(string MapperName)
        {
            Dictionary<object, SortedList<string, object>> Mapper;
            if (!MapperList.TryGetValue(MapperName, out Mapper)) Mapper = add_Mapper(MapperName);
            return Mapper;
        }
        protected bool Check_Mapper(string MapperName)
        {            
            return MapperList.ContainsKey(MapperName);
        }
        protected bool Delete_Mapper(string MapperName, bool DeleteifNullOnly = false)
        {
            bool Result = false;
            if (DeleteifNullOnly)
            {
                Dictionary<object, SortedList<string, object>> Mapper;
                if (MapperList.TryGetValue(MapperName, out Mapper))
                {
                    if (Mapper.Count == 0) Result = MapperList.Remove(MapperName);
                }
            }
            else 
            { 
                Result = MapperList.Remove(MapperName);
            }
            return Result;
        }
        #endregion

        #region Work_LinkertoObjects
        private SortedList<string, object> add_LinkertoObjects(Dictionary<object, SortedList<string, object>> Mapper, object MainObject)
        {
            SortedList<string, object> LinkertoObjects = new SortedList<string, object>();
            Mapper.Add(MainObject, LinkertoObjects);
            return LinkertoObjects;
        }
        protected SortedList<string, object> add_LinkertoObjects(string MapperName, object MainObject)
        {
            Dictionary<object, SortedList<string, object>> Mapper = Get_Mapper(MapperName);            
            return add_LinkertoObjects(Mapper, MainObject);
        }
        SortedList<string, object> Get_LinkertoObjects(string MapperName, object MainObject)
        {
            SortedList<string, object> LinkertoObjects=null;
            Dictionary<object, SortedList<string, object>> Mapper = Get_Mapper(MapperName);
            if (!((Mapper.Count > 0) && (Mapper.TryGetValue(MainObject, out LinkertoObjects)))) LinkertoObjects = add_LinkertoObjects(Mapper, MainObject);         
            return LinkertoObjects;
        }
        protected bool Check_LinkertoObjects(string MapperName, object MainObject)
        {
            bool Result = false;
            Dictionary<object, SortedList<string, object>> Mapper = null;
            if (MapperList.TryGetValue(MapperName, out Mapper))
            {
                 if ((Mapper.Count > 0) && Mapper.ContainsKey(MainObject)) Result = true;
            }
            return Result;
        }
        protected bool Delete_LinkertoObjects(string MapperName, object MainObject, bool DeleteifNullOnly = false, bool DeleteParentifNull = false)
        {
            bool Result = false;
            Dictionary<object, SortedList<string, object>> Mapper;
            if (MapperList.TryGetValue(MapperName, out Mapper))
            {

                if (DeleteifNullOnly)
                {
                    SortedList<string, object> LinkertoObjects = null;
                    if ((Mapper.Count > 0) && Mapper.TryGetValue(MainObject, out LinkertoObjects))
                    {
                        if (LinkertoObjects.Count == 0) Result = Mapper.Remove(MainObject);
                    }
                }
                else
                {
                    if (Mapper.Count > 0) Result = Mapper.Remove(MainObject);
                }
                if (DeleteParentifNull)
                {
                    if (Mapper.Count == 0) MapperList.Remove(MapperName);
                }
            }
            return Result;
        }           
        #endregion
        #region Work_LinkedObject
        private void add_LinkedtObject(SortedList<string, object> LinkertoObjects,string NameLinkedObject, object LinkedObject)
        {   
            LinkertoObjects.Add(NameLinkedObject, LinkedObject);
        }
        protected bool add_LinkedObject(string MapperName, object MainObject, string NameLinkedObject, object LinkedObject, bool Replace = false)
        {
            bool Result = true;
            SortedList<string, object> LinkertoObjects = Get_LinkertoObjects(MapperName, MainObject);
            if (!LinkertoObjects.ContainsKey(NameLinkedObject))
            {
                add_LinkedtObject(LinkertoObjects, NameLinkedObject, LinkedObject);
            }
            else if (Replace)
            {
                LinkertoObjects.Remove(NameLinkedObject);
                add_LinkedtObject(LinkertoObjects, NameLinkedObject, LinkedObject);
            }
            else
            {
                Result = false;
            }
            return Result;
        }
        protected object Get_LinkedObject(string MapperName, object MainObject, string NameLinkedObject, out bool Result) 
        {
            Result = false;   object LinkedObject = null;
            Dictionary<object, SortedList<string, object>> Mapper = null;
            if (MapperList.TryGetValue(MapperName, out Mapper))
            {
                SortedList<string, object> LinkertoObjects = null;
                if (Mapper.TryGetValue(MainObject, out LinkertoObjects))
                {
                    if (LinkertoObjects.TryGetValue(NameLinkedObject, out LinkedObject)) Result = true;
                }
            }
            return LinkedObject;
        }
        protected object Get_LinkedObject(string MapperName, object MainObject, string NameLinkedObject) 
        {
            bool Result;
            return Get_LinkedObject(MapperName, MainObject, NameLinkedObject, out Result);
        }
        protected bool Check_LinkedObject(string MapperName, object MainObject, string NameLinkedObject)
        {
            bool Result = false;
            Dictionary<object, SortedList<string, object>> Mapper = null;
            if (MapperList.TryGetValue(MapperName, out Mapper))
            {                
                SortedList<string, object> LinkertoObjects = null;
                if ((Mapper.Count > 0) && Mapper.TryGetValue(MainObject, out LinkertoObjects))
                {
                    if (LinkertoObjects.ContainsKey(NameLinkedObject)) Result = true;
                }
            }
            return Result;
        }
        protected bool Delete_LinkedObject(string MapperName, object MainObject, string NameLinkedObject, bool DeleteParentifNull = false)
        {
            bool Result = false;           
            Dictionary<object, SortedList<string, object>> Mapper;
            if (MapperList.TryGetValue(MapperName, out Mapper))
            {
                SortedList<string, object> LinkertoObjects = null;
                if ((Mapper.Count > 0) && Mapper.TryGetValue(MainObject, out LinkertoObjects))
                {
                    if (LinkertoObjects.Count > 0) Result = LinkertoObjects.Remove(NameLinkedObject);
                    if (DeleteParentifNull)
                    {
                        if (LinkertoObjects.Count == 0) Mapper.Remove(MainObject);
                        if (Mapper.Count == 0) MapperList.Remove(MapperName);
                    }
                }
            }
            return Result;
        }        
        #endregion

        public int GarbageCollector()
        {
            // MapperList.SelectMany(a => a.Value).Where(b => b.Value.Count==0).Select(b =>b.Key).ToList().
            int Result = 0;
            foreach (var MapperPair in MapperList)
            {
                var NullableMainObject = MapperPair.Value.Where(LinkertoObjectsPair => LinkertoObjectsPair.Value.Count == 0).Select(LinkertoObjectsPair => LinkertoObjectsPair.Key);
                Result = Result + NullableMainObject.Count();
                NullableMainObject.ToList().ForEach(MainObject => MapperPair.Value.Remove(MainObject));
            }
            var NullableMapperName = MapperList.Where(MapperPair => MapperPair.Value.Count == 0).Select(MapperPair => MapperPair.Key);
            Result = Result + NullableMapperName.Count();
            NullableMapperName.ToList().ForEach(MapperName => MapperList.Remove(MapperName));
            return Result;
        }

    }
 
}
