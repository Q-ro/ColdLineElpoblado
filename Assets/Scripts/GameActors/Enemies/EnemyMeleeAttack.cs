using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMeleeAttack : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] EnemyMovementAI enemyController;

    #endregion

    #region Private Variables

    GameManager.PlayerMeleeWeapon _currentlyHeldWeapon;

    #endregion

    public void ThrowWeapon()
    {
        SpawnTossedWeapon(_currentlyHeldWeapon.meleeWeapon);

        //Set weapon to none because we are discarting the weapon
        SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons.Punch);

        enemyController.ResetEnemyWeapon();
    }

    public void SpawnTossedWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        GameObject temp = WeaponPowerupPool.Instance.GetPooledObject();
        WeaponPowerupController weaponPowerup;

        if (weaponPowerup = temp.GetComponent<WeaponPowerupController>())
        {
            temp.transform.position = this.transform.TransformPoint(new Vector3(0,
                                                0,
                                                0));

            temp.transform.rotation = this.transform.rotation;
            weaponPowerup.SetToss(false, weapon, Vector3.zero);
        }
    }


    public void SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        _currentlyHeldWeapon = GameManager.Instance.GetMeleeWeapon(weapon);
        // UIManager.UIManagerInstance.SetSelectedWeapon(_currentlyHeldWeapon.meleeWeapon, 0, false);

        if (weapon != WeaponPowerupController.GameWeapons.None)
        {
            if (weapon == WeaponPowerupController.GameWeapons.Punch)
            {
                enemyController.HasMelee = false;
            }
            else if (weapon == WeaponPowerupController.GameWeapons.Knife)
            {
                enemyController.HasMelee = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().PlayerHit(_currentlyHeldWeapon.isWeaponLethal);
        }
    }
}
