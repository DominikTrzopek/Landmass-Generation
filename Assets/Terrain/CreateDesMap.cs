using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreateDesMap
{
    static int size = 241;
    const int radious = 3, maxradious = 10;
    public static float[,] DestructionMap(Vector2 collisionInfo)
    {
        float[,] map = new float[size, size];
        int collisionI = (int)collisionInfo.x;
        int collisionJ = (int)collisionInfo.y;
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {

                if (i != collisionI || j != collisionJ)
                    map[i, j] = 0;
                else if (i > maxradious && j > maxradious)
                    for (int y = j - maxradious; y <= j + maxradious; y++)
                    {
  
                        for (int x = i - maxradious; x <= i + maxradious; x++)
                        {
                            //x-row
                            //y-column-ver
                            if (Mathf.Pow(x - i, 2) + Mathf.Pow(y - j, 2) <= radious * radious)
                                map[x, y] = 0.1f;
                        }

                    }
            }
        }
        return map;
    }
}