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
            foreach(var t in targeting.GetTargets(3)){
                Debug.Log(t.name);
            }
            
        }

        public override void RecieveDamage()
        {
            throw new System.NotImplementedException();
        }

        public override void RecieveEffect(StatusEffect effect)
        {
            throw new System.NotImplementedException();
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
    }
}
