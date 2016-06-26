using UnityEngine;
using System.Collections;

public class ChangeColor : EnemyAttack {

    [SerializeField]
    private float changeTime;
    private float timeToNextChange;

	// Use this for initialization
	void Start () {
        timeToNextChange = changeTime;
	}
	
	// Update is called once per frame
	void Update () {
        timeToNextChange -= Time.deltaTime;

        if (timeToNextChange <= 0.0f)
        {
            timeToNextChange = changeTime;

            this.enemy.ColorKey = ColorKey.GetRandomColorKey();
        }
	}
}
