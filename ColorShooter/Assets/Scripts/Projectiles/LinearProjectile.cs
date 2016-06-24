using UnityEngine;
using System.Collections;

public class LinearProjectile : Projectile 
{
    [SerializeField]
    private float speed;
    
    private Vector2 direction;


    protected override void OnShotFired(Vector2 startPos, Vector2 direction)
    {
        this.direction = direction;
    }

    protected override void OnHitUpdate(Collider2D collider)
    {
       
    }

    protected override void OnHitEnter(Collider2D collider)
    {
       HitpointManager hitpointManager = collider.GetComponent<HitpointManager>();

       if (hitpointManager)
       {
           if (hitpointManager.gameObject.layer == targetLayer)
           {
               hitpointManager.Damage(this.Damage);
               Destroy(projectileRoot);
           }
       }
    }

    protected override void OnHitExit(Collider2D collider)
    {
        
    }

    protected override void OnShotDestroyed()
    {
        
    }

    protected override void OnFixedUpdate()
    {
        gameObject.transform.position += new Vector3(direction.x * Time.fixedDeltaTime * speed, direction.y * Time.fixedDeltaTime * speed, 0.0f);
    }

    protected override void OnUpdate()
    {
        
    }
}
