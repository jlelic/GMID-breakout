using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHandler : MonoBehaviour
{

    private float _textSpeed = 0.1f;

    void Update ()
	{
	    var pos = transform.position;

	    if (pos.y < 4)
	    {
	        pos.y += _textSpeed * Time.timeScale;
	    }

	    pos.z -= _textSpeed*Time.timeScale;

	    if (pos.z < -15)
	    {
	        Destroy(gameObject);
	    }

        transform.position = pos;
	}
}
