using UnityEngine;

namespace WinterUniverse
{
    public class WorldManager : Singleton<WorldManager>
    {
        public StateHolderConfig StateCreatorHolder;

        private StateHolder _stateHolder;

        public StateHolder StateHolder => _stateHolder;

        protected override void Awake()
        {
            base.Awake();
            _stateHolder = new();
            foreach (StateCreator creator in StateCreatorHolder.StatesToAdd)
            {
                _stateHolder.SetState(creator.Key.ID, creator.Value);
            }
        }
    }
}