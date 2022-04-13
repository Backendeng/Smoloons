using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_animation : MonoBehaviour
{
    public Transform current_monkey_postion1;
    public static Transform current_monkey_postion;
    public Camera m_OrthographicCamera;
    public Vector3 target_camera_postion;
    public float speed = 10.0f;
    public float zoom_speed = 0.3f;
    public static bool zoom;
    // Start is called before the first frame update
    void Start()
    {
        zoom= false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (current_monkey_postion) 
        // {
            if (!zoom){
                if (m_OrthographicCamera.orthographicSize > 3)
                    m_OrthographicCamera.orthographicSize -= zoom_speed;
                float step =  speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, current_monkey_postion.position, step);
            } else {
                if (m_OrthographicCamera.orthographicSize < 9.2)
                    m_OrthographicCamera.orthographicSize += zoom_speed;
                float step =  speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target_camera_postion, step);
            }
        // }
    }
}
