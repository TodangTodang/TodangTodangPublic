
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class SOCheckUtil
{
    public static void RecheckSO<T>(List<T> RecheckList) where T : BaseData
    {
        for (int i = 0; i < RecheckList.Count; i++)
        {
            RecheckList[i].RecheckSO();
        }
    }
    
    public static void RecheckSO<T>(T[] RecheckList) where T : BaseData
    {
        for (int i = 0; i < RecheckList.Length; i++)
        {
            RecheckList[i].RecheckSO();
        }
    }
    
    public static void StringToSO<T>(List<string> RecheckList, List<T> AddList ) where T : ScriptableObject
    {
        for (int i = 0; i < RecheckList.Count; i++)
        {
            T data = DataManager.Instance.GetDefaultData<T>(RecheckList[i]);
            AddList.Add(data);
        }
    }

    public static void CheckNewDefaultData<T,D>(T[] Sos, List<D> Datas) where T : ScriptableObject where D: BaseData
    {

        for (int i = 0; Sos.Length > Datas.Count && i < Sos.Length ; ++i)
        {
            int idx = Datas.FindIndex(x => x._defaultData == Sos[i]);
            if (idx == -1)
            {
                Type d = typeof(D);

                ConstructorInfo? ctor = d.GetConstructor(new Type[] { typeof(T) });
                if (ctor != null)
                {
                    Datas.Add(ctor.Invoke(new object[] { Sos[i] }) as D);
                }
            }
        }
        
        // for (int i = 0; Sos.Length < Datas.Count && i < Datas.Count ; ++i)
        // {
        //     int idx = Array.FindIndex(Sos,x => x == Datas[i]._defaultData);
        //     if (idx == -1)
        //     {
        //         Datas.RemoveAt(i);
        //     }
        // }
    }

    public static void CheckNewDefaultData<T>(T[] Sos, List<T> Datas) where T : ScriptableObject
    {
        for (int i = 0; Sos.Length > Datas.Count && i < Sos.Length ; ++i)
        {
            int idx = Datas.FindIndex(x => x == Sos[i]);
            if (idx == -1)
            {
                Datas.Add(Sos[i]);
            }
        }
    }

    public static void CheckNewDefaultData<T, D>(DataManager dataManager, List<D> Datas)
        where T : ScriptableObject where D : BaseData
    {
        T[] Sos = dataManager.GetDefaultDataArray<T>();
        
        for (int i = 0; Sos.Length > Datas.Count && i < Sos.Length ; ++i)
        {
            int idx = Datas.FindIndex(x => x._defaultData == Sos[i]);
            if (idx == -1)
            {
                Type d = typeof(D);

                ConstructorInfo? ctor = d.GetConstructor(new Type[] { typeof(T) });
                if (ctor != null)
                {
                    Datas.Add(ctor.Invoke(new object[] { Sos[i] }) as D);
                }
            }
        }
    }
    
    public static void CheckNewDefaultData<T>(DataManager dataManager, List<T> Datas) where T : ScriptableObject
    {
        T[] Sos = dataManager.GetDefaultDataArray<T>();
     
        for (int i = 0; Sos.Length > Datas.Count && i < Sos.Length ; ++i)
        {
            int idx = Datas.FindIndex(x => x == Sos[i]);
            if (idx == -1)
            {
                Datas.Add(Sos[i]);
            }
        }
    }
}
