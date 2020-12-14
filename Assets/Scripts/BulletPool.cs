using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : ObjectPooler
{

    private static BulletPool _instance;

    public static BulletPool Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(BulletPool)) as BulletPool;

                if (!_instance)
                {
                    var obj = new GameObject("BulletPool");
                    _instance = obj.AddComponent<BulletPool>();
                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }
    }

    new void Start()
    {
        _instance = this;

        base.Start();
    }

    // protected BulletPool() { } // guarantee this will be always a singleton only - can't use the constructor!

}
