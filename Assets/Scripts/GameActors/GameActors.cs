using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameActors
{
    public enum GameActorAnimationStates { None, Idle, Stunned, Dead, Walking, MeleeAttacking, MeleeAttackingWithKnife, RangeAttacking, Disarming }
    public class GameActors : MonoBehaviour
    {
        #region Inspector Variables
        [SerializeField] protected int initialGameObjectLayer = 1;
        [SerializeField] protected GameObject spritesBodyContainer;
        [SerializeField] protected GameObject bodySprite;
        [SerializeField] protected float punchCooldown = 2.8f;
        [SerializeField] protected float punchCooldowDecreseAmount = 0.1f;

        [SerializeField] protected float stunCooldown = 2.95f;
        [SerializeField] protected float stunCooldowDecreseAmount = 0.1f;

        #endregion

        #region Private Variables
        protected Animator _animator;
        protected Rigidbody2D _rigidBody2D;
        protected GameActorAnimationStates _currentAnimationState;
        protected bool _canMove = true;
        protected bool _isMeleeAttaking = false;
        protected bool _hasPistol = false;
        protected bool _hasSMG = false;
        protected bool _hasMachineGun = false;
        protected bool _hasMelee = false;
        private bool _isStunned = false;
        private bool _isDead = false;

        #endregion

        #region Properties Accessors

        public bool IsMeleeAttaking
        {
            get
            {
                return _isMeleeAttaking;
            }
        }

        public bool HasMelee
        {
            get
            {
                return _hasMelee;
            }

            set
            {
                _hasMelee = value;
            }
        }

        public bool HasPistol
        {
            get
            {
                return _hasPistol;
            }

            set
            {
                _hasPistol = value;
            }
        }

        public bool HasSMG
        {
            get
            {
                return _hasSMG;
            }

            set
            {
                _hasSMG = value;
            }
        }

        public bool HasMachineGun
        {
            get
            {
                return _hasMachineGun;
            }

            set
            {
                _hasMachineGun = value;
            }
        }

        public bool IsStunned
        {
            get
            {
                return _isStunned;
            }
        }

        public bool IsDead
        {
            get
            {
                return _isDead;
            }
        }

        public GameActorAnimationStates CurrentAnimationState
        {
            get { return _currentAnimationState; }
        }

        #endregion

        protected void Start()
        {
            if (!(_animator = bodySprite.GetComponent<Animator>()))
            {
                Debug.LogWarning("Missing Animator");
            }

            _rigidBody2D = this.GetComponent<Rigidbody2D>();

            initialGameObjectLayer = gameObject.layer;
        }

        protected void GetDirectionFromVector(Vector3 direction)
        {
            //Gets the angle for the current move direction
            var angle = -1 * Mathf.Round(Vector3.SignedAngle(direction, transform.right, Vector3.forward));
            // Clamps the angle into the 8 possible directions (45 degree angle increments)
            var campledAngle = 45 * (int)Math.Round((((double)angle % 360) / 45));
            //Set the rotation
            spritesBodyContainer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, campledAngle));
        }

        public void SetAnimationState(GameActorAnimationStates animationState)
        {
            // if (animationState == _currentAnimationState)
            //     return;

            _animator.SetBool("HasKnife", _hasMelee);
            _animator.SetBool("HasPistol", _hasPistol);
            _animator.SetBool("HasSMG", _hasSMG);
            _animator.SetBool("HasMachineGun", _hasMachineGun);

            switch (animationState)
            {
                case (GameActorAnimationStates.Idle):
                    _currentAnimationState = GameActorAnimationStates.Idle;
                    _animator.SetBool("Walking", false);
                    _animator.SetBool("IsStunned", false);
                    break;
                case (GameActorAnimationStates.MeleeAttacking):
                    _currentAnimationState = GameActorAnimationStates.MeleeAttacking;
                    _animator.SetTrigger("Punching");
                    StartCoroutine(ActorPunch());
                    break;
                case (GameActorAnimationStates.Walking):
                    _currentAnimationState = GameActorAnimationStates.Walking;
                    _animator.SetBool("Walking", true);
                    break;
                case (GameActorAnimationStates.Stunned):
                    if (!_isStunned)
                    {
                        _currentAnimationState = GameActorAnimationStates.Stunned;
                        StartCoroutine(ActorStunned());
                        _animator.SetBool("Walking", false);
                        _animator.SetBool("IsStunned", true);
                    }
                    break;
                case (GameActorAnimationStates.Dead):
                    _currentAnimationState = GameActorAnimationStates.Dead;
                    _animator.SetBool("Walking", false);
                    _animator.SetTrigger("Dying");
                    _isDead = true;
                    break;
                case (GameActorAnimationStates.Disarming):

                    _animator.SetTrigger("Disarming");

                    break;

                default:
                    break;
            }
        }

        IEnumerator ActorPunch()
        {
            _isMeleeAttaking = true;
            _canMove = false;

            float delayCounter = punchCooldown;
            while (delayCounter > 0)
            {
                delayCounter -= punchCooldowDecreseAmount;
                yield return null;
            }

            _isMeleeAttaking = false;
            _canMove = true;

        }

        IEnumerator ActorStunned()
        {
            _isStunned = true;
            _canMove = false;
            gameObject.layer = LayerMask.NameToLayer("StunnedLayer");

            float delayCounter = stunCooldown;
            while (delayCounter > 0)
            {
                delayCounter -= stunCooldowDecreseAmount;
                yield return null;
            }

            if(!_isDead)
            {

                gameObject.layer = initialGameObjectLayer;

                _isStunned = false;
                _canMove = true;

                SetAnimationState(GameActorAnimationStates.Idle);
            }

        }
    }
}
