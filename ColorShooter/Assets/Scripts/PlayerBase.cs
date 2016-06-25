using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour 
{
    [SerializeField]
    private int lifes;

    public void SetLifes(int amount)
    {
        this.lifes = amount;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();

        if (enemy)
        {
            lifes -= enemy.BreakThroughDamage;

            if (lifes <= 0)
            {
                lifes = 0;
                GameManager.Instance.Reset();
            }

            Destroy(enemy.gameObject);
        }
    }
}
