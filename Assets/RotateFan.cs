﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFan : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		transform.RotateAround(Vector3.zero, Vector3.up, 2*Time.timeScale);
	}
}
