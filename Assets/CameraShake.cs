using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShakeCamera(float shakeAmount)
    {
        StartCoroutine(ShakeCameraCoroutine(shakeAmount));
    }

    public IEnumerator ShakeCameraCoroutine(float shakeAmount)
    {
        var camTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        var originalPos = camTransform.localPosition;
        var shakeDuration = 10;
        while (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
            shakeDuration--;
            yield return new WaitForEndOfFrame();
        }
        camTransform.localPosition = originalPos;
    }
}
