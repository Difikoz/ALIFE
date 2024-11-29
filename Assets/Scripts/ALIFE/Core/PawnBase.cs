using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace WinterUniverse
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class PawnBase : MonoBehaviour
    {
        private readonly float _nearestPointSearchRange = 5f;
        private Animator _animator;
        private NavMeshAgent _agent;
        private StateHolder _stateHolder;
        private ActionBase _currentAction;
        private GoalHolder _currentGoal;
        private List<ActionBase> _actions = new();
        private Dictionary<GoalHolder, int> _goals = new();
        private Queue<ActionBase> _actionQueue;
        private bool _isReachedDestination;

        public List<StateCreator> StatesToAdd = new();
        public List<GoalCreator> GoalsToAdd = new();

        public StateHolder StateHolder => _stateHolder;
        public ActionBase CurrentAction => _currentAction;
        public GoalHolder CurrentGoal => _currentGoal;
        public List<ActionBase> Actions => _actions;
        public Dictionary<GoalHolder, int> Goals => _goals;
        public bool IsReachedDestination => _isReachedDestination;

        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            foreach (StateCreator creator in StatesToAdd)
            {
                _stateHolder.SetState(creator.Key.ID, creator.Value);
            }
            ActionBase[] actions = GetComponentsInChildren<ActionBase>();
            foreach (ActionBase action in actions)
            {
                _actions.Add(action);
            }
            foreach (GoalCreator creator in GoalsToAdd)
            {
                _goals.Add(new(creator.Config), creator.Priority);
            }
        }

        protected virtual void Update()
        {
            if (_currentAction != null)
            {
                if (_currentAction.CanAbort())
                {
                    _currentAction.OnAbort();
                    _currentAction = null;
                }
                else if (_currentAction.CanComplete())
                {
                    _currentAction.OnComplete();
                    _currentAction = null;
                }
                else
                {
                    _currentAction.OnUpdate(Time.deltaTime);
                }
                return;
            }
            if (_actionQueue == null)
            {
                var sortedGoals = from entry in _goals orderby entry.Value descending select entry;
                foreach (KeyValuePair<GoalHolder, int> sg in sortedGoals)
                {
                    //Debug.LogWarning($"Try Get Plan for [{sg.Key.GoalName}]");
                    _actionQueue = TaskManager.GetPlan(_actions, _stateHolder.States, _goals.ElementAt(0).Key.Conditions);
                    if (_actionQueue != null)
                    {
                        _currentGoal = sg.Key;
                        return;
                    }
                }
            }
            if (_actionQueue != null && _actionQueue.Count > 0)
            {
                _currentAction = _actionQueue.Dequeue();
                if (_currentAction.CanStart())
                {
                    _currentAction.OnStart();
                }
                else
                {
                    _actionQueue = null;
                }
                return;
            }
            if (!_currentGoal.Config.Repeatable)
            {
                _goals.Remove(_currentGoal);
            }
            _actionQueue = null;
        }

        public void SetDestination(Vector3 position)
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hitResult, _nearestPointSearchRange, NavMesh.AllAreas))
            {
                _agent.SetDestination(hitResult.position);
                _isReachedDestination = false;
            }
        }

        public void SetDestinationInRange(float radius)
        {
            Vector3 position = transform.position;
            radius /= 2f;
            position += Vector3.right * Random.Range(-radius, radius);
            position += Vector3.forward * Random.Range(-radius, radius);
            SetDestination(position);
        }
    }
}