using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubPlayerPrefs : MonoBehaviour // created by bat >:33333333333333
{
    // --------------- DEFAULT ---------------

    // -------------- SAVE ---------------
    public static void Save(string name, object value, char type = 'n') // save your variable, with "auto type getting"
    {
        if ((value is string && type == 'n') || type == 's') PlayerPrefs.SetString(name, value.ToString());
        else if ((value is float && type == 'n') || type == 'f') PlayerPrefs.SetFloat(name, (float)value);
        else if ((value is int && type == 'n') || type == 'i') PlayerPrefs.SetInt(name, (int)value);
        else if ((value is bool && type == 'n') || type == 'b') PlayerPrefs.SetInt(name, (bool)value ? 1 : 0);
    }
    public static void SaveVector(string name, Vector3 value)
    {
        PlayerPrefs.SetFloat(name + "_x", value.x);
        PlayerPrefs.SetFloat(name + "_y", value.y);
        PlayerPrefs.SetFloat(name + "_z", value.z);
    }

    // -------------- LOAD ---------------
    public static object Load(string name, Type type) // returning your variable
    {
        if (type == typeof(string)) return PlayerPrefs.GetString(name, string.Empty);
        if (type == typeof(int)) return PlayerPrefs.GetInt(name, int.MinValue);
        if (type == typeof(float)) return PlayerPrefs.GetFloat(name, float.MinValue);
        return null;
    }
    public static float LoadFloat(string name) { return PlayerPrefs.GetFloat(name, float.NaN); }
    public static int LoadInt(string name) { return PlayerPrefs.GetInt(name, int.MinValue); }
    public static string LoadString(string name) { return PlayerPrefs.GetString(name, string.Empty); }
    public static bool LoadBool(string name) { return PlayerPrefs.GetInt(name, int.MinValue) > 0; }
    public static Vector3 LoadVector(string name)
    {
        float x = PlayerPrefs.GetFloat(name + "_x", float.NaN);
        float y = PlayerPrefs.GetFloat(name + "_y", float.NaN);
        float z = PlayerPrefs.GetFloat(name + "_z", float.NaN);
        return new Vector3(x, y, z);
    }
}