using UnityEngine;
using System.Collections;

public class SinusPattern : MonoBehaviour {

    [SerializeField]
    private float speed;

    [SerializeField]
    private float sinusSpeedUp;

    [SerializeField]
    private float amplitude;

    [SerializeField]
    private Vector2 axis;
    private float time;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        time += Time.fixedDeltaTime;

        float sin = Mathf.Sin(time * sinusSpeedUp) * amplitude;

        float x = axis.x * speed * sin * Time.fixedDeltaTime;
        float y = axis.y * speed * sin * Time.fixedDeltaTime;

        gameObject.transform.position += new Vector3(x, y, 0);
	}
}
