using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class Encounter 
{
    public Enemy enemy;
    public float spawnWeight = 1.0f;
    public float spawnCooldown = 2.0f;
    public float multiplier = 1.0f;
    public int minSpawns = 1;
    public int maxSpawns = 1;
    public int numMinSpawnsPerLoop = 0;
    public int numMaxSpawnsPerLoop = 0;

    public float Chance { get; set;}
    public float AccumulatedChance { get; set; }
}

public class Level : MonoBehaviour 
{
    [SerializeField]
    private float lvlDuration = 30.0f;

    [SerializeField]
    private float enemyLoopIncrease = 0.25f;

    [ReadOnly]
    [SerializeField]
    private float enemyStrength = 1.0f;

    [ReadOnly]
    [SerializeField]
    private float curLvlDuration;

    [ReadOnly]
    [SerializeField]
    private float curSpawnCooldown;

    [SerializeField]
    public List<Encounter> encounters;

    private int loop;

    public bool CanSpawnEnemy   { get { return curSpawnCooldown <= 0.0f; } }
    public bool LevelFinished   { get { return curLvlDuration >= lvlDuration; } }

    private Encounter GetRandomEncounter()
    {
        Debug.Assert(CanSpawnEnemy);

        float randVal = UnityEngine.Random.value;

        for (int i = 0; i < encounters.Count; i++)
        {
            if (randVal <= encounters[i].AccumulatedChance)
            {
                return encounters[i];
            }
        }

        Debug.Assert(false);
        return null;
    }

    void Start()
    {
        ComputeChances();
    }

    public void StartLevel(int loop)
    {
        this.loop = loop;
        curLvlDuration = 0.0f;
        curSpawnCooldown = 0.0f;

        enemyStrength = 1.0f + loop * enemyLoopIncrease;
    }

    public void UpdateLevel(float deltaTime)
    {
        if (!LevelFinished)
        {
            curLvlDuration += deltaTime;

            if (CanSpawnEnemy)
            {
                Encounter newEncounter = GetRandomEncounter();
                curSpawnCooldown = newEncounter.spawnCooldown;


                int min = newEncounter.minSpawns + newEncounter.numMinSpawnsPerLoop * loop;
                int max = newEncounter.maxSpawns + newEncounter.numMaxSpawnsPerLoop * loop;

                int numSpawns = UnityEngine.Random.Range(min, max);

                float numPlayerDifficultyIncrease = 1.0f  + (float)(GameManager.Instance.RegisteredPlayers - 1) * 0.5f;
             
                numPlayerDifficultyIncrease = Mathf.Max(numPlayerDifficultyIncrease, 1.0f);
                Debug.Log(numPlayerDifficultyIncrease);

                for (int i = 0; i < numSpawns; i++)
                {
                    float tmpDifficulty = enemyStrength * newEncounter.multiplier;
                    tmpDifficulty *= numPlayerDifficultyIncrease;

                    SpawnMonster(newEncounter.enemy, tmpDifficulty);
                }
            }

            else
            {
                curSpawnCooldown -= Time.deltaTime;
            }

        }
    }

    private void SpawnMonster(Enemy prefab, float enemyModifier)
    {
        GameObject createdEnemy = Instantiate(prefab.gameObject);

        createdEnemy.transform.position = GameManager.Instance.GetRandomSpawnPosition();

        createdEnemy.GetComponent<Enemy>().Init(enemyModifier);
    }

    private void ComputeChances()
    {
        float accumulatedWeights = 0.0f;

        for (int i = 0; i < encounters.Count; i++)
        {
            accumulatedWeights += encounters[i].spawnWeight;
        }

        for (int i = 0; i < encounters.Count; i++)
        {
            int previous = Mathf.Max(i - 1, 0);

            float chance = encounters[i].spawnWeight / accumulatedWeights;

            encounters[i].AccumulatedChance = chance + encounters[previous].AccumulatedChance;
            encounters[i].Chance = chance;
        }


        encounters.Sort( (x,y) => (int)((x.AccumulatedChance * 1000.0f) - (y.AccumulatedChance * 1000.0f)) );
    }
}