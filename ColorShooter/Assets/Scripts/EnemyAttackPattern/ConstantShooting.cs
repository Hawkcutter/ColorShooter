using UnityEngine;
using System.Collections;

public class ConstantShooting : EnemyAttack 
{
    [SerializeField]
    private Weapon weapon;



    void Start()
    {
        weapon.RandomizeCooldown();
    }


	void Update () 
    {
        weapon.TryShoot(new Vector2(0.0f, -1.0f), enemy.ColorKey);
	}
}
