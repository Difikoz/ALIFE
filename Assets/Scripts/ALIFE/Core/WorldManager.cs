using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class WorldManager : Singleton<WorldManager>
    {
        public List<StateCreator> StatesToAdd = new();

        private StateHolder _stateHolder;

        public StateHolder StateHolder => _stateHolder;

        protected override void Awake()
        {
            base.Awake();
            foreach (StateCreator creator in StatesToAdd)
            {
                _stateHolder.SetState(creator.Key.ID, creator.Value);
            }
        }
    }
}