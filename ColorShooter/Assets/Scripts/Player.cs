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
        

      
	}

    void Shoot()
    {
        Debug.Log(playerID);
    }

    void Update()
    {
        if (Input.GetButtonDown(shootGreenName))
        {
            Shoot();
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
        velocity = movement * speed;

        rb2D.MovePosition(rb2D.position + velocity * Time.fixedDeltaTime);

	
	}
}
