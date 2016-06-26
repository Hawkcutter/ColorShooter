using UnityEngine;
using System.Collections;

public class BugSwarm : MonoBehaviour {

    static float spawnChance = 1.0f;

    [SerializeField]
    private GameObject BugPrefab;

	// Use this for initialization
	void Start () 
    {
        if (Random.value < spawnChance)
        {
            GameManager.Instance.SpawnEnemy(BugPrefab);

            spawnChance *= 0.65f;

        }

        else
        {
            spawnChance = 1.0f;
        }
	}
	
}
