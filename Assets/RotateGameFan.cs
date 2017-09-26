using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameFan : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(transform.position, Vector3.forward, 0.7f * Time.timeScale);
    }
}
