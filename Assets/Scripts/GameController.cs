using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MoreMountains.Feedbacks;
using DG.Tweening;
using UnityEngine.Pool;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [ReadOnly] public static bool isPaused;
    GameObject PlayerObj;


    private void Awake()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        enemyList.Clear();
    }
    private void Start()
    {
        isPaused = false;
        PlayerStatus.isDead = false;
        isGeneratingEnemy = false;
        UpdateFramerate();
        // InvokeRepeating(nameof(StartSpawn), 0, SpawnCoolDown);
    }

    #region Global Game Control
    [Tooltip("1:enable 0:disable 2:half")]
    public static int vSyncCount = 1;
    public static int framerate = 144;
    [Button]
    public static void UpdateFramerate()
    {
        QualitySettings.vSyncCount = vSyncCount;
        if (vSyncCount == 0)
        { Application.targetFrameRate = framerate; }
    }

    #endregion



    #region EnemySpawn
    [SerializeField] int minEnemyLimit = 10;
    // [SerializeField] float SpawnCoolDown = 5f;
    readonly WaitForSeconds SpawnCoolDownSmall = new(0.3f);
    [SerializeField] Vector2Int SpawnNumRange = new(3, 5);
    [SerializeField] Vector2 SpawnDistRange = new(2, 10);
    public static List<EnemyController> enemyList = new();

    [SerializeField] bool isGeneratingEnemy;

    private void Update()
    {
        if (!isGeneratingEnemy && enemyList.Count < minEnemyLimit)
        {
            StartSpawn();
            // InvokeRepeating(nameof(StartSpawn), 0, SpawnCoolDown);
            isGeneratingEnemy = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

    }

    [Button]
    void StartSpawn()
    {
        if (enemyList.Count < minEnemyLimit)
        {
            StopCoroutine(SpawnEnemiesCoroutine());
            StartCoroutine(SpawnEnemiesCoroutine());
        }
    }

    IEnumerator SpawnEnemiesCoroutine()
    {
        var spawnNum = Random.Range(SpawnNumRange.x, SpawnNumRange.y);
        for (int i = 0; i < spawnNum; i++)
        {
            var dist = Random.Range(SpawnDistRange.x, SpawnDistRange.y);
            var angle = Random.Range(0, 360f);
            Vector2 pos = PlayerObj.transform.position + Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right * dist;
            Summon(pos);
            yield return SpawnCoolDownSmall;
        }
        isGeneratingEnemy = false;
    }


    [SerializeField] SummonFX summonFXPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnDelay = 1f;

    [Button]
    public void Summon(Vector3 pos)
    {

        StartCoroutine(SummonCoroutine(pos));
    }

    IEnumerator SummonCoroutine(Vector3 pos)
    {
        var summonFXObj = ObjectPoolManager.GetObject(summonFXPrefab.gameObject);

        summonFXObj.transform.SetPositionAndRotation(pos, enemyPrefab.transform.rotation);
        var summonFX = summonFXObj.GetComponent<SummonFX>();
        summonFX.PlayFX();
        yield return new WaitForSeconds(spawnDelay);

        var enemyObj = ObjectPoolManager.GetObject(enemyPrefab);
        enemyObj.transform.SetPositionAndRotation(pos, enemyPrefab.transform.rotation);
        var enemy = enemyObj.GetComponent<EnemyController>();
        enemy.Appear(0.2f);
        enemyList.Add(enemy);
        yield return new WaitForSeconds(0.2f);
        summonFX.circleSpriteRenderer.DOColor(Color.clear, 0.5f);
        enemy.active = true;
    }
    #endregion

    #region Game Routine
    [SerializeField] MMF_Player gameOverFeedback;
    [SerializeField] MMF_Player pauseGameFeedback;
    [SerializeField] MMF_Player resumeGameFeedback;



    public void GameOver()
    {
        gameOverFeedback.PlayFeedbacks();
    }

    public void PauseGame()
    {
        if (isPaused || !pauseGameFeedback)
            return;
        pauseGameFeedback.PlayFeedbacks();

        Time.timeScale = 0;
        isPaused = true;
    }
    public void ResumeGame()
    {
        if (!isPaused || !resumeGameFeedback)
            return;
        resumeGameFeedback.PlayFeedbacks();
        Time.timeScale = 1;
        isPaused = false;
    }



    #endregion


    private void OnEnable()
    {
        PlayerStatus.PlayerDie += GameOver;
    }
    private void OnDisable()
    {
        PlayerStatus.PlayerDie -= GameOver;
    }
    private void OnDestroy()
    {
        DOTween.KillAll();
        StopAllCoroutines();
    }

    [Button]
    public void StopGenEnemies()
    {
        StopAllCoroutines();
    }
}
