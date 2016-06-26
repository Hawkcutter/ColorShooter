using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    [SerializeField]
    private float minRotationSpeed;
    [SerializeField]
    private float maxRotationSpeed;

    private float rotationSpeed;

	// Use this for initialization
	void Start () 
    {
        rotationSpeed = minRotationSpeed + Random.value * (maxRotationSpeed - minRotationSpeed);

        if (Random.value <= 0.5f)
            rotationSpeed *= -1;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        gameObject.transform.Rotate(new Vector3(0.0f, 0.0f, rotationSpeed * Time.fixedDeltaTime));
	}
}
