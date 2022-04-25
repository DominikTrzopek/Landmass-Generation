using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    const int meshScale = 7;
    public static int size = 201;
    public Vector2 collisionInfoCal;
    public float[,] map = new float[size, size];
    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.tag == "bullet_inactive")
        {
            Vector3 collisionInfo = collision.collider.transform.position;
            //Debug.Log(collisionInfo);
            collisionInfoCal = new Vector2(collisionInfo.x / meshScale + size / 2, -collisionInfo.z / meshScale + size / 2);
            Destroy(collision.collider.gameObject);
        }
    }

}

