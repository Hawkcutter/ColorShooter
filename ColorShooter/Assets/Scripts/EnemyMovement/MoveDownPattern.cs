using UnityEngine;
using System.Collections;
using System;

public class MoveDownPattern : MonoBehaviour
{
    [SerializeField]
    private float Speed;


    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(0.0f, -Speed * Time.fixedDeltaTime, 0.0f);
    }

}
