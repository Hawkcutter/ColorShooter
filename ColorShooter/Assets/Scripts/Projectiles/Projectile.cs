using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour
{
    public ColorKey ColorKey { get; private set; }

    [SerializeField]
    protected GameObject projectileRoot;

    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }

    public bool IsPlayerBullet { get; private set; }

    protected int targetLayer;

    protected abstract void OnShotFired(Vector2 startPos, Vector2 direction);

    protected abstract void OnUpdate();
    protected abstract void OnFixedUpdate();
    protected abstract void OnShotDestroyed();

    protected abstract void OnHitEnter(Collider2D collider);
    protected abstract void OnHitUpdate(Collider2D collider);
    protected abstract void OnHitExit(Collider2D collider);

    void Start()
    {
        if (!projectileRoot)
        {
            projectileRoot = gameObject;
        }
    }

    public void Init(Vector2 startPos, Vector2 direction, ColorKey colorKey, bool isPlayerBullet)
    {
        IsPlayerBullet = isPlayerBullet;
        ColorKey = colorKey;
        OnShotFired(startPos, direction);

        if (isPlayerBullet)
            targetLayer = GameManager.Instance.GetEnemyLayer();

        else
            targetLayer = GameManager.Instance.GetPlayerLayer();
    }

    void Update()
    {
        OnUpdate();
    }

    void FixedUpdate()
    {
        OnFixedUpdate();
    }

    void OnDestroy()
    {
        OnShotDestroyed();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        OnHitEnter(collider);
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        OnHitUpdate(collider);
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        OnHitExit(collider);
    }
}
