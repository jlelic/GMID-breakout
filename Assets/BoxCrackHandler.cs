using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCrackHandler : MonoBehaviour
{

    public Texture2D[] crackTextures;

    private MeshRenderer _meshRenderer;

	// Use this for initialization
	void Start ()
	{
	    _meshRenderer = GetComponent<MeshRenderer>();
	}

    public void ClearCrack()
    {
        _meshRenderer.enabled = false;
    }

    public void SetCrack()
    {
        _meshRenderer.enabled = true;
    }
}
