using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_volumn : MonoBehaviour
{
    public GameObject slider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        float VolumeSliderGet = slider.GetComponent <Slider> ().value;
        Debug.Log(VolumeSliderGet);
        gameObject.GetComponent<AudioSource>().volume = VolumeSliderGet;
    }
}
