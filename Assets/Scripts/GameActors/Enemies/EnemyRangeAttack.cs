using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour
{


    #region Inspector Variables

    // [SerializeField] GameManager.PlayerRangeWeapon[] playerRangeWeapons;
    [SerializeField] EnemyMovementAI enemyController;
    // [SerializeField] Rigidbody2D bulletPrefab;

    #endregion

    #region Properties Accesors
    GameManager.PlayerRangeWeapon _currentlyHeldWeapon;

    bool _isFiring = false;

    #endregion

    public void SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons weapon)
    {

        _currentlyHeldWeapon = GameManager.Instance.GetRangeWeapon(weapon);
        // UIManager.UIManagerInstance.SetSelectedWeapon(_currentlyHeldWeapon.rangeWeapon, _currentlyHeldWeapon.maxAmmo, true);

        if (weapon != WeaponPowerupController.GameWeapons.None)
        {
            if (weapon == WeaponPowerupController.GameWeapons.Pistol)
            {
                enemyController.HasPistol = true;
                enemyController.HasSMG = false;
                enemyController.HasMachineGun = false;
            }
            else if (weapon == WeaponPowerupController.GameWeapons.SMG)
            {
                enemyController.HasSMG = true;
                enemyController.HasPistol = false;
                enemyController.HasMachineGun = false;
            }
            else if (weapon == WeaponPowerupController.GameWeapons.MachineGun)
            {
                enemyController.HasMachineGun = true;
                enemyController.HasPistol = false;
                enemyController.HasSMG = false;
            }
        }
        else
        {
            enemyController.HasPistol = false;
            enemyController.HasSMG = false;
            enemyController.HasMachineGun = false;
        }

    }

    public void ShootWeapon()
    {
        if (_isFiring)
            return;

        if (_currentlyHeldWeapon.currentAmmo - 1 >= 0)
        {
            StartCoroutine(PlayerFire());
            // _currentlyHeldWeapon.currentAmmo--;

            // UIManager.UIManagerInstance.UpdateAmmoCount(_currentlyHeldWeapon.maxAmmo, _currentlyHeldWeapon.currentAmmo, true);
            // UIManager.UIManagerInstance.UpdateScore(5);

            GameObject bulletClone;
            if (bulletClone = BulletPool.Instance.GetPooledObject())
            {
                bulletClone.transform.position = this.transform.TransformPoint(new Vector3(_currentlyHeldWeapon.bulletSpawnOffsetX,
                                                             _currentlyHeldWeapon.bulletSpawnOffsetY,
                                                             0));

                bulletClone.transform.rotation = this.transform.rotation;
                bulletClone.SetActive(true);
                bulletClone.GetComponent<Rigidbody2D>().velocity = bulletClone.transform.right * 2.5f;
                bulletClone.GetComponent<BulletController>().IsPlayerBullet = false;
            }

            //TODO: Playsound
            SoundManager.Instance.PlayGunshotSound();
        }
        else
        {
            //TODO: Make sure to do something if the weapon runs out of bullets
            //Play sound
            return;
        }
    }

    public void SpawnTossedWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        GameObject temp = WeaponPowerupPool.Instance.GetPooledObject();
        WeaponPowerupController weaponPowerup;

        if (weaponPowerup = temp.GetComponent<WeaponPowerupController>())
        {
            temp.transform.position = this.transform.TransformPoint(new Vector3(_currentlyHeldWeapon.bulletSpawnOffsetX,
                                                _currentlyHeldWeapon.bulletSpawnOffsetY,
                                                0));

            temp.transform.rotation = this.transform.rotation;
            weaponPowerup.SetToss(false, weapon, Vector3.zero);
        }
    }


    public void ThrowWeapon()
    {
        SpawnTossedWeapon(_currentlyHeldWeapon.rangeWeapon);

        //Set weapon to none because we are discarting the weapon
        SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons.None);

        enemyController.ResetEnemyWeapon();
    }

    IEnumerator PlayerFire()
    {
        _isFiring = true;

        float delayCounter = _currentlyHeldWeapon.fireRate;
        while (delayCounter > 0)
        {
            delayCounter -= 0.1f;
            yield return null;
        }

        _isFiring = false;
    }


}
