/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player: Photon.MonoBehaviour {
	public static Player Instance;

	private Text bomb_label;
	private Text life_label;
	private Text kick_label;
	private Text explosion_label;
	private Text speed_label;

	public Transform selfSpawnTransform;

	public GlobalStateManager globalManager;

	public float moveSpeed = 5f;

	public ParticleSystem Explosion;

	public int bombs = 2;
	//Amount of bombs the player has left to drop, gets decreased as the player
	//drops a bomb, increases as an owned bomb explodes
	public bool canKick = false;
	public int lifes = 1;
	public int explosion_power = 2;
	public bool dead = false;
	public bool respawning = false;

	private fade_script fade;

	GameObject globalKillInc;
	KillsIncrementer globalKi;
	private PhotonView PhotonView;
	public float max_health, curr_health, health;
	public Text _playerHealth, playerKills, playerDeaths;
	public GameObject playerGameObject, target;
	public POWERUPS powerups;
	public string playername;

	public void update_label(POWERUPS powerup) {
		switch (powerup) {
		case POWERUPS.BOMB:
			bomb_label.text = bombs.ToString();
			break;
		// case POWERUPS.KICK:
		// 	if (canKick) {
		// 		kick_label.text = "1";
		// 	} else {
		// 		kick_label.text = "0";
		// 	}
		// 	break;
		// case POWERUPS.LIFE:
		// 	life_label.text = lifes.ToString();
		// 	break;
		case POWERUPS.POWER:
			explosion_label.text = explosion_power.ToString();
			break;
		case POWERUPS.SPEED:
			speed_label.text = moveSpeed.ToString();
			break;
		}
	}

	private void Awake() {

		globalKillInc = GameObject.FindGameObjectWithTag("Kills");
		globalKi = globalKillInc.GetComponent < KillsIncrementer > ();
		Instance = this;
		PhotonView = GetComponent < PhotonView > ();
		curr_health = max_health = 100;
		_playerHealth = GetComponentInChildren < Text > ();

		health = 10;
		target = GameObject.FindGameObjectWithTag("target");

	}

	IEnumerator respawn_wait() {
		yield
		return new WaitForSeconds(3);
		respawning = false;
	}

	IEnumerator gameover_wait() {

		yield
		return new WaitForSeconds(1f);
		show_gameover_panel();

	}

	// Use this for initialization
	void Start() {
		// init fader
		foreach(fade_script f in FindObjectsOfType < fade_script > ()) {
			if (f.tag == "fader") {
				continue;
			} else {
				fade = f;
			}
		}

		// init labels
		if (GetComponent < Player_Controller > ().isActiveAndEnabled) {
			foreach(Text t in FindObjectsOfType < Text > ()) {
				switch (t.tag) {
				case "Bomb":
					bomb_label = t;
					break;
				case "life":
					life_label = t;
					break;
				case "power":
					explosion_label = t;
					break;
				case "speed":
					speed_label = t;
					break;
				case "kick":
					kick_label = t;
					break;
				}
			}
		}
		//Cache the attached components for better performance and less typing
		playername = gameObject.name;
	}

	IEnumerator dmg_animation() {
		StartCoroutine(fade.FadeOnly(fade_script.FadeDirection.In));
		yield
		return new WaitForSeconds(1);
		StartCoroutine(fade.FadeOnly(fade_script.FadeDirection.Out));

		yield
		return new WaitForSeconds(1);
		StartCoroutine(fade.FadeOnly(fade_script.FadeDirection.In));

		yield
		return new WaitForSeconds(1);
		StartCoroutine(fade.FadeOnly(fade_script.FadeDirection.Out));

	}

	private void show_gameover_panel() {
		foreach(hide_on_start h in Resources.FindObjectsOfTypeAll < hide_on_start > ()) {

			if (h.tag == "gameover") {
				h.toggle();
				break;
			}
		}
		Destroy(gameObject);
	}

	private void FurtherRespawn() {

		gameObject.SetActive(true);
		gameObject.transform.position = selfSpawnTransform.position;

	}

	public void RPC_SpawnPlayer(Transform spawnPoint, string shape) {

		PhotonNetwork.Instantiate(Path.Combine("Prefabs", shape), spawnPoint.position, Quaternion.identity, 0);

	}

	// void OnTriggerEnter(Collider collider) {
	// 	if (collider.CompareTag("powerup")) {
			
	// 		Debug.Log(collider.gameObject.GetComponent < powerup_script > ().powerup);
	// 		if (PhotonView != null && PhotonView.isMine) {
	// 			update_label(collider.gameObject.GetComponent < powerup_script > ().powerup);
	// 			// PlayerManagement.Instance.ModifyHealth(PhotonView.owner, lifes);
	// 		}
	// 	}
	// }

	[PunRPC]
	private void increaseKills(int playerUID) {

		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		switch (playerUID % 5) {
		case 1:
			ki.eachPlayerKills[0]++;
			ki.eachPlayerScore[0] = ki.eachPlayerScore[0] + 25;
			break;
		case 2:
			ki.eachPlayerKills[1]++;
			ki.eachPlayerScore[1] = ki.eachPlayerScore[1] + 25;
			break;
		case 3:
			ki.eachPlayerKills[2]++;
			ki.eachPlayerScore[2] = ki.eachPlayerScore[2] + 25;
			break;
		case 4:
			ki.eachPlayerKills[3]++;
			ki.eachPlayerScore[3] = ki.eachPlayerScore[3] + 25;
			break;
		case 5:
			ki.eachPlayerKills[4]++;
			ki.eachPlayerScore[4] = ki.eachPlayerScore[4] + 25;
			break;
		case 0:
			ki.eachPlayerKills[5]++;
			ki.eachPlayerScore[5] = ki.eachPlayerScore[5] + 25;
			break;
		default:
			break;
		}
	}

	[PunRPC]
	private void setDeaths(int id) {

		GameObject KillsInc = GameObject.FindGameObjectWithTag("Kills");
		KillsIncrementer ki = KillsInc.GetComponent < KillsIncrementer > ();
		switch (id % 5) {
		case 1:
			ki.eachPlayerDeaths[0]++;
			break;
		case 2:
			ki.eachPlayerDeaths[1]++;
			break;
		case 3:
			ki.eachPlayerDeaths[2]++;
			break;
		case 4:
			ki.eachPlayerDeaths[3]++;
			break;
		case 5:
			ki.eachPlayerDeaths[4]++;
			break;
		case 0:
			ki.eachPlayerDeaths[5]++;
			break;
		default:
			break;
		}
	}
}