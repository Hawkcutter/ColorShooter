using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed;
    public int playerID;
    public int controllerID;
    float moveHorizontal;
    float moveVertical;
    Rigidbody2D rb2D;
    Vector2 velocity;
    string horizontalAxisName;
    string verticalAxisName;
    string shootGreenName;
    string shootRedName;
    string shootBlueName;
    string shootYellowName;
    string boostLeftName;
    string boostRightName;

    public bool controllerRegistered;


    public Gun mainWeapon;

    float xMin, xMax, yMin, yMax;

    [SerializeField]
    private float respawnCooldown = 5.0f;
    private float curRespawnCooldown;

    [SerializeField]
    private HitpointManager hitpointManager;

    private bool isDead;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private Collider2D playerCollider;


    [Header("Boost")]
    [SerializeField]
    private float boostCooldown;
    private float curBoostCooldown;

    [SerializeField]
    private float boostSpeed = 5.0f;

    [SerializeField]
    private float boostDuration = 5.0f;
    private float curBoostDuration = 0.0f;

    private Vector2 boostDirection;

    // Use this for initialization
    void Start()
    {
        curRespawnCooldown = respawnCooldown;

        rb2D = GetComponent<Rigidbody2D>();
        /*horizontalAxisName = "Horizontal_P" + playerID;
        verticalAxisName = "Vertical_P" + playerID;
        shootGreenName = "FireGreen_P" + playerID;
        shootRedName = "FireRed_P" + playerID;
        shootBlueName = "FireBlue_P" + playerID;
        shootYellowName = "FireYellow_P" + playerID;

        boostLeftName = "BoostLeft_P" + playerID;
        boostRightName = "BoostRight_P" + playerID;*/

        horizontalAxisName = "Horizontal_P" + 11;
        verticalAxisName = "Vertical_P" + 11;
        shootGreenName = "FireGreen_P" + 11;
        shootRedName = "FireRed_P" + 11;
        shootBlueName = "FireBlue_P" + 11;
        shootYellowName = "FireYellow_P" + 11;

        boostLeftName = "BoostLeft_P" + 11;
        boostRightName = "BoostRight_P" + 11;

        xMin = GameManager.Instance.LevelBoundsMin.x;
        xMax = GameManager.Instance.LevelBoundsMax.x;
        yMin = GameManager.Instance.LevelBoundsMin.y;
        yMax = GameManager.Instance.LevelBoundsMax.y;


        curBoostCooldown = 0.0f;

        GameManager.Instance.RegisterPlayer(this);
        this.gameObject.SetActive(false);
    }

    public void register(int ID)
    {
        this.horizontalAxisName = "Horizontal_P" + ID;
        this.verticalAxisName = "Vertical_P" + ID;
        this.shootGreenName = "FireGreen_P" + ID;
        this.shootRedName = "FireRed_P" + ID;
        this.shootBlueName = "FireBlue_P" + ID;
        this.shootYellowName = "FireYellow_P" + ID;

        this.boostLeftName = "BoostLeft_P" + ID;
        this.boostRightName = "BoostRight_P" + ID;

        this.controllerRegistered = true;
    }

    public void Killed()
    {
        isDead = true;
        rb2D.velocity = Vector2.zero;

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.25f);

        playerCollider.enabled = false;
    }

    public void Reset()
    {
        Respawned();

        mainWeapon.Reset();
    }

    public void Respawned()
    {
        isDead = false;
        curRespawnCooldown = respawnCooldown;

        hitpointManager.ResetHitpoints();

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);

        playerCollider.enabled = true;
    }

    void Update()
    {

        //Debug.Log(playerID + " " + controllerRegistered);
        curBoostDuration -= Time.deltaTime;

        if (isDead)
        {
            curRespawnCooldown -= Time.deltaTime;

            if (curRespawnCooldown <= 0.0f)
            {
                Respawned();
            }
        }


        else
        {
            if (Input.GetButton(shootGreenName))
            {
                mainWeapon.TryShoot(new Vector2(0, 1), ColorKey.GetColorKey(ColorKey.EColorKey.Green));
            }
            else if (Input.GetButton(shootRedName))
            {
                mainWeapon.TryShoot(new Vector2(0, 1), ColorKey.GetColorKey(ColorKey.EColorKey.Red));
            }
            else if (Input.GetButton(shootBlueName))
            {
                mainWeapon.TryShoot(new Vector2(0, 1), ColorKey.GetColorKey(ColorKey.EColorKey.Blue));
            }
            else if (Input.GetButton(shootYellowName))
            {
                mainWeapon.TryShoot(new Vector2(0, 1), ColorKey.GetColorKey(ColorKey.EColorKey.Yellow));
            }
        }
    }
	
	// Update is called once per frame
    void FixedUpdate()
    {
        if (curBoostDuration > 0.0f)
        {
            rb2D.velocity = boostDirection * boostSpeed;

            if(rb2D.position.x < xMin || rb2D.position.x > xMax || rb2D.position.y < yMin || rb2D.position.y > yMax)
            {
                rb2D.velocity = Vector2.zero;
                curBoostDuration = 0.0f;
            } 
        }

        else
        {

            moveHorizontal = Input.GetAxis(horizontalAxisName);
            moveVertical = -Input.GetAxis(verticalAxisName);

            if (Mathf.Abs(moveHorizontal) < 0.35f)
                moveHorizontal = 0f;

            if (Mathf.Abs(moveVertical) < 0.35f)
                moveVertical = 0f;

            Vector2 movement = new Vector2(moveHorizontal, moveVertical);
            rb2D.velocity = movement * speed;
        }
        //rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);
        rb2D.position = new Vector2
        (
            Mathf.Clamp(rb2D.position.x, xMin, xMax),
            Mathf.Clamp(rb2D.position.y, yMin, yMax)
        );

        if(curBoostCooldown > 0.0f)
            curBoostCooldown -= Time.fixedDeltaTime;

        else
        {
            if (Input.GetButtonDown(boostLeftName))
                ApplyBoost(new Vector2(-boostSpeed, 0.0f));

            else if (Input.GetButtonDown(boostRightName))
                ApplyBoost(new Vector2(boostSpeed, 0.0f));
        }
      
    }

    void ApplyBoost(Vector2 direction)
    {
        boostDirection = direction;

        curBoostCooldown = boostCooldown;

        curBoostDuration = boostDuration;
    }


}
