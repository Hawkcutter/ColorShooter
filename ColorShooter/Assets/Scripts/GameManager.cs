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

    private float gameTime;

    public bool SpawnEnemies = true;

    [Header("Score and Difficulty")]
    [SerializeField]
    private float gameTimeToMaxDifficulty = 300.0f;
    [ReadOnly]
    [SerializeField]
    private float curDifficultyPercent;
    [ReadOnly]
    [SerializeField]
    private int curDifficulty;
    [ReadOnly]
    [SerializeField]
    private int maxEnemyDifficulty;
    private float timeToNextScoreIncrease = 5.0f;
    private int highscore;
    public int Highscore { get { return highscore; } set { SetHighscore(value); } }

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
    private Enemy[] enemyPrefabs;
    private List<List<Enemy>> difficultySortedEnemyPrefabs;

    private UniqueList<Enemy> enemies;
    private UniqueList<Projectile> projectiles;

    [SerializeField]
    private int difficultyPower = 5;

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
        curDifficulty = 0;
        gameTime = 0.0f;
        curDifficultyPercent = 0.0f;
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

        Highscore = 0;
    }

    private void SetHighscore(int value)
    {
        this.highscore = value;

        if (ui)
            ui.UpdateScore(this.highscore);
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
                //blub += (curChance).ToString() + ", ";


                if (randVal <= curChance)
                {
                    pickId = i;
                    break;
                    
                }
                 
            }

            Enemy prefab = GetEnemyPrefab(pickId);

            GameObject createdEnemy = Instantiate(prefab.gameObject);

            SpawnZone zone = spawnZones[(int)(Random.value * spawnZones.Length)];
            createdEnemy.transform.position = zone.GetPointInsideArea();
        }
    }




    void Update()
    {
        gameTime += Time.deltaTime;

        curDifficultyPercent = gameTime / gameTimeToMaxDifficulty;
        curDifficultyPercent = Mathf.SmoothStep(0.0f, 1.0f, curDifficultyPercent);

        curDifficulty = (int)(curDifficultyPercent * maxEnemyDifficulty);

        curDifficulty = Mathf.Min(curDifficulty, maxEnemyDifficulty);

        timeToNextScoreIncrease -= Time.deltaTime;

        if(timeToNextScoreIncrease <= 0)
        {
            Highscore++;
            timeToNextScoreIncrease = 5.0f;
        }

        curSpawnCooldown -= Time.deltaTime;

        if(curSpawnCooldown <= 0.0f)
        {
            curSpawnCooldown = maxSpawnCooldown;
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
            Highscore += enemy.score;

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
