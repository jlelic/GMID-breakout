using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampBlinking : MonoBehaviour
{
    private Coroutine _blinkingCoroutine;
    private Light _light;
    void Start()
    {
        _light = gameObject.GetComponentInChildren<Light>();
    }

    void OnCollisionEnter(Collision col)
    {
        var tag = col.gameObject.tag;
        if (tag == "Ball")
        {
            if (_blinkingCoroutine != null)
            {
                StopCoroutine(_blinkingCoroutine);
            }
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < 20; i++)
        {
            _light.enabled = false;
            yield return new WaitForSeconds(Random.Range(0.1f,0.5f));
            _light.enabled = true;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
        _blinkingCoroutine = null;
    }
}
