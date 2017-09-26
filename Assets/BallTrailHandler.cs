using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTrailHandler : MonoBehaviour
{
    public float ScaleDownsizeSpeed = 0.004f;

    private float _scale = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.localScale = new Vector3(_scale, _scale, _scale);
	    _scale -= ScaleDownsizeSpeed;
	    if (_scale < 0)
	    {
	        Destroy(gameObject);
	    }
	}
}
