using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EntitySystem
{
    public class BasicTowerAttack : Attack
    {
        public BasicTowerAttack(Entity owner, Animator animator) : base(
            "basic_tower_attack"
            , "Basic Tower Attack"
            , "DisplayDiscription"
            , 1f
            , 3f
            , new TestTargetingHandler()
            , owner
            , animator)
        {
        }

        protected override void OnAttackStarted()
        {
            base.OnAttackStarted();
            foreach (var target in targetingHandler.GetTargets(3))
            {
                target.ReceiveHit(new Hit(owner.combatData.damage, owner, target));
            }
        }

        public override void OnAdded()
        {
            owner.updateEvent += AttackUpdate;
        }

        public override void OnRemoved()
        {
            owner.updateEvent -= AttackUpdate;
        }
    }
}

