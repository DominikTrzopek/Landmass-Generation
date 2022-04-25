using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSpawn
{
    const int radious = 3;
    const int size = 201, meshScale = 7;
    const int spawnRange = 500;
    static float[,] heightmap = Generator.noiseMap3;
    public static int player_number = 4;
    public static Vector2 SpawnPosition()
    {
        bool isplain;
        Vector2 position;
        do
        {
            isplain = false;
            position.x = Random.Range(-spawnRange, spawnRange) / meshScale + size / 2;
            position.y = Random.Range(-spawnRange, spawnRange) / meshScale + size / 2;
            for (int i = (int)position.x - radious; i <= (int)position.x + radious; i++) 
            {
                for (int j = (int)position.y - radious; j <= (int)position.y + radious; j++)
                {
                    if (heightmap[i,j] < 0.4f || heightmap[i,j] > 0.8f)
                    {
                         isplain = true;
                         break;
                    }          
                }
            }
        } while (isplain);

        return position;
    }
    public static Vector2[] playerco = new Vector2[player_number];
    public static bool isok = false;
    public static float[,] SpawnMap()
    {   
        float[,] map = new float[size, size];
        for(int a = 0; a < player_number; a++)
        {         
            Vector2 spawn = SpawnPosition();
            playerco[a] = spawn;
            Debug.Log(playerco[a]);
            float info = heightmap[(int)spawn.x,(int)spawn.y];
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                
                    if (i < spawn.x + radious && i > spawn.x - radious)
                    {

                        if (j < spawn.y + radious && j > spawn.y - radious)
                            map[i, j] = info;
                    }
                    else
                        map[i, j] = 0f;
                }
            }
            
        }
        isok = true;
        return map;

    }
    

}
