using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public abstract class ActionBase : MonoBehaviour
    {
        [SerializeField] protected ActionConfig _config;

        protected PawnBase _pawn;
        private Dictionary<string, bool> _conditionsToStart = new();
        private Dictionary<string, bool> _effects = new();
        private Dictionary<string, bool> _conditionsToAbort = new();
        private Dictionary<string, bool> _conditionsToComplete = new();

        public ActionConfig Config => _config;
        public Dictionary<string, bool> Conditions => _conditionsToStart;
        public Dictionary<string, bool> Effects => _effects;

        private void Awake()
        {
            _pawn = GetComponentInParent<PawnBase>();
            foreach (StateCreator creator in _config.ConditionsToStart)
            {
                _conditionsToStart.Add(creator.Key.ID, creator.Value);
            }
            foreach (StateCreator creator in _config.Effects)
            {
                _effects.Add(creator.Key.ID, creator.Value);
            }
            foreach (StateCreator creator in _config.ConditionsToAbort)
            {
                _conditionsToAbort.Add(creator.Key.ID, creator.Value);
            }
            foreach (StateCreator creator in _config.ConditionsToComplete)
            {
                _conditionsToComplete.Add(creator.Key.ID, creator.Value);
            }
        }

        public bool IsAchievable()
        {
            return true;
        }

        public bool IsAchievable(Dictionary<string, bool> givenConditions)
        {
            foreach (KeyValuePair<string, bool> condition in _conditionsToStart)
            {
                if (!givenConditions.ContainsKey(condition.Key))
                {
                    return false;
                }
                else if (givenConditions[condition.Key] != condition.Value)
                {
                    return false;
                }
            }
            return true;
        }

        public virtual bool CanStart()
        {
            return true;
        }

        public virtual bool CanAbort()
        {
            foreach (KeyValuePair<string, bool> condition in _conditionsToAbort)
            {
                if (_pawn.StateHolder.ValidState(condition.Key, condition.Value))
                {
                    return true;
                }
                if (WorldManager.StaticInstance.StateHolder.ValidState(condition.Key, condition.Value))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual bool CanComplete()
        {
            if (_config.CompleteOnReachedDestination && _pawn.IsReachedDestination)
            {
                return true;
            }
            if (_config.CompleteOnAnimationEnd)//&& !_pawn.IsPerfomingAction
            {
                return true;
            }
            foreach (KeyValuePair<string, bool> condition in _conditionsToComplete)
            {
                if (!_pawn.StateHolder.ValidState(condition.Key, condition.Value))
                {
                    return false;
                }
                if (!WorldManager.StaticInstance.StateHolder.ValidState(condition.Key, condition.Value))
                {
                    return false;
                }
            }
            return true;
        }

        public virtual void OnStart()
        {
            if (_config.PlayAnimationOnStart)
            {
                //_pawn.Animator.PlayAction(_config.AnimationName);
            }
        }

        public virtual void OnUpdate(float deltaTime)
        {

        }

        public virtual void OnAbort()
        {

        }

        public virtual void OnComplete()
        {

        }
    }
}