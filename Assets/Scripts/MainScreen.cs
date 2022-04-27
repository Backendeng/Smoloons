using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{

    // loading page animation 
    
    public Image creator;
    public Image leaf1;
    public Image leaf2;
    public Image leaf3;
    public Image leaf4;
    public Image leaf5;
    public Image leaf6;
    public Image leaf7;
    public Image leaf8;
    public Image leaf9;
    // Use this for initialization
    IEnumerator Start()
    {
        //creator.canvasRenderer.SetAlpha(0.0f);

        //FadeIn();
        yield return new WaitForSeconds(2.5f);
        FadeOut();
        yield return new WaitForSeconds(2.5f);
        PhotonNetwork.LoadLevel(1);
    }

    // Update is called once per frame
    void FadeIn()
    {
        creator.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf1.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf2.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf3.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf4.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf5.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf6.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf7.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf8.CrossFadeAlpha(1.0f, 1.5f, false);
        leaf9.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOut()
    {
        creator.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf1.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf2.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf3.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf4.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf5.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf6.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf7.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf8.CrossFadeAlpha(0.0f, 1.5f, false);
        leaf9.CrossFadeAlpha(0.0f, 1.5f, false);
    }
}