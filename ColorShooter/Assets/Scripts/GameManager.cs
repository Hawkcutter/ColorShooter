using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [ReadOnly]
    [SerializeField]
    private int highscore;
    public int Highscore { get { return highscore; } set { SetHighscore(value); } }

    /*
    [ReadOnly]
    [SerializeField]
    private float enemyStrengthFactor = 1.0f;
    public float EnemyStrengthFactor { get { return enemyStrengthFactor; } }
    */
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
    public int debugLevelId = -1;

    [ReadOnly]
    [SerializeField]
    private int curLevelId;
    [ReadOnly]
    [SerializeField]
    private int loop;
    [ReadOnly]
    [SerializeField]
    private float roundTime;

    [Header("Score and Difficulty")]

    [SerializeField]
    private int playerBaseLifes = 10;

    //[SerializeField]
    //private float timeToMaxDiff = 300.0f;

    //private int difficultyPower = 2;

    private float timeToNextScoreIncrease = 5.0f;



    [SerializeField]
    private float upgrChance = 0.0f;
    [SerializeField]
    private float upgrChanceInc = 0.05f;
    [SerializeField]
    private float timeBetweenLevel = 5.0f;
    [ReadOnly]
    [SerializeField]
    private float curTimeBetweenLevel;
    private bool IsBetweenLevels { get { return curTimeBetweenLevel > 0.0f; } }

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
    private ParticleSystem deathParticlesPrefab;

    [SerializeField]
    private GameObject upgradePrefab;

    private UniqueList<Enemy> enemies;
    private UniqueList<Projectile> projectiles;

    [SerializeField]
    private List<Player> players;
    private List<Player> allPlayers;

    private List<int> takenControllers;
    private int registeredPlayers;
    public int RegisteredPlayers { get { return takenControllers.Count; } }
    

    [SerializeField]
    private Level[] level;

    void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Multiple GameManagers detected, destroying: " + this.gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        ColorKey.InitColors(redBulletPrefab, greenBulletPrefab, blueBulletPrefab, yellowBulletPrefab);

        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        playerLayer = LayerMask.NameToLayer(playerLayerName);

        enemies = new UniqueList<Enemy>(10);
        projectiles = new UniqueList<Projectile>(50);

        players = new List<Player>();
        allPlayers = new List<Player>();
        takenControllers = new List<int>();

        //if(ui)
            //ui.UpdateLifes(playerBase.Lifes);

    }

    void Start()
    {
        Reset();
    }


    public void RegisterPlayer(Player player)
    {
        this.players.Add(player);
        noOfPlayers++;
    }
    public void RegisterControllers()
    {
        
        //if(players.Count >= takenControllers.Count)
          Player  player = players[takenControllers.Count];

        
        {
            if (!player.controllerRegistered)
            {
                if (!takenControllers.Contains(1) && Input.GetKeyDown(KeyCode.Joystick1Button7))
                {
                    player.register(1);
                    takenControllers.Add(1);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(2) && Input.GetKeyDown(KeyCode.Joystick2Button7))
                {
                    player.register(2);
                    takenControllers.Add(2);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(3) && Input.GetKeyDown(KeyCode.Joystick3Button7))
                {
                    player.register(3);
                    takenControllers.Add(3);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(4) && Input.GetKeyDown(KeyCode.Joystick4Button7))
                {
                    player.register(4);
                    takenControllers.Add(4);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(5) && Input.GetKeyDown(KeyCode.Joystick5Button7))
                {
                    player.register(5);
                    takenControllers.Add(5);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(6) && Input.GetKeyDown(KeyCode.Joystick6Button7))
                {
                    player.register(6);
                    takenControllers.Add(6);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(7) && Input.GetKeyDown(KeyCode.Joystick7Button7))
                {
                    player.register(7);
                    takenControllers.Add(7);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(8) && Input.GetKeyDown(KeyCode.Joystick8Button7))
                {
                    player.register(8);
                    takenControllers.Add(8);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(9) && Input.GetKeyDown((KeyCode)KeyCode.Joystick7Button7 + 19))
                {
                    player.register(9);
                    takenControllers.Add(9);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(10) && Input.GetKeyDown((KeyCode)KeyCode.Joystick7Button7 + 38))
                {
                    player.register(10);
                    takenControllers.Add(10);
                    player.gameObject.SetActive(true);
                }
                else if (!takenControllers.Contains(11) && Input.GetKeyDown((KeyCode)KeyCode.Joystick7Button7 + 57))
                {
                    player.register(11);
                    takenControllers.Add(11);
                    player.gameObject.SetActive(true);
                }
            }
        }
    }


    public void Reset()
    {


        curLevelId = 0;
        loop = 0;

        upgrChance = 0;
        roundTime = 0.0f;

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

        level[curLevelId].StartLevel(this.loop);
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


    public Vector2 GetRandomSpawnPosition()
    {
        SpawnZone zone = spawnZones[UnityEngine.Random.Range(0, spawnZones.Length)];

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

        timeToNextScoreIncrease -= Time.deltaTime;

        if(timeToNextScoreIncrease <= 0)
        {
            Highscore++;
            timeToNextScoreIncrease = 5.0f;

            if (UnityEngine.Random.value < upgrChance)
            {
                upgrChance = 0.0f;
                SpawnUpgrade();
            }

            else 
            {
                upgrChance += upgrChanceInc;
            }
        }

        if (!IsBetweenLevels)
        {

            int useId = curLevelId;

            if (debugLevelId != -1)
                useId = debugLevelId;

            level[useId].UpdateLevel(Time.deltaTime);
        }


        else
        {
            curTimeBetweenLevel -= Time.deltaTime;
        }

        if (level[curLevelId].LevelFinished)
        {
            this.curLevelId = (curLevelId + 1) % level.Length;

            if (curLevelId == 0)
            {
                this.loop++;
            }

            curTimeBetweenLevel = timeBetweenLevel;

            level[curLevelId].StartLevel(this.loop);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reset();
        }

        if(takenControllers.Count < 4)
            RegisterControllers();
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
            if (manager.CurLife <= 0)
            {

                Highscore += enemy.Score;

                GameObject deathParticlesInstance = Instantiate(deathParticlesPrefab.gameObject);
                deathParticlesInstance.transform.position = enemy.gameObject.transform.position;
               
            }


            OnBaseLifeChanged();
              
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

    public void OnBaseLifeChanged()
    {
        if (ui)
        {
            ui.UpdateLifes(playerBase.Lifes);
        }
    }


}
