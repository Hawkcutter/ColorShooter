using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float speed;
    public int playerID;
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
    public Weapon weapon;

    float xMin, xMax, yMin, yMax;
    

	// Use this for initialization
	void Start () 
    {
        rb2D = GetComponent<Rigidbody2D>();
        horizontalAxisName  = "Horizontal_P"    + playerID;
        verticalAxisName    = "Vertical_P"      + playerID;
        shootGreenName      = "FireGreen_P"     + playerID;
        shootRedName        = "FireRed_P"       + playerID;
        shootBlueName       = "FireBlue_P"      + playerID;
        shootYellowName     = "FireYellow_P"    + playerID;

        xMin = -6.0f;
        xMax = 6.0f;
        yMin = -4.6f;
        yMax = 4.6f;
      
	}

    void Shoot()
    {
        Debug.Log(playerID);
    }

    void Update()
    {
        if (Input.GetButton(shootGreenName))
        {
            weapon.TryShoot(new Vector2(0,1),ColorKey.GetColorKey( ColorKey.EColorKey.Green));
        }
        else if (Input.GetButton(shootRedName))
        {
            weapon.TryShoot(new Vector2(0, 1), ColorKey.GetColorKey(ColorKey.EColorKey.Red));
        }
        else if (Input.GetButton(shootBlueName))
        {
            weapon.TryShoot(new Vector2(0, 1), ColorKey.GetColorKey(ColorKey.EColorKey.Blue));
        }
        else if (Input.GetButton(shootYellowName))
        {
            weapon.TryShoot(new Vector2(0, 1), ColorKey.GetColorKey(ColorKey.EColorKey.Yellow));
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () 
    {   
        moveHorizontal = Input.GetAxis(horizontalAxisName);
        moveVertical = -Input.GetAxis(verticalAxisName);

        if (Mathf.Abs(moveHorizontal) < 0.35f)
            moveHorizontal = 0f;

        if (Mathf.Abs(moveVertical) < 0.35f)
            moveVertical = 0f;

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        rb2D.velocity = movement * speed;
        //rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);
        rb2D.position = new Vector2
        (
            Mathf.Clamp(rb2D.position.x, xMin, xMax),
            Mathf.Clamp(rb2D.position.y, yMin, yMax)
        );
	}
}
