using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_animation : MonoBehaviour
{

    public static Transform current_monkey_postion;
    public Vector3 target_camera_postion;
    public float speed = 5.0f;
    public bool zoom;
    // Start is called before the first frame update
    void Start()
    {
        zoom= false;
    }

    // Update is called once per frame
    void Update()
    {
        if (current_monkey_postion) 
        {
            if (!zoom){
                float step =  speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, current_monkey_postion.position, step);
            }
        }
    }
}
