using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "Goal", menuName = "Winter Universe/ALIFE/New Goal")]
    public class GoalConfig : ScriptableObject
    {
        public string DisplayName = "Name";
        public bool Repeatable = true;
        public List<StateCreator> Conditions = new();
    }
}