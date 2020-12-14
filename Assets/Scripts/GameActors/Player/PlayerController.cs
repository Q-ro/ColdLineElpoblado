using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameActors;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : GameActors.GameActors
{

    #region Inspector Variables

    [SerializeField] float moveSpeed;
    [SerializeField] GameObject playerBodySprite;
    [SerializeField] GameObject meleeHitbox;
    [SerializeField] PlayerMeleeAttack playerMeleeWeapon;
    [SerializeField] PlayerRangeAttack playerRangeAttack;

    #endregion

    // Use this for initialization
    new void Start()
    {
        base.Start();

        playerRangeAttack.SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons.None);
        playerMeleeWeapon.SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons.Punch);

        SetAnimationState(GameActorAnimationStates.Idle);
    }

    // Update is called once per frame
    void Update()
    {

        if (IsDead || IsStunned)
        //if (_currentAnimationState == GameActorAnimationStates.Dead || _currentAnimationState == GameActorAnimationStates.Stunned)
        {
            _rigidBody2D.velocity = Vector3.zero;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        bool moveRight = Input.GetAxisRaw("Horizontal") > 0 ? true : false;
        bool moveLeft = Input.GetAxisRaw("Horizontal") < 0 ? true : false;
        float moveY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        bool moveUp = Input.GetAxisRaw("Vertical") > 0 ? true : false;
        bool moveDown = Input.GetAxisRaw("Vertical") < 0 ? true : false;

        bool _isMoving = moveDown || moveUp || moveLeft || moveRight;

        Vector2 moveDirection = new Vector2(
            moveX,
            moveY
            );

        if (_isMoving)
        {
            GetDirectionFromVector(moveDirection);
            SetAnimationState(GameActorAnimationStates.Walking);
        }
        else
        {
            // if (_currentAnimationState != GameActorAnimationStates.Stunned)
            SetAnimationState(GameActorAnimationStates.Idle);
        }

        if (_canMove)
        {
            // _rigidBody2D.velocity = moveDirection * moveSpeed * Time.deltaTime;
            _rigidBody2D.velocity = moveDirection ;
        }
        else
        {
            _rigidBody2D.velocity = Vector3.zero;
        }

    }

    public void PlayerHit(bool isLethal)
    {
        if (isLethal)
        {
            GameManager.Instance.GameOver();
            SetAnimationState(GameActorAnimationStates.Dead);
        }
        else
            SetAnimationState(GameActorAnimationStates.Stunned);
    }

    public bool IsPlayerHoldingWeapon()
    {
        return _hasMachineGun || _hasPistol || _hasSMG || _hasMelee;
    }

    public void SetPlayerWeapon(WeaponPowerupController.GameWeapons weapon)
    {
        if (GameManager.Instance.GetMeleeWeapon(weapon).meleeWeapon != WeaponPowerupController.GameWeapons.None)
        {
            playerRangeAttack.SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons.None);
            playerMeleeWeapon.SetCurrentMeleeWeapon(weapon);
            return;
        }
        else if (GameManager.Instance.GetRangeWeapon(weapon).rangeWeapon != WeaponPowerupController.GameWeapons.None)
        {
            playerMeleeWeapon.SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons.None);
            playerRangeAttack.SetCurrentRangeWeapon(weapon);
            return;
        }
    }

    public void ResetPlayerWeapon()
    {
        playerRangeAttack.SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons.None);
        playerMeleeWeapon.SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons.Punch);
        SetAnimationState(GameActorAnimationStates.Disarming);
    }
}
