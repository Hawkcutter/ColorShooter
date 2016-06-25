using UnityEngine;
using System.Collections;

public class BackgroundScrolling : MonoBehaviour {

    public Material material;

    public Vector2 scrollingDir;
    public float scrollSpeed;

	// Update is called once per frame
	void Update () 
    {
        material.mainTextureOffset += scrollingDir * scrollSpeed * Time.deltaTime;
	}
}
