using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hex;
namespace EntitySystem
{
    public class TestTargetingHandler : TargetingHandler
    {
        private HashSet<Entity> targets = new HashSet<Entity>();
        public override HashSet<Entity> GetTargets(int targetNumber)
        {
            targets.RemoveWhere(t => t == null);
            if (targets.Count < targetNumber)
                targets.Add(GameObject.Find("TestCreep")?.GetComponent<Entity>());
            return targets;
        }
    }
}
