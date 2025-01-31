using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Action", menuName = "Winter Universe/ALIFE/New Action")]
    public class ActionConfig : ScriptableObject
    {
        [SerializeField] private string _displayName = "Name";
        [SerializeField] private int _cost = 1;
        [SerializeField] private bool _playAnimationOnStart;
        [SerializeField] private string _animationName = "Name";
        [SerializeField] private List<StateCreator> _conditionsToStart = new();
        [SerializeField] private List<StateCreator> _effects = new();
        [SerializeField] private List<StateCreator> _conditionsToAbort = new();
        [SerializeField] private bool _completeOnReachedDestination;
        [SerializeField] private bool _completeOnAnimationEnd;
        [SerializeField] private List<StateCreator> _conditionsToComplete = new();

        public string DisplayName => _displayName;
        public int Cost => _cost;
        public bool PlayAnimationOnStart => _playAnimationOnStart;
        public string AnimationName => _animationName;
        public List<StateCreator> ConditionsToStart => _conditionsToStart;
        public List<StateCreator> Effects => _effects;
        public List<StateCreator> ConditionsToAbort => _conditionsToAbort;
        public bool CompleteOnReachedDestination => _completeOnReachedDestination;
        public bool CompleteOnAnimationEnd => _completeOnAnimationEnd;
        public List<StateCreator> ConditionsToComplete => _conditionsToComplete;
    }
}