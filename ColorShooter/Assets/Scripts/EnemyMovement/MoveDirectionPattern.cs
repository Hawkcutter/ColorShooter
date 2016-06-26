using UnityEngine;
using System.Collections;
using System;

public class MoveDirectionPattern : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Vector2 direction;


    void Start()
    {
        Enemy enemy = GetComponent<Enemy>();

        if (enemy)
            this.speed = enemy.Speed;
    }

    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(direction.x * speed * Time.fixedDeltaTime, direction.y * speed * Time.fixedDeltaTime, 0.0f);
    }

}
