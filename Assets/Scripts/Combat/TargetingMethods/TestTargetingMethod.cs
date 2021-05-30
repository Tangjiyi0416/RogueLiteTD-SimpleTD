using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hex;
namespace CombatSystem
{
    public class TestTargetingMethod : TargetingMethod
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
