using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public Generator gen ;
    void Start()
    { 
        gen.GenerateMap();
    }
    int currentnumber = 0;
    void Update()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("bullet_inactive");
        int number = bullets.Length;
        if (currentnumber > number)
        {
            gen.UdpateMap();
        }
        currentnumber = bullets.Length;
    }
}
