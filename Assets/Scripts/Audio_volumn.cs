using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Audio_volumn : MonoBehaviour
{
    public GameObject slider;
    public GameObject powerupSound;
    public GameObject bombSound;
    public GameObject Monkey;
    public GameObject LastStone;
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
        powerupSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
        bombSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
        LastStone.GetComponent<AudioSource>().volume = VolumeSliderGet;
        Monkey.GetComponent<AudioSource>().volume = VolumeSliderGet;
        Monkey.transform.GetChild(2).GetComponent<AudioSource>().volume = VolumeSliderGet;
    }
}
