using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public static GameUI Instance;

    public Text playerHealth;
    public Text playerKills;
    public Text playerDeaths;
    public Text playerScore;
	// Use this for initialization
	private void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	private void Update () {
     //   playerHealth.text = PlayerMovement.Instance.Health.ToString();
	}
}
