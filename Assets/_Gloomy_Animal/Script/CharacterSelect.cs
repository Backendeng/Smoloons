using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterSelect : MonoBehaviour
{
    private int selection = 0;

    public List<GameObject> models = new List<GameObject>();

    private void Start()
    {
        foreach(GameObject go in models)
        {
            go.SetActive(false);
        }

        models[selection].SetActive(true);
        
    }

    /* 
     private void Update()
     {
         if (Input.GetKeyDown(KeyCode.W))
         {
             models[selection].SetActive(false);
             selection++;

             if (selection >= models.Count)
                 selection = 0;

             models[selection].SetActive(true);
         }

         if (Input.GetKeyDown(KeyCode.S))
         {
             models[selection].SetActive(false);
             selection--;

             if (selection < 0)
                 selection = models.Count - 1;

             models[selection].SetActive(true);
         }
         if (Input.GetKeyDown(KeyCode.Return))
         {
             PlayerPrefs.SetInt("PreferedModel", selection);
         }
         */
    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 350, 120, 40), "NEXT>"))
        {
            models[selection].SetActive(false);
            selection++;

            if (selection >= models.Count)
                selection = 0;

            models[selection].SetActive(true);
        }

        if (GUI.Button(new Rect(1100, 350, 120, 40), "<NEXT"))
        {
            models[selection].SetActive(false);
            selection--;

            if (selection < 0)
                selection = models.Count - 1;

            models[selection].SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayerPrefs.SetInt("PreferedModel", selection);
        }

    }
}
