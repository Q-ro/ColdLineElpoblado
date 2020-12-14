using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class WeaponPowerupController : MonoBehaviour
{
    public enum GameWeapons
    {
        None,
        Pistol,
        SMG,
        MachineGun,
        Punch,
        Knife,
        Sword
    }

    #region Inspector Variables

    // [SerializeField] GameWeapons meleeWeaponType;
    [SerializeField] GameWeapons weaponType;
    [SerializeField] float tossSpeed = 0.4f;
    [SerializeField] BoxCollider2D boxCollider;
    [SerializeField] CircleCollider2D circleCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Rigidbody2D rigidBody2D;

    #endregion

    #region Private Variables

    bool isPlayerToss = false;
    bool isBeingTossed = false;

    #endregion

    public bool IsPlayerToss
    {
        get
        {
            return isPlayerToss;
        }

        set
        {
            isPlayerToss = value;
        }
    }

    public bool IsBeingTossed
    {
        get
        {
            return isBeingTossed;
        }

        set
        {
            isBeingTossed = value;
        }
    }

    public GameWeapons WeaponType
    {
        get
        {
            return weaponType;
        }
        set
        {
            weaponType = value;
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetSprite(Sprite weaponSprite)
    {
        if (spriteRenderer == null)
            spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = weaponSprite;
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Wall" || collisionInfo.gameObject.tag == "Obstacle")
        {
            StopMoving();
        }
        // else if(collisionInfo.gameObject.tag == "Obstacle")
        //  Physics2D.IgnoreCollision(collisionInfo.collider, boxCollider); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        //if a player's bullet hits an enemy
        if (isBeingTossed && other.gameObject.tag == "Enemy")
        {
            if (weaponType != GameWeapons.Knife)
                other.gameObject.GetComponent<EnemyMovementAI>().EnemyHit(false);
            else
                other.gameObject.GetComponent<EnemyMovementAI>().EnemyHit(true);

            StopMoving();
        }
    }

    public void SetToss(bool isPlayerToss, GameWeapons weapon, Vector3 velocity)
    {
        gameObject.SetActive(true);
        boxCollider.enabled = isPlayerToss ? true : false;
        isBeingTossed = isPlayerToss ? true : false;
        IsPlayerToss = isPlayerToss;
        WeaponType = weapon;
        SetSprite(GameManager.Instance.GetspriteForWeapon(weapon));
        rigidBody2D.velocity = velocity;
    }

    void StopMoving()
    {
        rigidBody2D.velocity = Vector3.zero;
        isBeingTossed = false;
        boxCollider.enabled = false;
    }
}
