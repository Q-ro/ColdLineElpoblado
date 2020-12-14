using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] bool isPlayerBullet = false;

    #endregion

    public bool IsPlayerBullet
    {
        get
        {
            return isPlayerBullet;
        }

        set
        {
            isPlayerBullet = value;
        }
    }

    void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if a player's bullet hits an enemy
        if (isPlayerBullet && other.tag == "Enemy")
        {
            if(other.gameObject.layer != LayerMask.NameToLayer("Dead"))
            {
            other.GetComponent<EnemyMovementAI>().EnemyHit(true);
            gameObject.SetActive(false);
            }
        }
        //if an enemy bullet hits a player
        else if (!isPlayerBullet && other.tag == "Player")
        {
            other.GetComponent<PlayerController>().PlayerHit(true);
            gameObject.SetActive(false);
        }
        else if (other.tag == "Wall")
        {
            gameObject.SetActive(false);
        }
    }

}
