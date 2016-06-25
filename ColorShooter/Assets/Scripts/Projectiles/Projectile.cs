using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public abstract class Projectile : MonoBehaviour, IUnique
{
    public int UniqueId { get; set; }

    public ColorKey ColorKey { get; private set; }

    [SerializeField]
    protected GameObject projectileRoot;
    public GameObject ProjectileRoot { get; protected set; }

    [SerializeField]
    private int damage;
    public int Damage { get { return damage; } }

    public bool IsPlayerBullet { get; private set; }

    protected int targetLayer;

    protected abstract void OnShotFired(Vector2 startPos, Vector2 direction);

    void Start()
    {
        GameManager.Instance.RegisterProjectile(this);
    }

    void OnDestroy()
    {
        GameManager.Instance.UnregisterProjectile(this);
    }

    public void Init(Vector2 startPos, Vector2 direction, float speed, ColorKey colorKey, bool isPlayerBullet)
    {
        
        Init(startPos, direction, colorKey, isPlayerBullet);
    }

    public void Init(Vector2 startPos, Vector2 direction, ColorKey colorKey, bool isPlayerBullet)
    {
        if (!projectileRoot)
        {
            projectileRoot = gameObject;
        }

        IsPlayerBullet = isPlayerBullet;
        ColorKey = colorKey;


        if (isPlayerBullet)
            targetLayer = GameManager.Instance.GetEnemyLayer();

        else
            targetLayer = GameManager.Instance.GetPlayerLayer();

        OnShotFired(startPos, direction);
    }

}
