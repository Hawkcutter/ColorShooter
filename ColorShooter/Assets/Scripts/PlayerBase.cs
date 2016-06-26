using UnityEngine;
using System.Collections;

public class PlayerBase : MonoBehaviour 
{
    [SerializeField]
    private int lifes;
    public int Lifes { get { return lifes; } }

    public bool Indestructible = false;

    public void SetLifes(int amount)
    {
        this.lifes = amount;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();

        if (enemy)
        {
            if(!Indestructible)
                lifes -= enemy.BaseDmg;

            if (lifes <= 0)
            {
                lifes = 0;
                GameManager.Instance.Reset();
            }

            Destroy(enemy.gameObject);
        }
    }
}
