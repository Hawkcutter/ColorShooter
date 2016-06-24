using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public int uniqueId = -1;

    public ColorKey ColorKey;
         
    [SerializeField]
    private int breakThroughDamage;
    public int BreakThroughDamage { get { return breakThroughDamage; } }

    [SerializeField]
    private int difficulty;
    public int Difficulty { get { return difficulty; } }

    void Start()
    {
        GameManager.Instance.RegisterEnemy(this);
    }

}
