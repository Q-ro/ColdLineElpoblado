using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPowerupPool : ObjectPooler
{

    private static WeaponPowerupPool _instance;

    public static WeaponPowerupPool Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(WeaponPowerupPool)) as WeaponPowerupPool;

                if (!_instance)
                {
                    var obj = new GameObject("BulletPool");
                    _instance = obj.AddComponent<WeaponPowerupPool>();
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

    // public static ObjectPooler WeaponPowerupPoolInstance;

    // protected WeaponPowerupPool () {} // guarantee this will be always a singleton only - can't use the constructor!

}
