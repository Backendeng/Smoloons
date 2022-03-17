using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RankCalc : MonoBehaviour {

    GameObject KI;
    KillsIncrementer KII;

    public int[] rankScore = new int[5];
    public int[] rS = new int[5];
    public string[] s = new string[5];
    public string[] fs = new string[5];
    public int[] d = new int[5];
    public string[] t = new string[5];
    PhotonView pv;

	// Use this for initialization
	void Start () {
        rankScore = new int[PhotonNetwork.countOfPlayers];
        rS = new int[PhotonNetwork.countOfPlayers];
        s = new string[PhotonNetwork.countOfPlayers];
        fs = new string[PhotonNetwork.countOfPlayers];
        d = new int[PhotonNetwork.countOfPlayers];
        t = new string[PhotonNetwork.countOfPlayers];
        KI = GameObject.FindGameObjectWithTag("Kills");
        KII = KI.GetComponent<KillsIncrementer>();

        for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
        {
            fs[i] = "0";
        }

        pv = GetComponent<PhotonView>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        pv.RPC("SetRank", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    private void SetRank() {
        for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
        {
            rankScore[i] = KII.eachPlayerScore[i];
        }

        for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
        {
            rS[i] = Int32.Parse(rankScore[i].ToString() + i);
        }

        for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
        {
            d[i] = rS[i];
        }

        Array.Sort(d);



        for (int i = PhotonNetwork.countOfPlayers-1; i >= 0; i--)
        {
            s[i] = (d[i]).ToString()+(PhotonNetwork.countOfPlayers-i).ToString();
        }

        for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
        {
            t[i] = s[i];
        }
        for (int i = 0; i < PhotonNetwork.countOfPlayers; i++)
        {
            if (t[i].Length>2)
                fs[Int32.Parse(t[i].Substring(t[i].Length - 2, 1))] = t[i].Substring(t[i].Length - 1);
            else
                fs[Int32.Parse(t[i].Substring(t[i].Length - 2, 1))] = "0";
        }



    }
}
