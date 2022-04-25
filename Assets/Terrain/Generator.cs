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
        FollOffMap,
        DestructionMap
    };
    public DrawMode drawMode;
    public bool flatshading;
    public float perling = 0.5f;
    public float persistance = 0.5f, lacunarity = 0.5f;
    public int octaves = 4;
    public Vector2 offset = new Vector2(1, 1);
    public TerrainType[] Terrain;
    public float meshHeight;
    public AnimationCurve animationCurve;
    public int mapChunkSize = 201;
    [Range(0, 6)]
    public int LOD = 0;
    public bool useFallOff, useDestruction;
    float[,] falloff, mapdestruction;
    public Destruction destructionClass;
    public Vector2 destructionCoordinates;
    const int value = 1000000;
    float[,] noiseMap2;
    public int seed;
    public bool ifstarted = true;
    public static float[,] noiseMap3;
    void Start()
    {
        noiseMap2 = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, perling, persistance, lacunarity, octaves, seed, offset);
        noiseMap3 = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, perling, persistance, lacunarity, octaves, seed, offset);
        float[,] spawnmap = PlayerSpawn.SpawnMap();
        if (useFallOff)
        {
            for (int y = 0; y < mapChunkSize; y++)
            {
                for (int x = 0; x < mapChunkSize; x++)
                {
                    
                    noiseMap2[x, y] = Mathf.Clamp01(noiseMap2[x, y] - falloff[x, y]);
                    noiseMap3[x, y] = noiseMap2[x, y];
                    if (spawnmap[x, y] != 0)
                        noiseMap2[x, y] = spawnmap[x, y];
                }
            }
        }
    }
    void Awake()
    {
         seed = Random.Range(-value, value);
         //destructionCoordinates = destructionClass.collisionInfoCal;
         falloff = FallOffGen.FallOff(mapChunkSize);
        //mapdestruction = CreateDesMap.DestructionMap(destructionCoordinates);
    }
    public void GenerateMap()
    {
       float[,] spawnmap = PlayerSpawn.SpawnMap();
       float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, perling, persistance, lacunarity, octaves, seed, offset);
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                if (useFallOff)
                    noiseMap[x, y] = Mathf.Clamp01(noiseMap[x, y] - falloff[x, y]);
                if (spawnmap[x, y] != 0)
                {
                    noiseMap[x, y] = spawnmap[x, y];

                }
                float currentHeight = noiseMap[x, y];
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
        if (drawMode == DrawMode.NoiseMap)
            draw.DrawMap(TextureGenerator.DrawTextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.colorMap)
            draw.DrawMap(TextureGenerator.DrawTextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.mesh)
        {
            draw.DrawMesh(MeshGen.GenerateTerrain(noiseMap, meshHeight, animationCurve, LOD, flatshading), TextureGenerator.DrawTextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.FollOffMap)
        {
            draw.DrawMap(TextureGenerator.DrawTextureFromHeightMap(FallOffGen.FallOff(mapChunkSize)));
        }
    }

    public void UpdateMap()
    {
        destructionCoordinates = destructionClass.collisionInfoCal;
        mapdestruction = CreateDesMap.DestructionMap(destructionCoordinates);

        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {

                if (useDestruction)
                {
                    noiseMap2[x, y] = Mathf.Clamp01(noiseMap2[x, y] - mapdestruction[x, y]);
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
        if (drawMode == DrawMode.mesh)
            draw.DrawMesh(MeshGen.GenerateTerrain(noiseMap2, meshHeight, animationCurve, LOD, flatshading), TextureGenerator.DrawTextureFromColorMap(colorMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.DestructionMap)
            draw.DrawMap(TextureGenerator.DrawTextureFromHeightMap(mapdestruction));
    }

    void OnValidate()
    {
        falloff = FallOffGen.FallOff(mapChunkSize);
        mapdestruction = CreateDesMap.DestructionMap(destructionClass.collisionInfoCal);
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float regionHeight;
    public Color regionColour;
}