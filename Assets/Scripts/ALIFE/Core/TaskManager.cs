using System.Collections.Generic;
using UnityEngine;

namespace WinterUniverse
{
    public class TaskManager : Singleton<TaskManager>
    {
        // other logic

        public static Queue<ActionBase> GetPlan(List<ActionBase> allActions, Dictionary<string, bool> pawnStates, Dictionary<string, bool> goal)
        {
            List<ActionBase> usableActions = new();
            foreach (ActionBase action in allActions)
            {
                usableActions.Add(action);
            }
            List<TaskNode> leaves = new();
            TaskNode startNode = new(WorldManager.StaticInstance.StateHolder.States, pawnStates);
            bool success = BuildGraph(startNode, leaves, usableActions, goal);
            if (!success)
            {
                //Debug.Log($"NO PLAN!");
                return null;
            }
            TaskNode cheapestNode = null;
            foreach (TaskNode leaf in leaves)
            {
                if (cheapestNode == null)
                {
                    cheapestNode = leaf;
                }
                else if (leaf.Cost < cheapestNode.Cost)
                {
                    cheapestNode = leaf;
                }
            }
            List<ActionBase> result = new();
            TaskNode n = cheapestNode;
            while (n != null)
            {
                if (n.Action != null)
                {
                    result.Insert(0, n.Action);
                }
                n = n.Parent;
            }
            Queue<ActionBase> queue = new();
            foreach (ActionBase a in result)
            {
                queue.Enqueue(a);
            }
            string planText = "The Plan is: ";
            foreach (ActionBase a in queue)
            {
                planText += $" {a.Config.DisplayName}, ";
            }
            planText += "END";
            Debug.Log(planText);
            return queue;
        }

        private static bool BuildGraph(TaskNode parent, List<TaskNode> leaves, List<ActionBase> usableActions, Dictionary<string, bool> goal)
        {
            bool foundPath = false;
            foreach (ActionBase action in usableActions)
            {
                if (action.IsAchievable(parent.States))
                {
                    Dictionary<string, bool> currentState = new(parent.States);
                    foreach (KeyValuePair<string, bool> effect in action.Effects)
                    {
                        if (!currentState.ContainsKey(effect.Key))
                        {
                            currentState.Add(effect.Key, effect.Value);
                        }
                    }
                    TaskNode node = new(parent, parent.Cost + action.Config.Cost, currentState, action);
                    if (GoalAchieved(goal, currentState))
                    {
                        leaves.Add(node);
                        foundPath = true;
                    }
                    else
                    {
                        List<ActionBase> subset = ActionSubset(usableActions, action);
                        bool found = BuildGraph(node, leaves, subset, goal);
                        if (found)
                        {
                            foundPath = true;
                        }
                    }
                }
            }
            return foundPath;
        }

        private static List<ActionBase> ActionSubset(List<ActionBase> actions, ActionBase removeMe)
        {
            List<ActionBase> subset = new();
            foreach (ActionBase a in actions)
            {
                if (!a.Equals(removeMe))// rework equals?
                {
                    subset.Add(a);
                }
            }
            return subset;
        }

        private static bool GoalAchieved(Dictionary<string, bool> goal, Dictionary<string, bool> states)
        {
            foreach (KeyValuePair<string, bool> condition in goal)
            {
                if (!states.ContainsKey(condition.Key))
                {
                    return false;
                }
                else if (states[condition.Key] != condition.Value)
                {
                    return false;
                }
            }
            return true;
        }
    }
}