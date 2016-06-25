using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour {

    public Text scoreText;
    

	// Use this for initialization
	void Start () 
    {
       
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void UpdateScore(int score)
    {      
        scoreText.text = "Score: " + score;
    }
}
