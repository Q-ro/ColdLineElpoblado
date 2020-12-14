using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using GameActors;

/// <summary>
/// Defines the behaviour for the enemy movement
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class EnemyMovementAI : GameActors.GameActors
{

    public enum EnemyAIMovementBehaviourType { Stand, Patrol, Roam, SeekPlayerRange, SeekPlayerMeele }

    public enum EnemyAttackType { Melee, Range };

    #region Inspector Variables

    [SerializeField] private Transform[] target;
    [SerializeField] private float updateRatio = 2f;
    [SerializeField] private float waitTimeATWaypoint = 2f;
    [SerializeField] private float meleeDistance = 20f;
    [SerializeField] private float rangeDistance = 5f;
    [SerializeField] private float wanderingRadius = 1.5f;
    [SerializeField] private Path path;
    [SerializeField] private float walkingSpeed = 16f;
    [SerializeField] private float runningSpeed = 36f;
    [SerializeField] private EnemyAIMovementBehaviourType initialBehaviourTypeAI;
    [SerializeField] private EnemyAttackType typeOfAttack;
    [SerializeField] private GameObject enemyBodySprites;


    [SerializeField] private EnemyMeleeAttack enemyMeleeWeapon;
    [SerializeField] private EnemyRangeAttack enemyRangeWeapon;

    [SerializeField] private WeaponPowerupController.GameWeapons startingMeleeWeapon;
    [SerializeField] private WeaponPowerupController.GameWeapons startingRangeWeapon;

    [SerializeField] private float playerSensorRange;
    [SerializeField] private LayerMask raycastCheckLayers;

    #endregion

    #region Private Variables

    EnemyAIMovementBehaviourType _currentBehaviourTypeAI;
    private Seeker _seeker;
    private Transform _currentTarget;
    private bool _pathEnded = false;
    private bool _isTargetPlayer = false;
    private bool _isWaitingAtWaypoint = false;
    private float _distanceToNextWaypoint = 3f;
    private int _currentWaypoint = 0;
    private int _currentPatrolWaypoint = 0;
    Transform _roamPosition;

    #endregion

    #region Properties Accessors

    public bool PathEnded
    {
        get
        {
            return _pathEnded;
        }

        set
        {
            _pathEnded = value;
        }
    }

    #endregion

    // Use this for initialization
    new void Start()
    {
        base.Start();

        _roamPosition = new GameObject().transform;

        _seeker = this.GetComponent<Seeker>();
        _currentBehaviourTypeAI = initialBehaviourTypeAI;

        if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Patrol)
        {
            _currentTarget = target[_currentPatrolWaypoint];

            StartCoroutine(UpdatePath());
        }
        else if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Roam)
            PickRoamPoint();

        SetAnimationState(GameActorAnimationStates.Idle);

        enemyMeleeWeapon.SetCurrentMeleeWeapon(startingMeleeWeapon);
        enemyRangeWeapon.SetCurrentRangeWeapon(startingRangeWeapon);

    }

    private void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            path = p;
            _currentWaypoint = 0;
        }
        else
        {
            Debug.Log("Path had an error ! D:" + p.error);
        }
    }

    IEnumerator UpdatePath()
    {
        if (_currentTarget == null && _currentBehaviourTypeAI != EnemyAIMovementBehaviourType.Patrol)
        {
            yield return false;
        }


        _seeker.StartPath(this.transform.position, _currentTarget.position, OnPathCompleted);


        yield return new WaitForSeconds(1f / updateRatio);
        if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.SeekPlayerMeele || _currentBehaviourTypeAI == EnemyAIMovementBehaviourType.SeekPlayerRange)
            StartCoroutine(UpdatePath());
    }

    internal void EnemyHit(bool isLethal)
    {

        if (IsDead)
            return;

        GraphicEffectsHelper.Instance.Slowmo();

        if (isLethal)
        {
            SetAnimationState(GameActorAnimationStates.Dead);
            UIManager.UIManagerInstance.UpdateScore(1000);
            GameManager.Instance.EnemyKilled();

            this.gameObject.layer = LayerMask.NameToLayer("Dead");
            this.gameObject.tag = "Dead";
        }
        else
            SetAnimationState(GameActorAnimationStates.Stunned);

        if (IsEnemyHoldingWeapon())
        {
            if (_hasMelee)
                enemyMeleeWeapon.ThrowWeapon();
            else
                enemyRangeWeapon.ThrowWeapon();
        }

        GraphicEffectsHelper.Instance.EmitParticles(this.transform.position, Quaternion.Euler(-spritesBodyContainer.transform.rotation.eulerAngles));
    }

    IEnumerator WaitAtWaypoint()
    {
        _isWaitingAtWaypoint = true;

        yield return new WaitForSeconds(waitTimeATWaypoint);

        if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Patrol)
            GoToNextPatrolWaypoint();
        else if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Roam)
            PickRoamPoint();

        _isWaitingAtWaypoint = false;
    }

    void GoToNextPatrolWaypoint()
    {
        if (_currentPatrolWaypoint < target.Length - 1)
        {
            _currentPatrolWaypoint++;
            // Debug.Log("Current Patrol Waypoint is:" + _currentPatrolWaypoint);
        }
        else
        {
            _currentPatrolWaypoint = 0;
        }

        _currentTarget = target[_currentPatrolWaypoint];
        _currentWaypoint = 0;

        StartCoroutine(UpdatePath());
    }

    void Update()
    {
        if (_currentAnimationState == GameActorAnimationStates.Dead || _currentAnimationState == GameActorAnimationStates.Stunned)
            return;

        bool _isMoving = _rigidBody2D.velocity.x > Vector2.zero.x || _rigidBody2D.velocity.y > Vector2.zero.y;

        if (_isMoving)
            SetAnimationState(GameActorAnimationStates.Walking);
        else
            SetAnimationState(GameActorAnimationStates.Idle);
    }

    void FixedUpdate()
    {

        if (_currentAnimationState == GameActorAnimationStates.Dead || _currentAnimationState == GameActorAnimationStates.Stunned)
        {
            _rigidBody2D.velocity = Vector3.zero;
            return;
        }

        if (_currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerMeele || _currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerRange)
            PlayerSensor();

        if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Patrol || _currentBehaviourTypeAI == EnemyAIMovementBehaviourType.SeekPlayerRange || _currentBehaviourTypeAI == EnemyAIMovementBehaviourType.SeekPlayerMeele || _currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Roam)
        {
            if (target == null)
            {
                return;
            }

            if (path == null)
            {
                return;
            }

            if (_currentWaypoint >= path.vectorPath.Count)
            {
                if (_pathEnded)
                {
                    if ((_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Patrol || _currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Roam) && !_isWaitingAtWaypoint)
                    {
                        StartCoroutine(WaitAtWaypoint());
                    }
                    return;
                }


                _pathEnded = true;
                _rigidBody2D.velocity = Vector3.zero;

                return;
            }

            _pathEnded = false;

            Vector3 moveDirection = (path.vectorPath[_currentWaypoint] - transform.position);

            if (_currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerRange || _currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerRange)
                GetDirectionFromVector(moveDirection);
            else
                GetDirectionFromVector(_currentTarget.position - transform.position);

            if (_canMove)
            {
                if (_currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerRange || _currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerRange)
                    _rigidBody2D.velocity = moveDirection.normalized * walkingSpeed * Time.deltaTime;
                else
                    _rigidBody2D.velocity = moveDirection.normalized * runningSpeed * Time.deltaTime;
            }
            else
            {
                _rigidBody2D.velocity = Vector3.zero;
            }


            float distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[_currentWaypoint]);
            float distanceToTarget = Vector3.Distance(transform.position, _currentTarget.position);

            if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.SeekPlayerMeele)
            {
                if (distanceToTarget <= meleeDistance)
                {
                    _pathEnded = true;

                    if (!IsMeleeAttaking)
                        SetAnimationState(GameActorAnimationStates.MeleeAttacking);

                    return;
                }
            }
            else if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.SeekPlayerRange)
            {
                if (distanceToTarget <= rangeDistance)
                {
                    _pathEnded = true;

                    if (UnityEngine.Random.Range(1, 7) - UnityEngine.Random.Range(1, 7) > 4)
                    {
                        enemyRangeWeapon.ShootWeapon();
                    }

                    return;
                }
            }


            if (distanceToWaypoint <= _distanceToNextWaypoint / 100)
            {
                _currentWaypoint++;
                return;
            }
        }
        else if (_currentBehaviourTypeAI == EnemyAIMovementBehaviourType.Stand && !_isWaitingAtWaypoint)
        {
            // _rigidBody2D.MoveRotation(UnityEngine.Random.Range(0, 180));
            StartCoroutine(WaitAtWaypoint());

            float randomX = UnityEngine.Random.Range(-1f, 1f);
            float randomY = UnityEngine.Random.Range(-1f, 1f);

            GetDirectionFromVector(new Vector3(randomX, randomY, 0));
        }
    }

    Vector3 PickRandomPoint()
    {
        var point = UnityEngine.Random.insideUnitSphere * wanderingRadius;

        point.z = 0;
        point += transform.position;
        return point;
    }

    void PickRoamPoint()
    {
        _roamPosition.position = PickRandomPoint();
        _currentTarget = _roamPosition;
        _currentWaypoint = 0;
        StartCoroutine(UpdatePath());
    }

    /// <summary>
    /// Launches a raycast to try and find if the player is nearby
    /// </summary>
    private void PlayerSensor()
    {
        if (_currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerMeele && _currentBehaviourTypeAI != EnemyAIMovementBehaviourType.SeekPlayerRange)
        {

            Debug.DrawRay(spritesBodyContainer.transform.position, spritesBodyContainer.transform.right, Color.green);

            RaycastHit2D hit = Physics2D.Raycast(spritesBodyContainer.transform.position, spritesBodyContainer.transform.right * playerSensorRange, playerSensorRange, raycastCheckLayers);
            if (hit.collider != null)
            {
                Debug.DrawRay(spritesBodyContainer.transform.position, spritesBodyContainer.transform.right, Color.yellow);
                if (hit.collider.tag == "Player")
                {
                    Debug.DrawRay(spritesBodyContainer.transform.position, spritesBodyContainer.transform.right, Color.red);
                    StopAllCoroutines();
                    _isWaitingAtWaypoint = false;
                    if (typeOfAttack == EnemyAttackType.Melee)
                        _currentBehaviourTypeAI = EnemyAIMovementBehaviourType.SeekPlayerMeele;
                    else if (typeOfAttack == EnemyAttackType.Range)
                        _currentBehaviourTypeAI = EnemyAIMovementBehaviourType.SeekPlayerRange;

                    _currentTarget = hit.collider.gameObject.transform;
                    StartCoroutine(UpdatePath());
                }
            }
        }
    }

    public bool IsEnemyHoldingWeapon()
    {
        return _hasMachineGun || _hasPistol || _hasSMG || _hasMelee;
    }

    public void ResetEnemyWeapon()
    {
        enemyRangeWeapon.SetCurrentRangeWeapon(WeaponPowerupController.GameWeapons.None);
        enemyMeleeWeapon.SetCurrentMeleeWeapon(WeaponPowerupController.GameWeapons.Punch);
        typeOfAttack = EnemyAttackType.Melee;
        _currentBehaviourTypeAI = EnemyAIMovementBehaviourType.SeekPlayerMeele;
    }
}
