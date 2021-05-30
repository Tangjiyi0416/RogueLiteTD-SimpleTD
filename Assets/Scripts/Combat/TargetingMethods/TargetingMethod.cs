using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public abstract class TargetingMethod
    {
        public abstract HashSet<CombatManager> GetTargets(int targetNumber);
    }
}