using UnityEngine;
using System.Collections;
using System;

public class MoveDownPattern : MovementPattern
{
    [SerializeField]
    private float Speed;

    protected override void OnStart()
    {

    }

    protected override void OnUpdate()
    {

    }

    protected override void OnFixedUpdate()
    {
        RootObject.transform.position += new Vector3(0.0f, -Speed * Time.fixedDeltaTime, 0.0f);
    }

}
