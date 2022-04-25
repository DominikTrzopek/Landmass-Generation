using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FallOffGen 
{
    public static float[,] FallOff(int size)
    {
        float[,] map = new float[size, size];
        for (int i = 0; i < size; i++) 
        {
            for (int j = 0; j < size; j++) 
            {
                float valueI = i / (float)size * 2 - 1;
                float valueJ = j / (float)size * 2 - 1;
                float value = Mathf.Max(Mathf.Abs(valueI), Mathf.Abs(valueJ));
                map[i, j] = Evaluate(value);
            }
        }
        return map;
    }

    static float Evaluate(float x)
    {
        float a = 3;
        float b = 4f;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + (Mathf.Pow(b - b * x, a)));
    }
}
