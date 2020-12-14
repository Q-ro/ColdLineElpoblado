using System;
using System.Collections;
using System.Collections.Generic;
using GameActors;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMeleeAttack : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] PlayerController playerController;

    #endregion

    #region Private Variables
    GameManager.PlayerMeleeWeapon _currentlyHeldWeapon;

    #endregion

    void Update()
    {
        if (Input.GetButton("B_Button"))
        {
            if (_currentlyHeldWeapon.meleeWeapon != WeaponPowerupController.GameWeapons.None && !playerController.IsMeleeAttaking)
            {
                playerController.SetAnimationState(GameActorAnimationStates.MeleeAttacking);
            }
        }
        if (Input.GetButtonDown("A_Button") && playerController.IsPlayerHoldingWeapon() && !playerController.IsMeleeAttaking && !playerController.IsDead && !playerController.IsStunned)
        {
            if (_currentlyHeldWeapon.meleeWeapon != WeaponPowerupController.GameWeapons.None)
            {
                ThrowWeapon();
            }
        }
    }

    private void ThrowWeapon()
    {
        SpawnTossedWeapon(_currentlyHeldWeapon.meleeWeapon);

        //Set weapon to none because we are discarting the weapon
        SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons.Punch);

        playerController.ResetPlayerWeapon();
    }

    public void SpawnTossedWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        GameObject temp = WeaponPowerupPool.Instance.GetPooledObject();
        WeaponPowerupController weaponPowerup;

        if (weaponPowerup = temp.GetComponent<WeaponPowerupController>())
        {
            temp.transform.position = this.transform.TransformPoint(new Vector3(0.05f,
                                                            0,
                                                            0));
            temp.transform.rotation = this.transform.rotation;

            weaponPowerup.SetToss(true, weapon, temp.transform.right * 2.5f);
        }
    }

    public void SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        _currentlyHeldWeapon = GameManager.Instance.GetMeleeWeapon(weapon);
        UIManager.UIManagerInstance.SetSelectedWeapon(_currentlyHeldWeapon.meleeWeapon, 0, false);

        if (weapon != WeaponPowerupController.GameWeapons.None)
        {
            if (weapon == WeaponPowerupController.GameWeapons.Punch)
            {
                playerController.HasMelee = false;
            }
            else if (weapon == WeaponPowerupController.GameWeapons.Knife)
            {
                playerController.HasMelee = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyMovementAI>().EnemyHit(_currentlyHeldWeapon.isWeaponLethal);
            // var contact = 
            // Quaternion.FromToRotation(Vector3.up, contact.normal);
        }
    }
}
