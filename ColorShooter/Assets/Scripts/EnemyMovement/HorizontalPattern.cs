using UnityEngine;
using System.Collections;

public class HorizontalPattern : MonoBehaviour 
{

    [SerializeField]
    private float speed;

    private float toRight = 1.0f;

    private float xMin;
    private float xMax;

    void Start()
    {
        if (Random.value < 0.5f)
            toRight = -toRight;

        xMin = GameManager.Instance.LevelBoundsMin.x;
        xMax = GameManager.Instance.LevelBoundsMax.x;

        Enemy enemy = GetComponent<Enemy>();

        if (enemy)
            this.speed = enemy.Speed;
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        gameObject.transform.position += new Vector3(toRight * speed * Time.fixedDeltaTime, 0.0f, 0.0f);

        if (gameObject.transform.position.x > xMax || gameObject.transform.position.x < xMin)
        {
            toRight = -toRight;
        }

	}
}
