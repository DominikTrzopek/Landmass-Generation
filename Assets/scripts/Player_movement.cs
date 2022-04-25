using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public Rigidbody player;
    public int speed, bullet_force;
    public int tank_rotation, turret_rotation;
    Vector3 direction, rotation_axis, turret_rotation_axis;
    public GameObject tank_pivot, cannon, turret_pivot, bullet, cannon_pivot, turret, turret_rotation_pivot;
    public bool ifCollide = false;

    public float maxspeed;

    void OnCollisionEnter()
    {
        ifCollide = true;
    }
    void OnCollisionExit()
    {
        ifCollide = false;
    }

    void FixedUpdate()
    {
        float currentspeed = Mathf.Sqrt(Mathf.Pow(player.velocity.x, 2) + Mathf.Pow(player.velocity.z, 2));
        rotation_axis = turret_pivot.transform.position - cannon.transform.position;
        direction = (tank_pivot.transform.position - gameObject.transform.position).normalized;
        turret_rotation_axis = turret_rotation_pivot.transform.position - turret.transform.position;
        //Debug.Log(currentspeed);
        if (ifCollide == true)
        {
            if (Input.GetKey("w") && currentspeed < maxspeed - player.velocity.y * .5f)
            {
                player.AddForce(direction * speed);
            } 
            if (Input.GetKey("s") && currentspeed < maxspeed - player.velocity.y * .5f)
            {
                player.AddForce(-direction * speed);
            }
            if (Input.GetKey("a"))
            {
                player.transform.RotateAround(transform.position, -Vector3.up, tank_rotation);
            }
            if (Input.GetKey("d"))
            {
                player.transform.RotateAround(transform.position, Vector3.up, tank_rotation);
            }

        }
        if (Input.GetKey("left"))
        {
            turret.transform.RotateAround(turret.transform.position, -turret_rotation_axis, turret_rotation);
        }
        if (Input.GetKey("right"))
        {
            turret.transform.RotateAround(turret.transform.position, turret_rotation_axis, turret_rotation);
        }
        if (Input.GetKey("up"))
        {
            cannon.transform.RotateAround(cannon.transform.position, -rotation_axis, tank_rotation);
        }
        if (Input.GetKey("down"))
        {
            cannon.transform.RotateAround(cannon.transform.position, rotation_axis, tank_rotation);
        }


        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, cannon_pivot.transform.position, cannon_pivot.transform.rotation);
            //player.GetComponent<Player_movement>().enabled = false;
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("bullet_active");
        Vector3 bullet_direction = (cannon_pivot.transform.position - cannon.transform.position).normalized;
        foreach (GameObject obj in bullets)
        {
            obj.GetComponent<Rigidbody>().AddForce(bullet_direction * bullet_force, ForceMode.Impulse);
            obj.tag = "bullet_inactive";
        }
        
    }
}
