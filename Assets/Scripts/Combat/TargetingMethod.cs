using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public abstract class TargetingMethod
    {
        public abstract void Init();
        public abstract List<CombatManager> GetTargets(int targetNumber);
    }
}