using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IUnique
{
    

    public int UniqueId { get; set; }

    public ColorKey ColorKey { get; private set; }
         
    [SerializeField]
    private int breakThroughDamage;
    public int BreakThroughDamage { get { return breakThroughDamage; } }

    [SerializeField]
    private int difficulty;
    public int Difficulty { get { return difficulty; } }

    [SerializeField]
    private Enemy shieldObject;

    public Enemy ShieldObject { get { return shieldObject; } }

    public bool IsRoot = true;

    public int score;
  
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


    public void Kill()
    {
        Destroy(gameObject);
    }


}
