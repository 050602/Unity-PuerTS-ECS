using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Log
{
    private static bool isDeBug = false;

    public static void setDeBug()
    {
        Log.isDeBug = true;
    }

    public static void gzaLog(params object[] array)
    {
        // return;
        Log.log(array);
    }
    public static void log(params object[] array)
    {
        if (!Log.isDeBug)
        {
            return;
        }

        string str = "";
        foreach (var item in array)
        {
            str += item + "   ";
        }
        Debug.Log(str);
    }
    public static void logWarning(params object[] array)
    {
        if (!Log.isDeBug)
        {
            return;
        }
        string str = "";
        foreach (var item in array)
        {
            str += item + "   ";
        }
        Debug.LogWarning(str);
    }
    public static void logError(params object[] array)
    {
        if (!Log.isDeBug)
        {
            return;
        }
        string str = "";
        foreach (var item in array)
        {
            str += item + "   ";
        }
        Debug.LogError(str);
    }
}