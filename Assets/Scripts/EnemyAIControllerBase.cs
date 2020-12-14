using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIControllerBase : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] private float moveTime = 0.1f;           //Time it will take object to move, in seconds.
    [SerializeField] private LayerMask blockingLayer;         //Layer on which collision will be checked.

    #endregion

    #region Private Variables

    private Transform _currentTarget; //The current target of the enemy

    private NavMeshAgent _navMesh;
    private bool _isTargetPlayer = false;
    private bool _isInAttackRange = false;
    private BoxCollider2D _boxCollider;      //The BoxCollider2D component attached to this object.
    private Rigidbody2D _rigidBody;               //The Rigidbody2D component attached to this object.
    private float _inverseMoveTime;          //Used to make movement more efficient.

    #endregion


    // Use this for initialization
    protected virtual void Start()
    {
        //Get a component reference to this object's BoxCollider2D
        _boxCollider = GetComponent<BoxCollider2D>();

        //Get a component reference to this object's Rigidbody2D
        _rigidBody = GetComponent<Rigidbody2D>();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        _inverseMoveTime = 1f / moveTime;
    }

    // Update is called once per frame
    void Update()
    {

    }


    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        //Store start position to move from, based on objects current transform position.
        Vector2 start = transform.position;

        // Calculate end position based on the direction parameters passed in when calling Move.
        Vector2 end = start + new Vector2(xDir, yDir);

        //Disable the boxCollider so that linecast doesn't hit this object's own collider.
        _boxCollider.enabled = false;

        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(start, end, blockingLayer);

        //Re-enable boxCollider after linecast
        _boxCollider.enabled = true;

        //Check if anything was hit
        if (hit.transform == null)
        {
            //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
            //StartCoroutine (SmoothMovement (end));

            //Return true to say that Move was successful
            return true;
        }

        //If something was hit, return false, Move was unsuccesful.
        return false;
    }
}
