using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EntitySystem
{
    public abstract class TargetingHandler
    {
        public abstract HashSet<Entity> GetTargets(int targetNumber);
    }
}