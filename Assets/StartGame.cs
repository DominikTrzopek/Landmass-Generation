using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public Generator[] gen ;
    int biom;
    const int value = 1000000;
    public GameObject player;

    void Start()
    {
        
        biom = Random.Range(0, value);
        biom %= gen.GetLength(0);
        gen[biom].GenerateMap();
        Vector2[] playerco = PlayerSpawn.playerco;
        /*
        for (int i = 0; i < PlayerSpawn.player_number; i++)
        {
            Debug.Log(playerco[i]);
            Instantiate(player, new Vector3(playerco[i].x, 70, playerco[i].y), transform.rotation);
        }
        */

    }
    int currentnumber = 0;
    void Update()
    {        
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("bullet_inactive");
        int number = bullets.Length;
        if (currentnumber > number)
        {
            gen[biom].UpdateMap();
        }
        currentnumber = bullets.Length;
    }
    
}
