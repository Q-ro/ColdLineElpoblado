using System;
using System.Collections;
using System.Collections.Generic;
using GameActors;
using UnityEngine;

public class PlayerRangeAttack : MonoBehaviour
{

    #region Inspector Variables

    // [SerializeField] GameManager.PlayerRangeWeapon[] playerRangeWeapons;
    [SerializeField] PlayerController playerController;
    // [SerializeField] Rigidbody2D bulletPrefab;

    #endregion

    #region Propierty Accesors
    GameManager.PlayerRangeWeapon _currentlyHeldWeapon;

    bool _isFiring = false;

    #endregion

    void Update()
    {

        if (playerController.CurrentAnimationState == GameActorAnimationStates.Dead || playerController.CurrentAnimationState == GameActorAnimationStates.Stunned)
        {
            return;
        }

        if (Input.GetButton("B_Button"))
        {
            if (_currentlyHeldWeapon.rangeWeapon != WeaponPowerupController.GameWeapons.None)
            {
                if (!_isFiring)
                {
                    ShootWeapon();
                }
            }
        }
        if (Input.GetButtonDown("A_Button") && playerController.IsPlayerHoldingWeapon() && !playerController.IsDead && !playerController.IsStunned)
        {
            if (_currentlyHeldWeapon.rangeWeapon != WeaponPowerupController.GameWeapons.None)
            {
                ThrowWeapon();
            }
        }
    }

    public void SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons weapon)
    {

        _currentlyHeldWeapon = GameManager.Instance.GetRangeWeapon(weapon);

        if (weapon != WeaponPowerupController.GameWeapons.None)
        {
            if (weapon == WeaponPowerupController.GameWeapons.Pistol)
            {
                playerController.HasPistol = true;
                playerController.HasSMG = false;
                playerController.HasMachineGun = false;
            }
            else if (weapon == WeaponPowerupController.GameWeapons.SMG)
            {
                playerController.HasSMG = true;
                playerController.HasPistol = false;
                playerController.HasMachineGun = false;
            }
            else if (weapon == WeaponPowerupController.GameWeapons.MachineGun)
            {
                playerController.HasMachineGun = true;
                playerController.HasPistol = false;
                playerController.HasSMG = false;
            }
        UIManager.UIManagerInstance.SetSelectedWeapon(_currentlyHeldWeapon.rangeWeapon, _currentlyHeldWeapon.maxAmmo, true);
        }
        else
        {
            playerController.HasPistol = false;
            playerController.HasSMG = false;
            playerController.HasMachineGun = false;

            UIManager.UIManagerInstance.SetSelectedWeapon(_currentlyHeldWeapon.rangeWeapon, _currentlyHeldWeapon.maxAmmo, false);
        }

    }

    public void ShootWeapon()
    {
        if (_currentlyHeldWeapon.currentAmmo - 1 >= 0)
        {
            StartCoroutine(PlayerFire());
            _currentlyHeldWeapon.currentAmmo--;

            UIManager.UIManagerInstance.UpdateAmmoCount(_currentlyHeldWeapon.maxAmmo, _currentlyHeldWeapon.currentAmmo, true);
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
                bulletClone.GetComponent<BulletController>().IsPlayerBullet = true;
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

            weaponPowerup.SetToss(true, weapon, temp.transform.right * 2.5f);
        }
    }


    public void ThrowWeapon()
    {
        SpawnTossedWeapon(_currentlyHeldWeapon.rangeWeapon);

        //Set weapon to none because we are discarting the weapon
        SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons.None);

        playerController.ResetPlayerWeapon();
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
