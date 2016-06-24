using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public ColorKey ColorKey;
         
    [SerializeField]
    private int breakThroughDamage;
    public int BreakThroughDamage { get { return breakThroughDamage; } }

    [SerializeField]
    private int difficulty;
    public int Difficulty { get { return difficulty; } }

    [SerializeField]
    private HitpointManager hitPointManager;

    [SerializeField]
    private MovementPattern movementPattern;

    void Update()
    {
        //GetComponent<Weapon>().TryShoot(new Vector2(0.0f, -1.0f), ColorKey.GetRandomColorKey());
    }
}
