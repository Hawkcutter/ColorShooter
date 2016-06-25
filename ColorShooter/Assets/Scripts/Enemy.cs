using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
   
    public int uniqueId = -1;
    

    public ColorKey ColorKey;
         
    [SerializeField]
    private int breakThroughDamage;
    public int BreakThroughDamage { get { return breakThroughDamage; } }

    [SerializeField]
    private int difficulty;
    public int Difficulty { get { return difficulty; } }

    public bool IsRoot = true;

    public int score;
    public int Score { get { return score; } }

    void Start()
    {
        ColorKey = ColorKey.GetRandomColorKey();


        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if(sprite)
            sprite.color = ColorKey.RgbColor;

        if (IsRoot)
        {
            GameManager.Instance.RegisterEnemy(this);
        }

    }

    void OnDestroy()
    {
        if (IsRoot)
        {
            GameManager.Instance.UnregisterEnemy(this);
        }
    }

}
