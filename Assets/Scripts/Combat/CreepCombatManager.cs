using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class CreepCombatManager : CombatManager{

        private void Update() {
            TriggerUpdateStatusEffectEvent();
        }
    }
}
