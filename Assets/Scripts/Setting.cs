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
        backSlider.GetComponent <Slider> ().value = PlayerNetwork.Instance.background_music;
        FXSlider.GetComponent <Slider> ().value = PlayerNetwork.Instance.FX_music;
    }

    // Update is called once per frame
    void Update()
    {
        backSound.GetComponent<AudioSource>().volume = PlayerNetwork.Instance.background_music;
        powerupSound.GetComponent<AudioSource>().volume = PlayerNetwork.Instance.FX_music;
        powerupSound1.GetComponent<AudioSource>().volume = PlayerNetwork.Instance.FX_music;
        powerupSound2.GetComponent<AudioSource>().volume = PlayerNetwork.Instance.FX_music;
        bombSound.GetComponent<AudioSource>().volume = PlayerNetwork.Instance.FX_music;
        bombSound.GetChild(2).GetComponent<AudioSource>().volume = PlayerNetwork.Instance.FX_music;
        LastStone.GetComponent<AudioSource>().volume = PlayerNetwork.Instance.FX_music;
    }

    public void Appear() {
        setting_panel.SetActive(true);
    }

    public void close() {
        setting_panel.SetActive(false);
    }

    public void OnBackValueChanged() {
        PlayerNetwork.Instance.background_music = backSlider.GetComponent <Slider> ().value;
        // float VolumeSliderGet = backSlider.GetComponent <Slider> ().value;
        // backSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
    }
    public void OnFXValueChanged() {
        PlayerNetwork.Instance.FX_music = FXSlider.GetComponent <Slider> ().value;
        // float VolumeSliderGet = FXSlider.GetComponent <Slider> ().value;
        // powerupSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
        // powerupSound1.GetComponent<AudioSource>().volume = VolumeSliderGet;
        // powerupSound2.GetComponent<AudioSource>().volume = VolumeSliderGet;
        // bombSound.GetComponent<AudioSource>().volume = VolumeSliderGet;
        // bombSound.GetChild(2).GetComponent<AudioSource>().volume = VolumeSliderGet;
        // LastStone.GetComponent<AudioSource>().volume = VolumeSliderGet;
        // for (int i = 0; i < globalKi.allPlayers.Length; i++) {
        //     globalKi.allPlayers[0].transform.GetComponent<AudioSource>().volume = VolumeSliderGet;
        //     globalKi.allPlayers[0].transform.GetChild(2).GetComponent<AudioSource>().volume = VolumeSliderGet;
        // }
    }
}
