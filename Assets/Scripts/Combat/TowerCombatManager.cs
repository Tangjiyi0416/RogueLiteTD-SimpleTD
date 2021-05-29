using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class TowerCombatManager : CombatManager
    {
        public TargetingMethod targeting;
        public int attackCoolDown;
        public int attackCoolDownTimer;

        public void Init()
        {
            targeting = new TestTargetingMethod();
            targeting.Init();
        }
        public override void Attack()
        {
            foreach (var target in targeting.GetTargets(3))
            {
                target.ReceiveHit(new Hit(baseDamage, totalDamageIncrement, totalDamageMultiplier, this, target));
            }
            
            tmp?.ReceiveHit(new Hit(baseDamage, totalDamageIncrement, totalDamageMultiplier, this, tmp));
        }

        private void Start()
        {
            Init();
            attackCoolDownTimer = 0;
        }
        private void Update()
        {
            if (attackCoolDownTimer == 0)
            {
                Attack();
                attackCoolDownTimer = attackCoolDown;
            }
            else if (attackCoolDownTimer > 0) attackCoolDownTimer--;
            if (statusEffects.Count > 0)
                //Debug.Log($"{statusEffects[0].DISPLAY_NAME} {statusEffects.Count}");

            TriggerUpdateStatusEffectEvent();

        }

        protected override void Dead()
        {
            Debug.Log("I'm a Tower and I'm dead.");
            base.Dead();
        }


    }
}
