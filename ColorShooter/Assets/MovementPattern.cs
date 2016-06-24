using UnityEngine;
using System.Collections;

public abstract class MovementPattern : MonoBehaviour {

    [SerializeField]
    protected GameObject RootObject;
	
    void Start()
    {
        if (RootObject == null)
            RootObject = gameObject;

        OnStart();
    }

    void Update()
    {
        OnUpdate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void OnFixedUpdate();
}
