using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSet : MonoBehaviour {
    [SerializeField]
    private Texture blockTexture;
	// Use this for initialization
	void Start () {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material.mainTexture = blockTexture;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
