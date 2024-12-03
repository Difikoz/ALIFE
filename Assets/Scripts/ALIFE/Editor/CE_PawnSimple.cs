using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace WinterUniverse
{
    [CustomEditor(typeof(PawnSimple))]
    public class CE_PawnSimple : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            PawnSimple pawn = (PawnSimple)target;
            //GUILayout.Label("Name => " + pawn.Agent.Data.DisplayName);
            //GUILayout.Label("Health => " + pawn.Agent.Pawn.HealthSystem.HealthPercent * 100f + "%");
            GUILayout.Label($"Current Target => " + (pawn.Target != null ? pawn.Target.gameObject.name : "Empty"));
            GUILayout.Label($"Current Goal => " + (pawn.CurrentGoal != null ? pawn.CurrentGoal.Config.DisplayName : "Empty"));
            GUILayout.Label($"Current Action => " + (pawn.CurrentAction != null ? pawn.CurrentAction.Config.DisplayName : "Empty"));
            //GUILayout.Label("===== Possible Actions =====");
            //foreach (GoapAction a in agent.Agent.Actions)
            //{
            //    string condition = "";
            //    string effect = "";
            //    foreach (KeyValuePair<string, int> p in a.Conditions)
            //        condition += $"{p.Key} {p.Value}, ";
            //    foreach (KeyValuePair<string, int> e in a.Effects)
            //        effect += $"{e.Key} {e.Value}, ";
            //    GUILayout.Label($"== DO [{a.ActionName}] IF ==\n[{condition}]\n== AND NEED ==\n[{effect}]");
            //}
            GUILayout.Label("===== Goals =====");
            if (pawn.Goals.Count > 0)
            {
                var sortedGoals = from entry in pawn.Goals orderby entry.Value descending select entry;
                foreach (KeyValuePair<GoalHolder, int> goal in sortedGoals)
                {
                    GUILayout.Label($"{goal.Key.Config.DisplayName} : {goal.Value}");
                }
            }
            GUILayout.Label("===== States =====");
            if (pawn.StateHolder != null && pawn.StateHolder.States.Count > 0)
            {
                foreach (KeyValuePair<string, bool> state in pawn.StateHolder.States)
                {
                    GUILayout.Label($"{state.Key} : {state.Value}");
                }
            }
            GUILayout.Label("===== Equipment =====");
            GUILayout.Label("===== Inventory =====");
            //foreach (WU_ItemStack stack in pawn.Agent.Pawn.InventorySystem.Items)
            //{
            //    GUILayout.Label($"{stack.Item.DisplayName}:{stack.Amount}");
            //}
            serializedObject.ApplyModifiedProperties();
        }
    }
}