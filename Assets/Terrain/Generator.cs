using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public enum DrawMode
    {
        colorMap,
        NoiseMap,
        mesh,
        FollOffMap
    };
    public DrawMode drawMode;

    public float perling = 0.5f;
    public float persistance = 0.5f, lacunarity = 0.5f;
    public int octaves = 4, seed;
    public Vector2 offset = new Vector2(1, 1);
    public TerrainType[] Terrain;
    public float meshHeight;
    public AnimationCurve animationCurve;
    public int mapChunkSize = 241;
    [Range(0, 6)]
    public int LOD = 0;
    public bool useFallOff, useDestruction;
    float[,] falloff, mapdestruction;
    public Destruction destructionClass;
    public Vector2 destructionCoordinates;

    float[,] noiseMap2;
    void Start()
    {
        noiseMap2 = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, perling, persistance, lacunarity, octaves, seed, offset);
    }
    void Awake()
    {
        //destructionCoordinates = destructionClass.collisionInfoCal;
        falloff = FallOffGen.FallOff(mapChunkSize);
        //mapdestruction = CreateDesMap.DestructionMap(destructionCoordinates);
    }
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, perling, persistance, lacunarity, octaves, seed, offset);
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for(int y = 0; y < mapChunkSize; y++)
        {
            for(int x = 0; x < mapChunkSize; x++)
            {
                if (useFallOff)
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloff[x, y]);      

                float currentHeight = noiseMap[x, y];
                for(int i = 0; i < Terrain.Length; i++)
                {
                    if (currentHeight <= Terrain[i].regionHeight)
                    {
                        colorMap[y * mapChunkSize + x] = Terrain[i].regionColour;
                        break;
                    }
                }
            }
        }
        Display draw = FindObjectOfType<Display>();
        if(drawMode == DrawMode.NoiseMap)
             draw.DrawMap(TextureGenerator.DrawTextureFromHeightMap(noiseMap));
        else if(drawMode == DrawMode.colorMap)
             draw.DrawMap(TextureGenerator.DrawTextureFromColorMap(colorMap,mapChunkSize,mapChunkSize));
        else if(drawMode == DrawMode.mesh)
        {
            draw.DrawMesh(MeshGen.GenerateTerrain(noiseMap,meshHeight, animationCurve, LOD),TextureGenerator.DrawTextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if(drawMode == DrawMode.FollOffMap)
        {
            draw.DrawMap(TextureGenerator.DrawTextureFromHeightMap(FallOffGen.FallOff(mapChunkSize)));
        }
    }

    public void UdpateMap()
    {
        destructionCoordinates = destructionClass.collisionInfoCal;
        mapdestruction = CreateDesMap.DestructionMap(destructionCoordinates);
     
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFallOff)
                    noiseMap2[x, y] = Mathf.Clamp01(noiseMap2[x, y] - falloff[x, y]);
                if (useDestruction)
                {
                    noiseMap2[x, y] = (noiseMap2[x, y] - mapdestruction[x, y]);
                }

                float currentHeight = noiseMap2[x, y];
                for (int i = 0; i < Terrain.Length; i++)
                {
                    if (currentHeight <= Terrain[i].regionHeight)
                    {
                        colorMap[y * mapChunkSize + x] = Terrain[i].regionColour;
                        break;
                    }
                }
            }
        }
        Display draw = FindObjectOfType<Display>();
            draw.DrawMesh(MeshGen.GenerateTerrain(noiseMap2, meshHeight, animationCurve, LOD), TextureGenerator.DrawTextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));

    }

    void OnValidate()
    {
        falloff = FallOffGen.FallOff(mapChunkSize);
        mapdestruction = CreateDesMap.DestructionMap(destructionCoordinates);
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float regionHeight;
    public Color regionColour;
}