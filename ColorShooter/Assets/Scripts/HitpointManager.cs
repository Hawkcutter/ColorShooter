using UnityEngine;
using System.Collections;

public class HitpointManager : MonoBehaviour
{
    private Enemy enemy;

    private Player player;

    [SerializeField]
    private int curLife;
    public int CurLife { get { return curLife; } }

    private int maxLife;
    public int MaxLife{ get { return maxLife; } }

    public bool IgnoreDamage = false;

    public float PercentLife { get { return (float)curLife / (float)maxLife; } }

    [SerializeField]
    private GameObject RootObject;



    void Start()
    {
        if (RootObject == null)
            RootObject = gameObject;

            this.maxLife = curLife;
        

        enemy = RootObject.GetComponent<Enemy>();

        if (!enemy)
        {
            player = RootObject.GetComponent<Player>();
            if (player){
                curLife = 1;
            maxLife = 1;
            }
        }
        else
        {
            this.maxLife = enemy.Hitpoints;
            this.curLife = enemy.Hitpoints;
        }
    }

    public void Damage(int damage, ColorKey bulletColor)
    {
        if (IgnoreDamage)
            return;

        if (damage != 0)
        {
            //it is an enemy:
            if (enemy != null)
            {
                if (enemy.ShieldObject != null)
                    return;

                else if (bulletColor == enemy.ColorKey || enemy.ColorKey.Key == ColorKey.EColorKey.White)
                    ChangeHealth(-damage);         
            }

            //it is a player
            else
            {
                ChangeHealth(-damage);
            }
        }
    }

    private void ChangeHealth(int delta)
    {
        this.curLife += delta;

        if (curLife < 0)
        {
            curLife = 0;

            if (!player)
                Destroy(RootObject);

            else
            {
                player.Killed();
            }
        }

        else if (curLife > maxLife)
        {
            curLife = maxLife;
        }
    }





    public void ResetHitpoints()
    {
        curLife = maxLife;
    }
}
