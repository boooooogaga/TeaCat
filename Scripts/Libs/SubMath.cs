using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMath : MonoBehaviour // created by bat >:33333333333333
{
    // ----------------------------------------------------------------- CLAMPS ----------------------------------------------------------------- 

    public static float fastClamp(float value, float scale) // fast
    {
        if (scale < 0) scale *= -1;
        return Mathf.Clamp(value, -scale, scale);
    }
    public static float clampFromZero(float value, float max)
    {
        return Mathf.Clamp(value, 0, max);
    }

    // --------------- COMMON VECTOR2 ---------------
    public static Vector2 vector2Clamp(Vector2 vec, float minX, float maxX, float minY, float maxY)
    {
        vec.x = Mathf.Clamp(vec.x, minX, maxX);
        vec.y = Mathf.Clamp(vec.y, minY, maxY);
        return vec;
    }

    public static Vector2 fastVector2Clamp(Vector2 vec, float scaleX, float scaleY) // fast
    {
        vec.x = fastClamp(vec.x, scaleX);
        vec.y = fastClamp(vec.y, scaleY);
        return vec;
    }

    public static float maxInVector2(Vector2 vec)
    {
        return Mathf.Max(vec.x, vec.y);
    }

    // --------------- SAME VECTOR2 ---------------

    public static Vector2 sqVector2Clamp(Vector2 vec, float min, float max) // samenes in x and y
    {
        vec.x = Mathf.Clamp(vec.x, min, max);
        vec.y = Mathf.Clamp(vec.y, min, max);
        return vec;
    }

    public static Vector2 fastSmVector2Clamp(Vector2 vec, float scale) // fast || samenes in x and y
    {
        vec.x = fastClamp(vec.x, scale);
        vec.y = fastClamp(vec.y, scale);
        return vec;
    }

    // --------------- COMMON VECTOR3 ---------------

    public static Vector3 vectorClamp(Vector3 vec, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        vec.x = Mathf.Clamp(vec.x, minX, maxX);
        vec.y = Mathf.Clamp(vec.y, minY, maxY);
        vec.z = Mathf.Clamp(vec.z, minZ, maxZ);
        return vec;
    }

    public static Vector3 fastVectorClamp(Vector3 vec, float scaleX, float scaleY, float scaleZ) // fast
    {
        vec.x = fastClamp(vec.x, scaleX);
        vec.y = fastClamp(vec.y, scaleY);
        vec.z = fastClamp(vec.z, scaleZ);
        return vec;
    }

    public static float maxInVector(Vector3 vec)
    {
        return Mathf.Max(vec.x, vec.y, vec.z);
    }

    // --------------- SAME VECTOR2 ---------------

    public static Vector3 smVectorClamp(Vector3 vec, float min, float max) // samenes in x, y and z
    {
        vec.x = Mathf.Clamp(vec.x, min, max);
        vec.y = Mathf.Clamp(vec.y, min, max);
        vec.z = Mathf.Clamp(vec.z, min, max);
        return vec;
    }

    public static Vector3 fastSmVectorClamp(Vector3 vec, float scale) // fast || samenes in x, y and z
    {
        vec.x = fastClamp(vec.x, scale);
        vec.y = fastClamp(vec.y, scale);
        vec.z = fastClamp(vec.z, scale);
        return vec;
    }
}
