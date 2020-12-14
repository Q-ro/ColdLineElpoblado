using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPoolController : MonoBehaviour
{

    public static BulletPoolController BulletPoolInstance;

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

    [SerializeField] List<GameObject> objectPool;
    [SerializeField] GameObject objectToPool;
    [SerializeField] int maxPoolSize;

    #endregion

    void Awake()
    {
        BulletPoolInstance = this;
    }

    // Use this for initialization
    void Start()
    {
        objectPool = new List<GameObject>();
        for (int i = 0; i < maxPoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(objectToPool);
            obj.SetActive(false);
            objectPool.Add(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }
        return null;
    }
}
