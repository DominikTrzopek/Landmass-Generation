using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D DrawTextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.SetPixels(colorMap);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
    public static Texture2D DrawTextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        Texture2D texture = new Texture2D(width, height);
        Color[] mapcolour = new Color[width * height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapcolour[width * y + x] = Color.Lerp(Color.black, Color.white, heightMap[x, y]);
            }
        }
        return DrawTextureFromColorMap(mapcolour, width, height);
    }
}
