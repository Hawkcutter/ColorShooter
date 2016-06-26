using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Gun : Weapon
{
    [SerializeField]
    private LinearProjectile projectilePrefab;

    public bool RotateProjectileToDirection = true;
    public bool ApplyStartPos = true;

    public enum GunUpgradeType { Damage, Speed, NumShots,             Count}

    private int[] upgradeCounts = new int[(int)GunUpgradeType.Count];


    /*
    [SerializeField]
    public int damageUpgradeCount = 0;
    [SerializeField]
    public int speedUpgradeCount = 0;
    [SerializeField]
    public int numShotUpgradeCount = 0;
    */
    private int numProjectiles = 1;

    [SerializeField]
    private float perShotDmgReduce;

    public int[] DamageAtLevel;
    public float[] CooldownAtLevel;
    public float[] SpeedAtLevel;
    public GameObject[] ProjectileSpawnPoints;

    void Start()
    {
        if(IsPlayerWeapon)
            cooldown = CooldownAtLevel[upgradeCounts[(int)GunUpgradeType.Speed]];
    }

    public void Reset()
    {
        if (IsPlayerWeapon)
        {
            upgradeCounts[(int)GunUpgradeType.Damage] = 0;
            upgradeCounts[(int)GunUpgradeType.Speed] = 0;
            upgradeCounts[(int)GunUpgradeType.NumShots] = 0;

            cooldown = CooldownAtLevel[upgradeCounts[(int)GunUpgradeType.Speed]];

            numProjectiles = upgradeCounts[(int)GunUpgradeType.NumShots] + 1;

            if (numProjectiles == 4)
                numProjectiles = 5;
        }
    }

    public int GetUpgrade(GunUpgradeType type)
    {
        return upgradeCounts[(int)type];
    }

    protected override void Shoot(Vector2 startPos, Vector2 direction, ColorKey color)
    {
        if (projectilePrefab)
        {

            if(IsPlayerWeapon)
            {
                if (upgradeCounts[(int)GunUpgradeType.NumShots] == 0)
                {
                    MyShoot(ProjectileSpawnPoints[0].transform.position, ProjectileSpawnPoints[0].transform.up, color);
                }

                else if (upgradeCounts[(int)GunUpgradeType.NumShots] == 1)
                {
                    MyShoot(ProjectileSpawnPoints[1].transform.position, ProjectileSpawnPoints[1].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[2].transform.position, ProjectileSpawnPoints[2].transform.up, color);
                }

                else if (upgradeCounts[(int)GunUpgradeType.NumShots] == 2)
                {
                    MyShoot(ProjectileSpawnPoints[0].transform.position, ProjectileSpawnPoints[0].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[1].transform.position, ProjectileSpawnPoints[1].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[2].transform.position, ProjectileSpawnPoints[2].transform.up, color);
                }

                else if (upgradeCounts[(int)GunUpgradeType.NumShots] == 3)
                {
                    MyShoot(ProjectileSpawnPoints[0].transform.position, ProjectileSpawnPoints[0].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[1].transform.position, ProjectileSpawnPoints[1].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[2].transform.position, ProjectileSpawnPoints[2].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[3].transform.position, ProjectileSpawnPoints[3].transform.up, color);

                    MyShoot(ProjectileSpawnPoints[4].transform.position, ProjectileSpawnPoints[4].transform.up, color);
                }
            }

            else
            {
                MyShoot(startPos, direction, color);
            }



        }
    }

    private void MyShoot(Vector2 startPos, Vector2 direction, ColorKey color)
    {
         LinearProjectile projectile = null;

         if (IsPlayerWeapon)
         {
             projectile = InstantiateProjectile(ColorKey.GetProjectileFromColor(color.Key)) as LinearProjectile;
             projectile.Speed = SpeedAtLevel[upgradeCounts[(int)GunUpgradeType.Speed]];


             int oneShotDmg = Mathf.Max((int)(DamageAtLevel[upgradeCounts[(int)GunUpgradeType.Damage]] * perShotDmgReduce * (numProjectiles - 1)), DamageAtLevel[upgradeCounts[(int)GunUpgradeType.Damage]]);

             int perShotDmg = (int)Mathf.Ceil((float)oneShotDmg / numProjectiles);

             projectile.Damage = perShotDmg;
         }


         else
             projectile = InstantiateProjectile(projectilePrefab) as LinearProjectile;

        if (ApplyStartPos)
            projectile.transform.position = startPos;

        if (RotateProjectileToDirection)
            projectile.transform.up = direction;

        projectile.Init(startPos, direction, color, IsPlayerWeapon);
    }

   

    public void UpgradeAttackDamage()
    {
     //   damageUpgradeCount 

        upgradeCounts[(int)GunUpgradeType.Damage] = Mathf.Clamp(upgradeCounts[(int)GunUpgradeType.Damage] + 1, 0, DamageAtLevel.Length - 1);
    }

    public void DegradeAttackDamage()
    {
        upgradeCounts[(int)GunUpgradeType.Damage] = Mathf.Clamp(upgradeCounts[(int)GunUpgradeType.Damage] - 1, 0, DamageAtLevel.Length - 1);
    }

    public void UpgradeAttackSpeed()
    {
        upgradeCounts[(int)GunUpgradeType.Speed] = Mathf.Clamp(upgradeCounts[(int)GunUpgradeType.Speed] + 1, 0, SpeedAtLevel.Length - 1);

        cooldown = CooldownAtLevel[upgradeCounts[(int)GunUpgradeType.Speed]];
    }

    public void DowngradeAttackSpeed()
    {
        upgradeCounts[(int)GunUpgradeType.Speed] = Mathf.Clamp(upgradeCounts[(int)GunUpgradeType.Speed] - 1, 0, SpeedAtLevel.Length - 1);

        cooldown = CooldownAtLevel[upgradeCounts[(int)GunUpgradeType.Speed]];
    }


    public void UpgradeNumAttacks()
    {
        upgradeCounts[(int)GunUpgradeType.NumShots] = Mathf.Clamp(upgradeCounts[(int)GunUpgradeType.NumShots] + 1, 0, 3);


        numProjectiles = upgradeCounts[(int)GunUpgradeType.NumShots] + 1;

        if (numProjectiles == 4)
            numProjectiles = 5;
    }

    public void DowngradeNumAttacks()
    {
        upgradeCounts[(int)GunUpgradeType.NumShots] = Mathf.Clamp(upgradeCounts[(int)GunUpgradeType.NumShots] - 1, 0, 3);

        numProjectiles = upgradeCounts[(int)GunUpgradeType.NumShots] + 1;

        if (numProjectiles == 4)
            numProjectiles = 5;
    }


    public void LoseAnyUpgrade()
    {

        List<int> avaibleUpgradeIds = new List<int>();

        for (int i = 0; i < upgradeCounts.Length; i++)
        {
            if (upgradeCounts[i] == 3)
            {
                avaibleUpgradeIds.Add(i);
            }
        }

        int rand = avaibleUpgradeIds[UnityEngine.Random.Range(0, avaibleUpgradeIds.Count)];

        if (rand == (int)GunUpgradeType.NumShots)
        {
            DowngradeNumAttacks();
        }

        else if (rand == (int)GunUpgradeType.Damage)
        {
            DegradeAttackDamage();
        }

        else if (rand == (int)GunUpgradeType.Speed)
        {
            DowngradeAttackSpeed();
        }
    }
}
