using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Action", menuName = "Winter Universe/ALIFE/New Action")]
    public class ActionConfig : ScriptableObject
    {
        public string DisplayName = "Name";
        public int Cost = 1;
        public bool PlayAnimationOnStart;
        public string AnimationName = "Name";
        public List<StateCreator> ConditionsToStart = new();
        public List<StateCreator> Effects = new();
        public List<StateCreator> ConditionsToAbort = new();
        public bool CompleteOnReachedDestination;
        public bool CompleteOnAnimationEnd;
        public List<StateCreator> ConditionsToComplete = new();
    }
}