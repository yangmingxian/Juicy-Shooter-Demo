using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using Sirenix.OdinInspector;
using Animancer;
using MoreMountains.Feedbacks;
using DG.Tweening;
public class GameController : MonoBehaviour
{

    GameObject PlayerObj;
    private void Awake()
    {
        PlayerObj = GameObject.FindWithTag("Player");
    }
    private void Start()
    {
        UpdateFramerate();
        InvokeRepeating(nameof(StartSpawn), 0, SpawnCoolDown);

    }

    #region Global Game Control
    [Tooltip("1:enable 0:disable 2:half")]
    [SerializeField] int vSyncCount = 1;
    [SerializeField] int framerate = 60;
    [Button]
    void UpdateFramerate()
    {
        QualitySettings.vSyncCount = vSyncCount;
        if (vSyncCount == 0)
        { Application.targetFrameRate = framerate; }
    }

    #endregion



    #region EnemySpawn
    [SerializeField] int minEnemyLimit = 10;
    [SerializeField] float SpawnCoolDown = 5f;
    readonly WaitForSeconds SpawnCoolDownSmall = new(0.3f);
    [SerializeField] Vector2Int SpawnNumRange = new(3, 5);
    [SerializeField] Vector2 SpawnDistRange = new(2, 10);
    public static List<EnemyController> enemyList = new();

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
        var summonFX = GameObject.Instantiate(summonFXPrefab, pos, enemyPrefab.transform.rotation);
        summonFX.PlayFX();
        yield return new WaitForSeconds(spawnDelay);
        var enemyObj = GameObject.Instantiate(enemyPrefab, pos, enemyPrefab.transform.rotation);
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
    public void GameOver()
    {

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
}
