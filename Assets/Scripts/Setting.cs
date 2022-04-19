using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public GameObject setting_panel; 
    public Transform backSlider;
    public Transform FXSlider;
    public Transform backSound;
    public Transform powerupSound;
    public Transform powerupSound1;
    public Transform powerupSound2;
    public Transform bombSound;
    public Transform LastStone;
    public GameObject globalKillInc;
    KillsIncrementer globalKi;
    // Start is called before the first frame update
    void Start()
    {
        globalKi = globalKillInc.GetComponent < KillsIncrementer > ();
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
        backSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
    }
    public void OnFXValueChanged() {
        float VolumeSliderGet = FXSlider.GetComponent <Slider> ().value;
        Debug.Log(powerupSound.GetComponent<AudioSource>().volume);
        powerupSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
        powerupSound1.GetComponent<AudioSource>().volume = VolumeSliderGet;
        powerupSound2.GetComponent<AudioSource>().volume = VolumeSliderGet;
        bombSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
        bombSound.GetChild(2).GetComponent<AudioSource>().volume = VolumeSliderGet;
        LastStone.GetComponent<AudioSource>().volume = VolumeSliderGet;
        for (int i = 0; i < globalKi.allPlayers.Length; i++) {
            globalKi.allPlayers[0].transform.GetComponent<AudioSource>().volume = VolumeSliderGet;
            globalKi.allPlayers[0].transform.GetChild(2).GetComponent<AudioSource>().volume = VolumeSliderGet;
        }
    }
}
