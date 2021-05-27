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
                target.RecieveHit(new Hit(baseDamage, totalDamageIncrement, totalDamageMultiplier, this));
            }

        }

        public override void RecieveHit(Hit hit)
        {
            hit.origin.TriggerOnHitEffect(hit);
            Phases finalDamage = hit.baseDamage * (hit.totalDamageIncrease) * (hit.totalDamageMore);
            life -= finalDamage.Total;
            TriggerWhenHitEffects(hit);
            if (IsDead()) Dead();
        }
        public override void RecieveEffect(StatusEffect effect)
        {
            throw new NotImplementedException();
        }

        public override void UpdateModifiedDamage()
        {
            throw new System.NotImplementedException();
        }

        public override void UseSkill()
        {
            throw new System.NotImplementedException();
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

        }

        protected override void Dead()
        {
            Debug.Log("I'm a Tower and I'm dead.");
            base.Dead();
        }


    }
}
