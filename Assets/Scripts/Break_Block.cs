using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Break_Block : MonoBehaviour
{

    public float delete_time = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        delete_time += Time.deltaTime;
        if (delete_time > 2)
            Destroy(gameObject);
    }
}
