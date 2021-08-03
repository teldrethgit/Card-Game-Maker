using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Reference: https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity/56146280#56146280
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Result;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Result = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Result = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    private class Wrapper<T>
    {
        public T[] Result;
    }
}