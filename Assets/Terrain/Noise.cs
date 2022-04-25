using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, float scale, float persistance, float lacunarity, int octaves, int seed, Vector2 offset  )
    {
        float[,] noiseMap = new float[width, height];
        System.Random prng = new System.Random(seed);
        Vector2[] octaveoffset = new Vector2[octaves];
        for(int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-10000, 10000) + offset.x;
            float offsetY = prng.Next(-10000, 10000) + offset.y;
            octaveoffset[i] = new Vector2(offsetX, offsetY);
        }
        if (scale == 0)
            scale = 0.0001f;
        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                float noiseHeight = 0;
                float amplitude = 1;
                float frequency = 1;
                for (int k = 0; k < octaves; k++)
                {

                    float sampleX = i / scale * frequency + octaveoffset[k].x;
                    float sampleY = j / scale * frequency + octaveoffset[k].y;
                    float perling = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perling * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                    //Debug.Log(amplitude);
                }
                if (noiseHeight > maxHeight)
                    maxHeight = noiseHeight;
                else if (noiseHeight < minHeight)
                    minHeight = noiseHeight;
                noiseMap[i, j] = noiseHeight;
            }
        }

        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                noiseMap[i, j] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[i, j]);
            }
        }
        return noiseMap;
    }
}
