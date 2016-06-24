using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    [SerializeField]
    private PlayerBase playerBase;

    [SerializeField]
    private SpawnZone[] spawnZones;

    private int detroyedEnemies;
    private float gameTime;

    private float curDifficulty;
    public float CurrentGameDifficulty { get { return curDifficulty; } }

    [SerializeField]
    private Enemy[] enemyPrefabs;
    private List<List<Enemy>> difficultySortedEnemyPrefabs;

    [SerializeField]
    private float maxSpawnCooldown = 1.0f;
    private float curSpawnCooldown;


    [Header("LayerNames")]
    [SerializeField]
    private string enemyLayerName;
    private int enemyLayer;

    [SerializeField]
    private string playerLayerName;
    private int playerLayer;


    void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Multiple GameManagers detected, destroying: " + this.gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        CreatePrefabList();

        ColorKey.InitColors();

        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);
    }


    void Reset()
    {
        detroyedEnemies = 0;
        gameTime = 0.0f;
        curDifficulty = 0.0f;
        curSpawnCooldown = 0.0f;
    }

    void CreatePrefabList()
    {
        difficultySortedEnemyPrefabs = new List<List<Enemy>>();
        int maxDifficulty = -1;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].Difficulty > maxDifficulty)
                maxDifficulty = enemyPrefabs[i].Difficulty;
        }

        maxDifficulty += 1;

        for (int i = 0; i <= maxDifficulty; i++)
            difficultySortedEnemyPrefabs.Add(new List<Enemy>());

        for (int i = 0; i < enemyPrefabs.Length; i++)
            difficultySortedEnemyPrefabs[enemyPrefabs[i].Difficulty].Add(enemyPrefabs[i]);
    }

    Enemy GetEnemyPrefabOfDifficulty(int difficulty)
    {
        if(difficulty < 0 || difficulty > difficultySortedEnemyPrefabs.Count)
        {
            Debug.Log("tried to fetch an enemy of difficulty that is not avaible! Difficulty: " + difficulty);
            return null;
        }

        else
        {
            int numOfAvaibleEnemies = difficultySortedEnemyPrefabs[difficulty].Count;
            int randValue = (int)(Random.value * numOfAvaibleEnemies);

            return difficultySortedEnemyPrefabs[difficulty][randValue];
        }
    }

    void SpawnEnemy(int difficulty)
    {
        SpawnEnemy(difficulty, ColorKey.GetRandomColorKey());
    }

    void SpawnEnemy(int difficulty, ColorKey color)
    {
        Enemy prefab = GetEnemyPrefabOfDifficulty(difficulty);

        GameObject createdEnemy = Instantiate(prefab.gameObject);

        SpawnZone zone = spawnZones[(int)(Random.value * spawnZones.Length)];
        createdEnemy.transform.position = zone.GetPointInsideArea();
        
       createdEnemy.GetComponent<Enemy>().ColorKey = color;
       createdEnemy.GetComponent<SpriteRenderer>().color = color.RgbColor;
    }




    void Update()
    {
        gameTime += Time.deltaTime;

        curSpawnCooldown -= Time.deltaTime;

        if(curSpawnCooldown <= 0.0f)
        {
            curSpawnCooldown = maxSpawnCooldown;
            SpawnEnemy(0, ColorKey.GetRandomColorKey());
        }
    }

    public void OnEnemyDestroyed(Enemy enemy)
    {
        if(enemy != null)
        {
            detroyedEnemies++;
        }
    }

    public int GetEnemyLayer()
    {
        return enemyLayer;
    }

    public int GetPlayerLayer()
    {
        return playerLayer;
    }

}
