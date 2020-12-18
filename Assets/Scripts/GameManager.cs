using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Serializable]
    public struct PlayerRangeWeapon
    {
        public WeaponPowerupController.GameWeapons rangeWeapon;
        public int maxAmmo;
        public int currentAmmo;
        public float fireRate;
        public float bulletSpawnOffsetX;
        public float bulletSpawnOffsetY;
    }


    [Serializable]
    public struct PlayerMeleeWeapon
    {
        public WeaponPowerupController.GameWeapons meleeWeapon;
        public bool isWeaponLethal;
    }

    [Serializable]
    public struct SpritesForWeapon
    {
        public WeaponPowerupController.GameWeapons weapon;
        public Sprite weaponSprite;
    }

    #region Inspector Variables

    [SerializeField] int numberOfEnemies;
    [SerializeField] GameManager.PlayerMeleeWeapon[] meleeWeapons;
    [SerializeField] GameManager.PlayerRangeWeapon[] rangeWeapons;
    [SerializeField] GameManager.SpritesForWeapon[] spritesForWeapons;

    #endregion

    #region Private Variables

    // bool _isPlayerDead = false;

    #endregion

    #region Properties Accessors

    // public bool IsPlayerDead
    // {
    //     get
    //     {
    //         return _isPlayerDead;
    //     }

    //     set
    //     {
    //         _isPlayerDead = value;
    //     }
    // }

    #endregion


    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        this.numberOfEnemies = FindObjectsOfType<EnemyMovementAI>().Length;
    }

    public PlayerMeleeWeapon GetMeleeWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        PlayerMeleeWeapon selectedWeapon = new PlayerMeleeWeapon();

        foreach (var gameWeapon in meleeWeapons)
        {
            if (gameWeapon.meleeWeapon == weapon)
            {
                selectedWeapon = gameWeapon;
            }
        }

        return selectedWeapon;
    }

    public PlayerRangeWeapon GetRangeWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        PlayerRangeWeapon selectedWeapon = new PlayerRangeWeapon();

        foreach (var gameWeapon in rangeWeapons)
        {
            if (gameWeapon.rangeWeapon == weapon)
            {
                selectedWeapon = gameWeapon;
            }
        }

        return selectedWeapon;
    }

    public Sprite GetspriteForWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        Sprite sprite = Sprite.Create(null,Rect.zero,Vector2.zero);

        foreach (var spriteForweapon in spritesForWeapons)
        {
            if (spriteForweapon.weapon == weapon)
            {
                sprite = spriteForweapon.weaponSprite;
            }
        }

        return sprite;
    }

    public void EnemyKilled()
    {
        numberOfEnemies--;
        if (numberOfEnemies <= 0)
            WinGame();
    }

    public void GameOver()
    {
        // UIManager.UIManagerInstance.ShowGameOverMenu();
        SceneManager.LoadScene(3);
    }

    public void WinGame()
    {
        // UIManager.UIManagerInstance.ShowGameOverMenu();
        
        SceneManager.LoadScene(4);
    }
}
