using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool IsPlayerWeapon = true;
    public bool UseCooldown = true;

    [SerializeField]
    private GameObject spawnPosition;

    private float curCooldown;
    [SerializeField]
    private float cooldown;

    public bool IsReady { get { return curCooldown <= 0.0f; } }

    protected abstract void Shoot(Vector2 startPos, Vector2 direction, ColorKey color);

    void Start()
    {
        if (!spawnPosition)
            spawnPosition = gameObject;
    }

    public bool TryShoot(Vector2 direction, ColorKey color)
    {
        if (IsReady || !UseCooldown)
        {
            Shoot(spawnPosition.transform.position, direction, color);
            curCooldown = cooldown;
            return true;
        }

        return false;
    }

    void Update()
    {
        if(!IsReady)
            curCooldown -= Time.deltaTime;
    }

    protected Projectile InstantiateProjectile(Projectile prefab)
    {
        GameObject newObj = Instantiate(prefab.gameObject);
        return newObj.GetComponent<Projectile>();
    }
}
