using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;
public class ObjectPoolManager : MonoBehaviour
{
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

    private void OnEnable()
    {
        objectPool.Clear();
    }


}