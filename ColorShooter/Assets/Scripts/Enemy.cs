using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IUnique
{
    public enum ColorMode { ByRgbColor, BySprite }


    public ColorMode Mode;
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

    [SerializeField]
    private Sprite redSprite;
    [SerializeField]
    private Sprite yellowSprite;
    [SerializeField]
    private Sprite greenSprite;
    [SerializeField]    
    private Sprite blueSprite;
  
    void Start()
    {
        ColorKey = ColorKey.GetRandomColorKey();




        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        if (sprite)
        {
            if(Mode == ColorMode.ByRgbColor)
                sprite.color = ColorKey.RgbColor;

            else if(Mode == ColorMode.BySprite)
            {
                if (ColorKey.Key == ColorKey.EColorKey.Red)
                    sprite.sprite = redSprite;

                if (ColorKey.Key == ColorKey.EColorKey.Blue)
                    sprite.sprite = blueSprite;

                if (ColorKey.Key == ColorKey.EColorKey.Green)
                    sprite.sprite = greenSprite;

                if (ColorKey.Key == ColorKey.EColorKey.Yellow)
                    sprite.sprite = yellowSprite;
            }
        }
         

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
