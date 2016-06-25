using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public UserInterface ui;
    


    [SerializeField]
    private int playerBaseLifes = 10;

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

    private UniqueList<Enemy> enemies;
    private UniqueList<Projectile> projectiles;

    public bool SpawnEnemies;

    [SerializeField]
    private LinearProjectile redBulletPrefab;
    [SerializeField]
    private LinearProjectile greenBulletPrefab;
    [SerializeField]
    private LinearProjectile blueBulletPrefab;
    [SerializeField]
    private LinearProjectile yellowBulletPrefab;

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

        ColorKey.InitColors(redBulletPrefab, greenBulletPrefab, blueBulletPrefab, yellowBulletPrefab);

        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);

        enemies = new UniqueList<Enemy>(10);
        projectiles = new UniqueList<Projectile>(50);

        if(ui)
            ui.UpdateLifes(playerBase.Lifes);

      
    }

    public void Reset()
    {
        detroyedEnemies = 0;
        gameTime = 0.0f;
        curDifficulty = 0.0f;
        curSpawnCooldown = 0.0f;
        

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies.Get(i) != null)
                Destroy(enemies.Get(i).gameObject);
        }


        for (int i = 0; i < projectiles.Count; i++)
        {
            if (projectiles.Get(i) != null)
                Destroy(projectiles.Get(i).gameObject);
        }

        playerBase.SetLifes(playerBaseLifes);
        highscore = 0;

        if(ui)
            ui.UpdateScore(highscore);

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
        if (SpawnEnemies)
        {
            Enemy prefab = GetEnemyPrefabOfDifficulty(difficulty);

            GameObject createdEnemy = Instantiate(prefab.gameObject);

            SpawnZone zone = spawnZones[(int)(Random.value * spawnZones.Length)];
            createdEnemy.transform.position = zone.GetPointInsideArea();
        }
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
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
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            HitpointManager manager = enemy.GetComponent<HitpointManager>();         
            if(manager.CurLife <= 0)
            {
                highscore += enemy.score;

                if(ui)
                    ui.UpdateScore(highscore);
            }

            if(ui)
                ui.UpdateLifes(playerBase.Lifes);

            detroyedEnemies++;
            
            enemies.Remove(enemy);
        }
        
    }

    public void RegisterProjectile(Projectile projectile)
    {
        projectiles.Add(projectile);
    }

    public void UnregisterProjectile(Projectile projectile)
    {
        projectiles.Remove(projectile);
    }


}
