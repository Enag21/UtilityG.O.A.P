using System;
using System.Collections.Generic;
using UGOAP.CommonUtils;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.Blackboard;

public partial class Blackboard<T> : Singleton<Blackboard<T>>
{
    public event Action<T> ObjectDeregistered;
    public event Action<T> ObjectRegistered;
    public Dictionary<FastName, T> Objects { get; private set; } = new Dictionary<FastName, T>();

    public T GetObject(FastName id) => Objects[id];

    public void RegisterObject(FastName id, T obj)
    {
        Objects.Add(id, obj);
        ObjectRegistered?.Invoke(obj);
    }

    public void UnregisterObject(FastName id)
    {
        ObjectDeregistered?.Invoke(Objects[id]);
        Objects.Remove(id);
    }

    public TGeneric GetGenericObject<TGeneric>(FastName id)
        where TGeneric : T
    {
        if (Objects.TryGetValue(id, out T value))
        {
            if (value is TGeneric result)
            {
                return result;
            }
        }

        return default(TGeneric);
    }

    public void TryGetGenericObject<TGeneric>(out TGeneric result)
        where TGeneric : T
    {
        foreach (var (_, obj) in Objects)
        {
            if (obj is TGeneric genericObj)
            {
                result = genericObj;
                return;
            }
        }
        result = default(TGeneric);
    }
}