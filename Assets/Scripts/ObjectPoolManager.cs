using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;
public class ObjectPoolManager : MonoBehaviour
{
    // public GameObject bulletPrefab;
    // public GameObject shellPrefab;
    // public GameObject bulletExplodePrefab;
    // public GameObject enemyPrefab;
    // public GameObject spawnVFXPrefab;
    // public GameObject explosionPrefab;


    // public static UnityEngine.Pool.ObjectPool<GameObject> bulletPool;
    // public static UnityEngine.Pool.ObjectPool<GameObject> shellPool;
    // public static UnityEngine.Pool.ObjectPool<GameObject> bulletExplodePool;

    // public static UnityEngine.Pool.ObjectPool<GameObject> enemyPool;
    // public static UnityEngine.Pool.ObjectPool<GameObject> spawnVFXPool;

    // public static UnityEngine.Pool.ObjectPool<GameObject> explosionPool;

    // private void Awake()
    // {
    //     bulletPool = new ObjectPool<GameObject>(
    //            () => GameObject.Instantiate(bulletPrefab),
    //            (obj) => obj.SetActive(true),
    //            (obj) => obj.SetActive(false),
    //            (obj) => GameObject.Destroy(obj)
    //        );
    //     shellPool = new ObjectPool<GameObject>(
    //         () => GameObject.Instantiate(shellPrefab),
    //         (obj) => obj.SetActive(true),
    //         (obj) => obj.SetActive(false),
    //         (obj) => GameObject.Destroy(obj)
    //     );
    //     bulletExplodePool = new ObjectPool<GameObject>(
    //      () => GameObject.Instantiate(bulletExplodePrefab),
    //      (obj) => obj.SetActive(true),
    //      (obj) => obj.SetActive(false),
    //      (obj) => GameObject.Destroy(obj)
    //  );
    //     enemyPool = new ObjectPool<GameObject>(
    //      () => GameObject.Instantiate(enemyPrefab),
    //      (obj) => obj.SetActive(true),
    //      (obj) => obj.SetActive(false),
    //      (obj) => GameObject.Destroy(obj)
    //  );
    //     spawnVFXPool = new ObjectPool<GameObject>(
    //      () => GameObject.Instantiate(spawnVFXPrefab),
    //      (obj) => obj.SetActive(true),
    //      (obj) => obj.SetActive(false),
    //      (obj) => GameObject.Destroy(obj)
    //  );
    //     explosionPool = new ObjectPool<GameObject>(
    //      () => GameObject.Instantiate(explosionPrefab),
    //      (obj) => obj.SetActive(true),
    //      (obj) => obj.SetActive(false),
    //      (obj) => GameObject.Destroy(obj)
    //  );
    // }
    private void Start()
    {
        objectPool.Clear();
    }

    // private static ObjectPool instance;
    private static Dictionary<string, Queue<GameObject>> objectPool = new();
    private static GameObject pool;


    // public static ObjectPool Instance
    // {
    //     get
    //     {
    //         if (instance == null)
    //         {
    //             // ChangeObjectPool?.Invoke();
    //             instance = new ObjectPool();
    //         }
    //         return instance;
    //     }
    // }

    public static GameObject GetObject(GameObject prefab)
    {
        GameObject _object;
        if (!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);
            if (pool == null)
                pool = new GameObject("ObjectPool");
            var childTrans = pool.transform.Find(prefab.name + "Pool");
            if (!childTrans)
            {
                var child = new GameObject(prefab.name + "Pool");
                child.transform.SetParent(pool.transform);
                childTrans = child.transform;
            }
            _object.transform.SetParent(childTrans);
        }
        _object = objectPool[prefab.name].Dequeue();
        if (!_object)
            return null;
        _object.SetActive(true);
        return _object;
    }
    public static void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(_name))
            objectPool.Add(_name, new Queue<GameObject>());
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }

    private void OnDestroy()
    {
        objectPool.Clear();
    }




}