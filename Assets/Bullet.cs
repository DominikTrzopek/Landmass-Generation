using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    const int border = -30;

    private void Update()
    {
        if (transform.position.y < border)
        {
            Destroy(gameObject);
        }
        

    }
}

