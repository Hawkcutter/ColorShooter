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

        Debug.Log(LayerMask.LayerToName(targetLayer));
    }


    void OnTriggerEnter2D(Collider2D collider)
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

    void FixedUpdate()
    {
        gameObject.transform.position += new Vector3(direction.x * Time.fixedDeltaTime * speed, direction.y * Time.fixedDeltaTime * speed, 0.0f);
    }


}
