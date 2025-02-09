using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    [CreateAssetMenu(fileName = "State Holder", menuName = "Winter Universe/ALIFE/New State Holder")]
    public class StateHolderConfig : ScriptableObject
    {
        public List<StateCreator> StatesToAdd = new();
    }
}