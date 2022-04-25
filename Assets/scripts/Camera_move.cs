using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_move : MonoBehaviour
{
    const int rotationX = 45;
    public Camera cam;
    public float camera_speed = 20f;
    public Generator gen;
    public float scroll_speed = 20f;
    public float maxY = 120f, minY = 0f;
    public GameObject player, pivot;
    const float height = 40f;
    bool free_cam = true;
    Vector2 moveMulti; 
    float cameraRotation = 0;
    public float cameraRotationSpeed;
    void Update()
    {
        Vector2 panlimit = new Vector2(gen.mapChunkSize * 2f, gen.mapChunkSize * 2f);
        float border = Screen.height / 3f;
        Vector2 cursorPoz = new Vector2(Mathf.Abs(Input.mousePosition.x - Screen.width / 2f), Mathf.Abs(Input.mousePosition.y - Screen.height / 2f));
        moveMulti = ReturnMultiplayer(cursorPoz, border);
        if (Input.GetKeyDown(KeyCode.Space))
            free_cam = !free_cam;
        if (free_cam == true)
        {
            if (Input.GetMouseButton(2))
            {
                moveMulti = new Vector2(0, 0);
                transform.rotation = Quaternion.Euler(rotationX, cameraRotation, 0);
                if (Input.mousePosition.x - Screen.width / 2f > 0)
                    cameraRotation += cameraRotationSpeed;
                else if (Input.mousePosition.x - Screen.width / 2f < 0)
                    cameraRotation -= cameraRotationSpeed;
            }

            Vector3 pos = cam.transform.position;
            float speed = camera_speed * Time.deltaTime * transform.position.y / 50f;
            if (Input.mousePosition.y > Screen.height - border)
            {
                pos += pivot.transform.forward * speed * moveMulti.y;
            }
            if (Input.mousePosition.y < border)
            {
                pos -= pivot.transform.forward * speed * moveMulti.y;
            }
            if (Input.mousePosition.x > Screen.width - border)
            {
                pos += pivot.transform.right * speed * moveMulti.x;
            }
            if (Input.mousePosition.x < border)
            {
                pos -= pivot.transform.right * speed * moveMulti.x;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scroll_speed * Time.deltaTime * 100f;

            pos.x = Mathf.Clamp(pos.x, -panlimit.x, panlimit.x);
            pos.z = Mathf.Clamp(pos.z, -panlimit.y, panlimit.y);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;

        }
        Vector3 follow_pos = new Vector3(player.transform.position.x, player.transform.position.y + height, player.transform.position.z - 50f);
        if (free_cam == false)
        {
            transform.position = follow_pos;
            transform.rotation = Quaternion.Euler(rotationX, 0, 0);
        }
  

    }
    Vector2 ReturnMultiplayer(Vector2 cursorPoz, float border)
    {
        Vector2 maxValue = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 minValue = new Vector2(maxValue.x - border, maxValue.y - border);
        Vector2 calculate = (cursorPoz - minValue) / (maxValue - minValue);
        if (cursorPoz.x - minValue.x < 0)
            calculate.x = 0;
        if (cursorPoz.y - minValue.y < 0)
            calculate.y = 0;
        return calculate * 3;
    }
}
