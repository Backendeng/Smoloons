using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{

    public Image creator;
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
    }

    void FadeOut()
    {
        creator.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}