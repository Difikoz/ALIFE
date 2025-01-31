using UnityEngine;
using UnityEngine.AI;

namespace WinterUniverse
{
    public abstract class PawnBase : MonoBehaviour
    {
        public float ForwardVelocity;
        public float RightVelocity;
        public float TurnVelocity;
        public bool IsMoving;
        public bool IsRunning;

        protected PawnBaseConfig _pawnBaseConfig;
        protected readonly float _nearestPointSearchRange = 5f;
        protected Animator _animator;
        protected NavMeshAgent _agent;
        protected StateHolder _stateHolder;
        protected Vector3 _moveVelocity;
        protected bool _reachedDestination;
        protected bool _isDead;
        protected PawnBase _target;
        protected Vector3 _lastTargetPosition;
        protected WeaponSlot _rightHandWeaponSlot;
        protected WeaponSlot _leftHandWeaponSlot;

        [SerializeField] protected Transform _head;
        [SerializeField] protected Transform _body;
        [SerializeField] protected float _targetLostDistance = 100f;
        [SerializeField] protected float _rotateSpeed = 0.5f;
        [SerializeField] protected float _turnAnimationAngle = 45f;

        public Transform Head => _head;
        public Transform Body => _body;
        public StateHolder StateHolder => _stateHolder;
        public bool ReachedDestination => _reachedDestination;
        public bool IsDead => _isDead;
        public PawnBase Target => _target;
        public WeaponSlot RightHandWeaponSlot => _rightHandWeaponSlot;
        public WeaponSlot LeftHandWeaponSlot => _leftHandWeaponSlot;

        protected virtual void Awake()
        {
            GetComponents();
            InitializeComponents();
        }

        protected virtual void GetComponents()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponentInChildren<NavMeshAgent>();
            WeaponSlot[] weaponSlots = GetComponentsInChildren<WeaponSlot>();
            foreach (WeaponSlot ws in weaponSlots)
            {
                if (ws.HandSlotType == HandSlotType.Right)
                {
                    _rightHandWeaponSlot = ws;
                }
                else if (ws.HandSlotType == HandSlotType.Left)
                {
                    _leftHandWeaponSlot = ws;
                }
            }
            _stateHolder = new();
        }

        protected virtual void InitializeComponents()
        {
            //_agent.updatePosition = false;
            _agent.updateRotation = false;
        }

        protected virtual void OnEnable()
        {

        }

        protected virtual void OnDisable()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void Update()
        {
            if (!_reachedDestination && _agent.remainingDistance < 1f)
            {
                StopMovement();
            }
            IsMoving = _agent.desiredVelocity != Vector3.zero;
            if (IsMoving)
            {
                if (IsRunning)
                {
                    _moveVelocity = Vector3.MoveTowards(_moveVelocity, _agent.desiredVelocity.normalized * 2f, 4f * Time.deltaTime);
                }
                else
                {
                    _moveVelocity = Vector3.MoveTowards(_moveVelocity, _agent.desiredVelocity.normalized, 2f * Time.deltaTime);
                }
            }
            else
            {
                _moveVelocity = Vector3.MoveTowards(_moveVelocity, Vector3.zero, 4f * Time.deltaTime);
            }
            ForwardVelocity = Vector3.Dot(_moveVelocity, transform.forward);
            RightVelocity = Vector3.Dot(_moveVelocity, transform.right);
            if (_target != null)
            {
                TurnVelocity = Vector3.SignedAngle(transform.forward, (_target.transform.position - transform.position).normalized, Vector3.up);
            }
            else
            {
                TurnVelocity = Vector3.SignedAngle(transform.forward, _agent.desiredVelocity.normalized, Vector3.up);
            }
            if (IsMoving)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_agent.desiredVelocity.normalized), _rotateSpeed * Time.deltaTime);
            }
            _animator.SetFloat("ForwardVelocity", ForwardVelocity);
            _animator.SetFloat("RightVelocity", RightVelocity);
            _animator.SetFloat("TurnVelocity", TurnVelocity / _turnAnimationAngle);
            _animator.SetBool("IsMoving", IsMoving);
            _agent.transform.localPosition = Vector3.zero;
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual void SetTarget(PawnBase target, bool resetMovement = true)
        {
            if (target != null)
            {
                _target = target;
            }
            else
            {
                _target = null;
            }
            if (resetMovement)
            {
                StopMovement();
            }
        }

        public void StopMovement()
        {
            _reachedDestination = true;
            _agent.ResetPath();
        }

        public void SetDestination(Vector3 position)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hitResult, _nearestPointSearchRange, NavMesh.AllAreas))
            {
                _agent.SetDestination(hitResult.position);
                _reachedDestination = false;
            }
        }

        public void SetDestinationInRange(Vector3 position, float radius)
        {
            radius /= 2f;
            position += Vector3.right * Random.Range(-radius, radius);
            position += Vector3.forward * Random.Range(-radius, radius);
            SetDestination(position);
        }

        public void SetDestinationInRange(float radius)
        {
            SetDestinationInRange(transform.position, radius);
        }
    }
}