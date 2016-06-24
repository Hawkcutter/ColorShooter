using UnityEngine;
using System.Collections;

public class LinearProjectile : Projectile 
{
    [SerializeField]
    private float speed;
    private float lifetime;
    
    private Vector2 direction;
    public SpriteRenderer spriteRenderer;


    protected override void OnShotFired(Vector2 startPos, Vector2 direction)
    {
        this.lifetime = 5.0f;

        this.direction = direction;
        spriteRenderer = projectileRoot.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = ColorKey.RgbColor;
        Debug.Log(LayerMask.LayerToName(targetLayer));
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
       HitpointManager hitpointManager = collider.GetComponent<HitpointManager>();

       if (hitpointManager)
       {
           if (hitpointManager.gameObject.layer == targetLayer)
           {
               hitpointManager.Damage(this.Damage, this.ColorKey);
               Destroy(projectileRoot);
           }
       }
    }
    void Update()
    {
        this.lifetime -= Time.deltaTime;

        if (this.lifetime <= 0)
            Destroy(projectileRoot);
    }

    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(direction.x * Time.fixedDeltaTime * speed, direction.y * Time.fixedDeltaTime * speed, 0.0f);
    }


}
