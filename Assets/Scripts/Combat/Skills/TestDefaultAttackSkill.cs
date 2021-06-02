using UnityEngine;
namespace CombatSystem
{
    public class TestDefaultAttackSkill : Skill
    {
        private int attackCooldown = 60;
        private int attackCooldownTimer = 60;
        TestTargetingMethod targetingMethod = new TestTargetingMethod();
        public TestDefaultAttackSkill(CombatManager owner, Animator animator)
        : base("permanent_default_attack", "Attack", "You attack, you ......", new string[] { }, 0, 0, owner, animator)
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
            if (attackCooldownTimer <= 0)
            {
                foreach (var target in targetingMethod.GetTargets(1))
                {
                    animator.Play("attack_default");
                    target?.ReceiveHit(
                        new Hit(
                            owner.combatData.baseDamage
                            , owner.combatData.totalDamageIncrement
                            , owner.combatData.totalDamageMultiplier
                            , owner
                            , target)
                    );
                }

                attackCooldownTimer = attackCooldown;
            }
            if (--attackCooldownTimer < 0) attackCooldownTimer = 0;

        }
        public override void Use() { }
    }
}