using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    // public static ObjectPooler BulletPoolInstance;

    // public static BulletPoolController BulletPoolInstance
    // {
    //     get
    //     {
    //         if (!_bulletPoolInstance)
    //         {
    //             _bulletPoolInstance = FindObjectOfType(typeof(BulletPoolController)) as BulletPoolController;

    //             if (!_bulletPoolInstance)
    //             {
    //                 var obj = new GameObject("BulletPool");
    //                 _bulletPoolInstance = obj.AddComponent<BulletPoolController>();
    //                 DontDestroyOnLoad(obj);
    //             }
    //         }

    //         return _bulletPoolInstance;
    //     }
    // }

    #region Inspector Variables

    [SerializeField] GameObject objectToPool;
    [SerializeField] int maxPoolSize;

    #endregion

    List<GameObject> _objectPool;

    // void Awake()
    // {
    //     BulletPoolInstance = this;
    // }

    // Use this for initialization
    protected void Start()
    {
        _objectPool = new List<GameObject>();
        for (int i = 0; i < maxPoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.transform.SetParent(this.transform);
            obj.SetActive(false);
            _objectPool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _objectPool.Count; i++)
        {
            if (!_objectPool[i].activeInHierarchy)
            {
                return _objectPool[i];
            }
        }
        return null;
    }
}
