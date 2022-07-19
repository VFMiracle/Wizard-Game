using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Sprite[] enemySprites = new Sprite[3];
    [SerializeField]
    private GameObject[] enemyTypes;

    private static int score = 0, enemiesKilled = 0, enemiesSpawned = 0, currentWave = 1,
        qtdEnmPerWave = 3, qtdMultipleSpawns = 0, qtdEnmPerMltplSpawn = 2,
        waveMult_IncrEnmPerWave = 2, waveMult_IncrMltplSpawns = 3, waveMult_IncrEnmPerMltplSpawn = 5, 
        waveMult_ReducEnmSpawnTime = 1, waveMult_IncrEnmSpd = 2,
        incrQtdEnmPerWave = 2, incrQtdMultipleSpawns = 2, incrQtdEnmPerMltplSpawn = 2;
    private static float enmSpawnTime = 2f, minEnmSpawnTime = 0.2f, reducEnmSpawnTime = 0.075f, 
        currEnmSpeed = 2.5f, maxEnmSpeed = 5.5f, incrEnmSpeed = 0.1f, 
        waveDelay = 3f;
    public static bool isPlayerAlive = true, gamePaused = false;

    public static Player playerRef;
    public static List<Spawner> spawners = new List<Spawner>();

    private void Awake()
    {
        score = 0; enemiesKilled = 0; enemiesSpawned = 0; currentWave = 1;
        qtdEnmPerWave = 3; qtdMultipleSpawns = 0; qtdEnmPerMltplSpawn = 2;
        waveMult_IncrEnmPerWave = 2; waveMult_IncrMltplSpawns = 3; waveMult_IncrEnmPerMltplSpawn = 5;
        waveMult_ReducEnmSpawnTime = 1;
        incrQtdEnmPerWave = 2; incrQtdMultipleSpawns = 2; incrQtdEnmPerMltplSpawn = 2;

        enmSpawnTime = 2f; minEnmSpawnTime = 0.2f; reducEnmSpawnTime = 0.1f;
        waveDelay = 3f;

        isPlayerAlive = true; gamePaused = false;

        enemySprites = new Sprite[3];
        spawners = new List<Spawner>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.Find("Player").GetComponent<Player>();
        foreach (GameObject spw in GameObject.FindGameObjectsWithTag("Spawner"))
            spawners.Add(spw.GetComponent<Spawner>());
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(waveDelay);
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        int qtdMult = qtdMultipleSpawns;
        for (int i = 0; i < qtdEnmPerWave; i++)
        {
            int qtdEnm = 1;
            if(qtdMult > 0)
            {
                if(UnityEngine.Random.Range(0, 2) > 0 || (qtdEnmPerWave - i) == qtdMult)
                {
                    qtdEnm = qtdEnmPerMltplSpawn;
                    qtdMult--;
                }
            }
            for (int j = 0; j < qtdEnm; j++)
            {
                enemiesSpawned++;
                Element.ElementType element = Element.RandomElement();
                int enmTypeIndex = Convert.ToInt32(element);
                Enemy enm = spawners[UnityEngine.Random.Range(0, spawners.Count)].Spawn(enemyTypes[enmTypeIndex]).
                    GetComponent<Enemy>();
                enm.setSpeed(currEnmSpeed);
                yield return new WaitForSeconds(enmSpawnTime);
            }
        }
    }

    public void IncreaseDifficulty()
    {
        enemiesKilled++;
        if (enemiesKilled == enemiesSpawned)
        {
            currentWave++;
            if (currentWave % waveMult_IncrEnmPerWave == 0)
                qtdEnmPerWave += incrQtdEnmPerWave;
            if (currentWave % waveMult_IncrMltplSpawns == 0)
                qtdMultipleSpawns += incrQtdMultipleSpawns;
            if (currentWave % waveMult_IncrEnmPerMltplSpawn == 0)
                qtdEnmPerMltplSpawn += incrQtdEnmPerMltplSpawn;
            if (currentWave % waveMult_ReducEnmSpawnTime == 0 && enmSpawnTime > minEnmSpawnTime)
                enmSpawnTime -= reducEnmSpawnTime;
            if (currentWave % waveMult_IncrEnmSpd == 0 && currEnmSpeed < maxEnmSpeed)
                currEnmSpeed += incrEnmSpeed;
            enemiesKilled = 0;
            enemiesSpawned = 0;
            StartCoroutine(UiManager.DisplayWaveInfo(currentWave));
            StartCoroutine(StartWave());
        }
    }

    //Returns the current global position of the player.
    public static Vector2 getPlayerPosition()
    {
        return playerRef.transform.position;
    }

    //Increases the 'score' by the amount specified in the 'increment' parameter.
    public static void IncreaseScore(int increment)
    {
        score += increment;
        UiManager.UpdateScoreText(score);
    }

    public static void DeactivatePlayerInfo()
    {
        isPlayerAlive = false;
        playerRef = null;
        GameObject.Find("Canvas").GetComponent<UiManager>().ActivateEndText(score);
    }

    public static void PauseGame()
    {
        if (!gamePaused)
        {
            gamePaused = true;
            Time.timeScale = 0;
            GameObject.Find("Canvas").GetComponent<UiManager>().ChangePauseInfoState(true);
        }
        else
        {
            gamePaused = false;
            Time.timeScale = 1;
            GameObject.Find("Canvas").GetComponent<UiManager>().ChangePauseInfoState(false);
        }
    }
}
