using System.Collections.Generic;

namespace WinterUniverse
{
    public class StateHolder
    {
        private Dictionary<string, bool> _states = new();

        public Dictionary<string, bool> States => _states;

        public bool ValidState(string key, bool value)
        {
            if (_states.ContainsKey(key))
            {
                return _states[key] == value;
            }
            return false;
        }

        public void SetState(string key, bool value)
        {
            if (_states.ContainsKey(key))
            {
                _states[key] = value;
            }
            else
            {
                _states.Add(key, value);
            }
        }
    }
}