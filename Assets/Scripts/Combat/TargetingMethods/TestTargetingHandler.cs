using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapSystem;
namespace CombatSystem
{
    public class TestTargetingHandler : TargetingHandler
    {
        private HashSet<CombatManager> targets = new HashSet<CombatManager>();
        public override HashSet<CombatManager> GetTargets(int targetNumber)
        {
            targets.RemoveWhere(t => t == null);
            if (targets.Count < targetNumber)
                targets.Add(GameObject.Find("TestCreep")?.GetComponent<CombatManager>());
            return targets;
        }
    }
}
