using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UserInterface ui;

    [SerializeField]
    private PlayerBase playerBase;

    [SerializeField]
    private SpawnZone[] spawnZones;

    private int detroyedEnemies;
    private float gameTime;

    private float curDifficulty;
    public float CurrentGameDifficulty { get { return curDifficulty; } }

    private int highscore;
    public int Highscore { get { return highscore; } }

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


    private List<Enemy> enemies;
    private Queue<int> freeIndices;


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


        enemies = new List<Enemy>();
        freeIndices = new Queue<int>();

        for (int i = 0; i < 10; i++)
        {
            enemies.Add(null);
            freeIndices.Enqueue(i);
        }

      
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
        Enemy prefab = GetEnemyPrefabOfDifficulty(difficulty);

        GameObject createdEnemy = Instantiate(prefab.gameObject);

        SpawnZone zone = spawnZones[(int)(Random.value * spawnZones.Length)];
        createdEnemy.transform.position = zone.GetPointInsideArea();
       
    }




    void Update()
    {
        gameTime += Time.deltaTime;

        curSpawnCooldown -= Time.deltaTime;

        if(curSpawnCooldown <= 0.0f)
        {
            curSpawnCooldown = maxSpawnCooldown;
            SpawnEnemy(0);
        }
    }

    public void OnEnemyDestroyed(Enemy enemy)
    {
        if(enemy != null)
        {
            Debug.Log("Destroy");
            detroyedEnemies++;
            highscore += enemy.Score;
            ui.UpdateScore(highscore);
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


    public void RegisterEnemy(Enemy enemy)
    {
        int id = GetFreeIndexForEnemy();
        Debug.Assert(enemies[id] == null);
        Debug.Assert(id >= 0);

        enemy.uniqueId = id;
        enemies[id] = enemy;
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        Debug.Assert(enemy.uniqueId != -1);
        Debug.Assert(enemies[enemy.uniqueId] == enemy);

        freeIndices.Enqueue(enemy.uniqueId);
        enemies[enemy.uniqueId] = null;
    }

    private int GetFreeIndexForEnemy()
    {
        if (freeIndices.Count == 0)
        {
            int oldCount = enemies.Count;

            for (int i = 0; i < 20; i++)
            {
                enemies.Add(null);
                freeIndices.Enqueue(oldCount + i);
            }
        }

        return freeIndices.Dequeue();
    }
}
