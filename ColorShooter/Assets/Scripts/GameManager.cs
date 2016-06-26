using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [ReadOnly]
    [SerializeField]
    private int highscore;
    public int Highscore { get { return highscore; } set { SetHighscore(value); } }

    [ReadOnly]
    [SerializeField]
    private float enemyStrengthFactor = 1.0f;

    [Header("Scene Objects")]
    public UserInterface ui;
    
    [SerializeField]
    private PlayerBase playerBase;

    [SerializeField]
    private GameObject levelBoundsMin;
    public Vector2 LevelBoundsMin { get { return levelBoundsMin.transform.position; } }

    [SerializeField]
    private GameObject levelBoundsMax;
    public Vector2 LevelBoundsMax { get { return levelBoundsMax.transform.position; } }

    [SerializeField]
    private SpawnZone[] spawnZones;


    [Header("Debug")]
    public bool SpawnEnemies = true;
    public int noOfPlayers;

    [ReadOnly]
    [SerializeField]
    private float roundTime;

    [Header("Score and Difficulty")]

    [SerializeField]
    private int playerBaseLifes = 10;

    [SerializeField]
    private float diffiIncreaseFactor = 0.25f;

    [SerializeField]
    private float timeToMaxDiff = 300.0f;

    private int difficultyPower = 2;

    private float timeToNextScoreIncrease = 5.0f;

    [SerializeField]
    private float upgrChance = 0.0f;
    [SerializeField]
    private float upgrChanceInc = 0.05f;

    [SerializeField]
    private float enemySpawnCD = 1.0f;
    private float enemySpawnCDTotal;
    private float curEnemySpawnCD;
    [SerializeField]
    private float enemySpawnCDReduce = 0.95f;
    [SerializeField]
    private float enemySpawnCDMin = 1.0f;
    [ReadOnly]
    [SerializeField]
    private float curDifficultyPercent;
    [ReadOnly]
    [SerializeField]
    private int curDifficulty;
    [ReadOnly]
    [SerializeField]
    private int maxEnemyDifficulty;

    [Header("LayerNames")]
    [SerializeField]
    private string enemyLayerName;
    private int enemyLayer;

    [SerializeField]
    private string playerLayerName;
    private int playerLayer;

    [Header("Prefabs")]
    [SerializeField]
    private LinearProjectile redBulletPrefab;
    [SerializeField]
    private LinearProjectile greenBulletPrefab;
    [SerializeField]
    private LinearProjectile blueBulletPrefab;
    [SerializeField]
    private LinearProjectile yellowBulletPrefab;

    [SerializeField]
    private GameObject upgradePrefab;

    [SerializeField]
    private Enemy[] enemyPrefabs;
    private List<List<Enemy>> difficultySortedEnemyPrefabs;

    private UniqueList<Enemy> enemies;
    private UniqueList<Projectile> projectiles;


    private List<Player> players;
    

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

        players = new List<Player>();

        if(ui)
            ui.UpdateLifes(playerBase.Lifes);

        enemySpawnCDTotal = enemySpawnCD;
    }

    void Start()
    {
        Reset();
    }

    public void RegisterPlayer(Player player)
    {
        this.players.Add(player);
    }

    public void Reset()
    {
        Debug.Log("reset game");

        upgrChance = 0;
        curDifficulty = 0;
        roundTime = 0.0f;
        curDifficultyPercent = 0.0f;
        curEnemySpawnCD = 0.0f;
        enemySpawnCD = enemySpawnCDTotal;

        playerBase.SetLifes(this.playerBaseLifes);

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

        for (int i = 0; i < players.Count; i++)
        {
            players[i].Reset();
        }

        playerBase.SetLifes(playerBaseLifes);

        Highscore = 0;
    }

    private void SetHighscore(int value)
    {
        if (this.highscore != value)
        {
            this.highscore = value;

            if (ui)
                ui.UpdateScore(this.highscore);
        }
    }

    void CreatePrefabList()
    {
        difficultySortedEnemyPrefabs = new List<List<Enemy>>();
        int maxDifficulty = -1;

        maxEnemyDifficulty = int.MinValue;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            if (enemyPrefabs[i].Difficulty > maxDifficulty)
                maxDifficulty = enemyPrefabs[i].Difficulty;

            if (enemyPrefabs[i].Difficulty > maxEnemyDifficulty)
                maxEnemyDifficulty = enemyPrefabs[i].Difficulty;
        }

        maxDifficulty += 1;

        for (int i = 0; i < maxDifficulty; i++)
            difficultySortedEnemyPrefabs.Add(new List<Enemy>());

        for (int i = 0; i < enemyPrefabs.Length; i++)
            difficultySortedEnemyPrefabs[enemyPrefabs[i].Difficulty].Add(enemyPrefabs[i]);


        //fill empty difficulties with earlier ones that are not empty:
        for (int i = 0; i < difficultySortedEnemyPrefabs.Count; i++)
        {
            if (difficultySortedEnemyPrefabs[i].Count == 0)
            {
                for (int j = i; j >= 0; j--)
                {
                    if (difficultySortedEnemyPrefabs[j].Count != 0)
                    {
                        difficultySortedEnemyPrefabs[i] = difficultySortedEnemyPrefabs[j];
                        break;
                    }
                }
            }
        }
    }

    Enemy GetEnemyPrefab(int difficulty)
    {
        difficulty = Mathf.Clamp(difficulty, 0, difficultySortedEnemyPrefabs.Count - 1);

        int randEnemy = Random.Range(0, difficultySortedEnemyPrefabs[difficulty].Count);

        return difficultySortedEnemyPrefabs[difficulty][randEnemy];
    }

    private float GetDifficultyDistribution(int monsterDifficulty)
    {
        float monsterDiff = ((float)monsterDifficulty / maxEnemyDifficulty);

        float val = Mathf.Abs(1.0f / (1 + Mathf.Abs(monsterDiff - curDifficultyPercent)));

        for (int i = 0; i < difficultyPower; i++)
            val *= val;

        return val;
    }

    public void SpawnEnemy(GameObject prefab)
    {
        GameObject createdEnemy = Instantiate(prefab.gameObject);

        createdEnemy.transform.position = GetRandomSpawnPosition();

        createdEnemy.GetComponent<Enemy>().Init(this.enemyStrengthFactor);
    }
    
    void SpawnEnemy()
    {
        if (SpawnEnemies)
        {
            float[] accumulatesChances = new float[difficultySortedEnemyPrefabs.Count];
            float sum = 0.0f;

            for (int i = 0; i < difficultySortedEnemyPrefabs.Count; i++)
            {
                int previous = i - 1;

                if(previous < 0)
                    previous = 0;

                float curVal = GetDifficultyDistribution(i);
                accumulatesChances[i] = curVal + accumulatesChances[previous];
                sum += curVal;
            }

            float randVal = Random.value;

            int pickId = -1;

            for (int i = 0; i < accumulatesChances.Length; i++)
            {
                float curChance = accumulatesChances[i] / sum;

                if (randVal <= curChance)
                {
                    pickId = i;
                    break;               
                }
            }

            Enemy prefab = GetEnemyPrefab(pickId);

            SpawnEnemy(prefab.gameObject);
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        SpawnZone zone = spawnZones[Random.Range(0, spawnZones.Length)];

        return zone.GetPointInsideArea();
    }

    private void SpawnUpgrade()
    {
        GameObject createdUpgrade = Instantiate(upgradePrefab);

        createdUpgrade.transform.position = GetRandomSpawnPosition();
    }

    void Update()
    {
        roundTime += Time.deltaTime;

        curDifficultyPercent = roundTime / timeToMaxDiff;

        int tmpDifficulty = (int)(curDifficultyPercent * maxEnemyDifficulty);

        if (tmpDifficulty != curDifficulty)
        {
            curDifficulty = tmpDifficulty;

            curDifficulty = curDifficulty % maxEnemyDifficulty;

            int remainder = tmpDifficulty / maxEnemyDifficulty;

            enemyStrengthFactor = 1.0f + (remainder) * diffiIncreaseFactor;
        }

        timeToNextScoreIncrease -= Time.deltaTime;

        if(timeToNextScoreIncrease <= 0)
        {
            Highscore++;
            timeToNextScoreIncrease = 5.0f;

            if(enemySpawnCD > enemySpawnCDMin)
                enemySpawnCD *= enemySpawnCDReduce;

            else if(enemySpawnCD != enemySpawnCDMin)
                enemySpawnCD = enemySpawnCDMin;

            if (Random.value < upgrChance)
            {
                upgrChance = 0.0f;
                SpawnUpgrade();
            }

            else 
            {
                upgrChance += upgrChanceInc;
            }
        }

        curEnemySpawnCD -= Time.deltaTime;

        if(curEnemySpawnCD <= 0.0f)
        {
            curEnemySpawnCD = enemySpawnCD;
            SpawnEnemy();
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
            if(manager.CurLife <=0)
                Highscore += enemy.Score;

            if (ui)
            {
                 ui.UpdateLifes(playerBase.Lifes);
            }
              
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
