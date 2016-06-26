using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour, IUnique
{
    public enum ColorMode { ByRgbColor, BySprite }

    [Header("GameDesign")]
    [SerializeField]
    private int hitpoints;
    public int Hitpoints { get { return hitpoints; } }
    [SerializeField]
    private int baseDmg;
    public int BaseDmg { get { return baseDmg; } }
    [SerializeField]
    private int difficulty;
    public int Difficulty { get { return difficulty; } }
    [SerializeField]
    private int score;
    public int Score { get { return score; } }
    [SerializeField]
    private Enemy shieldObject;
    public Enemy ShieldObject { get { return shieldObject; } }
    [SerializeField]
    private float verticalSpeed;
    public float VerticalSpeed { get { return verticalSpeed; } }
    [SerializeField]
    private int touchDamage;
    public int TouchDamage { get { return touchDamage; } }


    [Header("Other")]
    public ColorMode MyColorMode;
    public int UniqueId { get; set; }

    private ColorKey colorKey;
    public ColorKey ColorKey { get { return colorKey; } set { SetColor(value); } }

    public bool IsRoot = true;

    [SerializeField]
    private Sprite redSprite;
    [SerializeField]
    private Sprite yellowSprite;
    [SerializeField]
    private Sprite greenSprite;
    [SerializeField]    
    private Sprite blueSprite;

    private SpriteRenderer sprite;

    public void Init(float strengthFactor)
    {
        hitpoints = (int)(hitpoints * strengthFactor);
        score = (int)(score * strengthFactor);
        verticalSpeed *= strengthFactor;

        if (shieldObject)
            shieldObject.Init(strengthFactor);
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        ColorKey = ColorKey.GetRandomColorKey();

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


    void SetColor(ColorKey color)
    {
        this.colorKey = color;

        if (sprite)
        {
            if (MyColorMode == ColorMode.ByRgbColor)
                sprite.color = ColorKey.RgbColor;

            else if (MyColorMode == ColorMode.BySprite)
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player)
        {
            HitpointManager hitpointM = player.GetComponent<HitpointManager>();

            if (hitpointM)
            {
                hitpointM.Damage(this.TouchDamage, this.colorKey);
            }
        }
    }


}
