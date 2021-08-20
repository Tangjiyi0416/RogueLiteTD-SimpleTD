using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public abstract class TargetingHandler
    {
        public abstract HashSet<CombatManager> GetTargets(int targetNumber);
    }
}