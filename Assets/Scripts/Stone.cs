using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    private float time = .0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
 
        if(time > 1)
            {
                transform.GetChild(0).gameObject.tag = "Untagged";
                transform.GetChild(0).GetComponent<BoxCollider>().size = new Vector3(0.023f, 0.023f, 0.1f);
            }
    }


}
