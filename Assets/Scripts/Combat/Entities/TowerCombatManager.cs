using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class TowerCombatManager : CombatManager
    {
        public Animator animator;
        private void Start()
        {
            animator = GetComponent<Animator>();
            ReceiveSkill(new TestBuffFireAttackSkill(this, animator));
            ReceiveAttack(new BasicTowerAttack(this, animator));
        }
        public void SelfHit()
        {
            ReceiveHit(new Hit(combatData.damage, this, this));
            Debug.Log("Why are you hitting yourself?");
        }
        protected override void Update()
        {
            base.Update();
        }

        protected override void Die()
        {
            Debug.Log("I'm a Tower and I'm dead.");
            base.Die();
        }


    }
}
