using UnityEngine;
using System.Collections;

public class RemoveAfterTime : MonoBehaviour {

    
    public float LifeTime;

    public bool IsRunning;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (IsRunning)
        {
            LifeTime -= Time.deltaTime;

            if (LifeTime <= 0.0f)
                Destroy(gameObject);
        }
	}
}
