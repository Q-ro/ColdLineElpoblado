using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUpPowerUpController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    bool _powerupInRange = false;
    WeaponPowerupController _weaponToPickUp;

    void LateUpdate()
    {
        if (Input.GetButtonDown("A_Button") && !playerController.IsPlayerHoldingWeapon() && _powerupInRange)
        {
            _powerupInRange = false;
            playerController.SetPlayerWeapon(_weaponToPickUp.WeaponType);
            _weaponToPickUp.gameObject.SetActive(false);
            _weaponToPickUp = null;
        }

    }

    //When player steps on it to pick it up
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PowerUp")
        {
            if (this.gameObject.tag == "Player")
            {
                if (other.GetComponent<WeaponPowerupController>().IsBeingTossed)
                {
                    if (!other.GetComponent<WeaponPowerupController>().IsPlayerToss)
                    {

                    }
                }
                else
                {
                    _powerupInRange = true;
                    _weaponToPickUp = other.GetComponent<WeaponPowerupController>();
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "PowerUp")
        {
            if (this.gameObject.tag == "Player")
            {
                _powerupInRange = false;
                _weaponToPickUp = null;
            }
        }
    }

}
