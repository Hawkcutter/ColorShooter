using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    

    public Text scoreText;
    

	// Use this for initialization
	void Start () 
    {
        scoreText.text = "Score: 0";
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (score == 0)

    }
}
