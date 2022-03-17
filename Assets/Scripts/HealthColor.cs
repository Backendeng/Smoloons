using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthColor : MonoBehaviour {

    Color healthGreen, healthOrange, healthRed;

	// Use this for initialization
	void Start () {
        healthGreen = new Color32(6, 255, 12,255);
        healthOrange = new Color32(255, 125, 19,255);
        healthRed = new Color32(255, 8, 21,255);
    }
	
	// Update is called once per frame
	void Update () {
        if (gameObject.GetComponent<Image>().fillAmount > 0.7f)
            gameObject.GetComponent<Image>().color = healthGreen;
        else if (gameObject.GetComponent<Image>().fillAmount > 0.3f && GetComponent<Image>().fillAmount < 0.7f)
            gameObject.GetComponent<Image>().color = healthOrange;
        else if (gameObject.GetComponent<Image>().fillAmount < 0.3f)
            gameObject.GetComponent<Image>().color = healthRed;
    }
}
