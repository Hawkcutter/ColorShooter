using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    public Text scoreText;
    public Text lifeText;
    public Player player1;
    public Player player2;
    public Player player3;
    public Player player4;

    public PlayerUI playerUI_1;
    public PlayerUI playerUI_2;
    public PlayerUI playerUI_3;
    public PlayerUI playerUI_4;
    

	// Use this for initialization
	void Start () 
    {
        scoreText.text = "Score: 0";
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (player1.gameObject.activeSelf)
            playerUI_1.gameObject.SetActive(true);
        else
            playerUI_1.gameObject.SetActive(false);

        if (player2.gameObject.activeSelf)
            playerUI_2.gameObject.SetActive(true);
        else
            playerUI_2.gameObject.SetActive(false);

        if (player3.gameObject.activeSelf)
            playerUI_3.gameObject.SetActive(true);
        else
            playerUI_3.gameObject.SetActive(false);

        if (player4.gameObject.activeSelf)
            playerUI_4.gameObject.SetActive(true);
        else
            playerUI_4.gameObject.SetActive(false);

	}

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void UpdateLifes(int lifes)
    {
        if (lifeText != null)
            lifeText.text = "Lifes: " + lifes;
    }
}
