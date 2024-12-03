using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace WinterUniverse
{
    public class PawnSimple : PawnBase
    {
        protected SensorHear _hear;
        protected SensorVision _vision;
        protected ActionBase _currentAction;
        protected GoalHolder _currentGoal;
        protected List<ActionBase> _actions = new();
        protected Dictionary<GoalHolder, int> _goals = new();
        protected Queue<ActionBase> _actionQueue;
        [Header("Add this to World Config")]
        public List<StateCreator> StatesToAdd = new();
        [Header("Add this to AI Config")]
        public List<GoalCreator> GoalsToAdd = new();

        public ActionBase CurrentAction => _currentAction;
        public GoalHolder CurrentGoal => _currentGoal;
        public List<ActionBase> Actions => _actions;
        public Dictionary<GoalHolder, int> Goals => _goals;

        protected override void GetComponents()
        {
            base.GetComponents();
            _hear = GetComponentInChildren<SensorHear>();
            _vision = GetComponentInChildren<SensorVision>();
        }

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
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
            _hear.Initialize();
            _vision.Initialize();
        }

        protected override void Update()
        {
            base.Update();
            _hear.Detect();
            _vision.Detect();
            //if (_target == null)
            //{
            //    if (_hear.DetectedTargets.Count > 0)
            //    {
            //        SetTarget(_hear.DetectedTargets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault());
            //    }
            //    else if (_vision.DetectedTargets.Count > 0)
            //    {
            //        SetTarget(_vision.DetectedTargets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault());
            //    }
            //}
            if (_hear.DetectedTargets.Count > 0)
            {
                SetTarget(_hear.DetectedTargets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault(), _target == null);
            }
            else if (_vision.DetectedTargets.Count > 0)
            {
                SetTarget(_vision.DetectedTargets.OrderBy(target => Vector3.Distance(target.transform.position, transform.position)).FirstOrDefault(), _target == null);
            }
        }

        protected override void LateUpdate()
        {
            base.Update();
            //ProccessGOAP();
            if (_target != null)
            {
                IsRunning = true;
                if (_hear.TargetIsDetected(_target))
                {
                    _lastTargetPosition = _target.transform.position;
                    SetDestination(_lastTargetPosition);
                }
                else if (_vision.TargetIsDetected(_target))
                {
                    _lastTargetPosition = _target.transform.position;
                    SetDestination(_lastTargetPosition);
                }
                else if (_reachedDestination)
                {
                    SetTarget(null);
                }
            }
            else
            {
                IsRunning = false;
                if (_reachedDestination)
                {
                    SetDestinationInRange(50f);
                }
            }
        }

        protected void ProccessGOAP()
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
            if (_actionQueue != null)
            {
                if (_actionQueue.Count > 0)
                {
                    _currentAction = _actionQueue.Dequeue();
                    if (_currentAction.CanStart())
                    {
                        _currentAction.OnStart();
                        return;
                    }
                    else
                    {
                        _actionQueue = null;
                    }
                }
                if (_currentGoal != null)
                {
                    if (!_currentGoal.Config.Repeatable)
                    {
                        _goals.Remove(_currentGoal);
                    }
                    _actionQueue = null;
                }
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
        }
    }
}