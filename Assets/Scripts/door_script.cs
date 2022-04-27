using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_script : MonoBehaviour {

	/*
	* Don't need this script.
	*/


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Destroy(){
		Destroy(gameObject);
	}
}
