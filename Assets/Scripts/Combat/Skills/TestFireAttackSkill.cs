using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public class TestFireAttackSkill : Skill
    {
        private float attackCooldown = 60f;
        private float attackCooldownTimer = 60f;
        TestTargetingMethod targetingMethod = new TestTargetingMethod();
        public TestFireAttackSkill(CombatManager owner, Animator animator)
        : base("10sec_alt_attack", "Fire form", "Your attack is imbued with fire.", new string[] { }, 600f, 600f, owner,animator)
        {
        }
        public override void OnAdded()
        {
            owner.updateSkillEvent += Tick;
        }

        public override void OnRemoved()
        {
            owner.updateSkillEvent -= Tick;
        }

        public void Tick()
        {
            if (skillDurationTimer > 0f && attackCooldownTimer <= 0f)
            {
                foreach (var target in targetingMethod.GetTargets(1))
                {
                    if(target==null) continue;
                    animator.Play("skill_0");
                    target.ReceiveHit(
                        new Hit(
                            owner.combatData.baseDamage + new Phases(0, 0, 0, 10, 0)
                            , owner.combatData.totalDamageIncrement
                            , owner.combatData.totalDamageMultiplier
                            , owner
                            , target)
                        );
                    target?.ReceiveStatusEffect(new TestBurningStatusEffect(owner, target, 25f, 10));
                }
                attackCooldownTimer = attackCooldown;
            }
            else if (--cooldownTimer < 0f) cooldownTimer = 0f;

            if (--attackCooldownTimer < 0f) attackCooldownTimer = 0f;
            if (--skillDurationTimer < 0f) { cooldownTimer = COOLDOWN; skillDurationTimer = 0f; }
        }
        public override void Use()
        {
            if (cooldownTimer <= 0f) skillDurationTimer = SKILL_DURATION;
        }
        
    }
}