using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class TowerCombatManager : CombatManager
    {
        private void Start() {
            skills.Add(new TestDefaultAttackSkill(this));
            skills.Add(new TestFireAttackSkill(this));
            ResetCurrentSkillToDefaultSkill();
        }
        private void Update()
        {
            //if (statusEffects.Count > 0) Debug.Log($"{statusEffects[0].DISPLAY_NAME} {statusEffects.Count}");
            UseCurrentSkill();
            TriggerUpdateStatusEffectEvent();
        }

        protected override void Dead()
        {
            Debug.Log("I'm a Tower and I'm dead.");
            base.Dead();
        }


    }
}
