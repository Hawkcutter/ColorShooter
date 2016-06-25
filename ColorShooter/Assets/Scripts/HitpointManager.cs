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

        if (curLife <= 0)
        {
            Debug.Log(gameObject.name + " has 0 life upon construction, destroying it!");
            Destroy(RootObject);
        }

        else
        {
            this.maxLife = curLife;
        }

        enemy = RootObject.GetComponent<Enemy>();

        if (!enemy)
            player = RootObject.GetComponent<Player>();
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

                else if (bulletColor == enemy.ColorKey)
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
