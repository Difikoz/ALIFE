using System.Collections.Generic;

namespace WinterUniverse
{
    public class GoalHolder
    {
        private GoalConfig _config;
        private Dictionary<string, bool> _conditions = new();

        public GoalConfig Config => _config;
        public Dictionary<string, bool> Conditions => _conditions;

        public GoalHolder(GoalConfig config)
        {
            _config = config;
            foreach (StateCreator creator in _config.Conditions)
            {
                _conditions.Add(creator.Key.ID, creator.Value);
            }
        }
    }
}