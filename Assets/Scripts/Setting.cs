using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject setting_panel; 
    public Transform backSound;
    public Transform backSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Appear() {
        setting_panel.SetActive(true);
    }

    public void close() {
        setting_panel.SetActive(false);
    }

    public void OnBackValueChanged() {
        float VolumeSliderGet = backSlider.GetComponent <Slider> ().value;
        Debug.Log(VolumeSliderGet);
        backSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
    }
}
