using System.Collections.Generic;

namespace WinterUniverse
{
    public class TaskNode
    {
        public TaskNode Parent;
        public float Cost;
        public Dictionary<string, bool> States;
        public ActionBase Action;

        public TaskNode(TaskNode newParent, float newCost, Dictionary<string, bool> allStates, ActionBase newAction)
        {
            Parent = newParent;
            Cost = newCost;
            States = new Dictionary<string, bool>(allStates);
            Action = newAction;
        }

        public TaskNode(Dictionary<string, bool> worldStates, Dictionary<string, bool> pawnStates)
        {
            States = new Dictionary<string, bool>(worldStates);
            foreach (KeyValuePair<string, bool> b in pawnStates)
            {
                if (!States.ContainsKey(b.Key))
                {
                    States.Add(b.Key, b.Value);
                }
            }
        }
    }
}