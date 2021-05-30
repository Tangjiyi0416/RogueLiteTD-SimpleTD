using UnityEngine;
namespace CombatSystem
{
    public class TestDefaultAttackSkill : Skill
    {
        TestTargetingMethod targetingMethod = new TestTargetingMethod();
        public TestDefaultAttackSkill(CombatManager owner)
        : base("permanent_default_attack", "Attack", "You attack, you ......", new string[] { }, 0, 0, 120, owner)
        {
        }

        public override void PrepareSkill()
        {
            cooldownTimer = COOLDOWN;
        }
        public override void Use()
        {
            if (cooldownTimer == 0)
            {
                foreach (var target in targetingMethod.GetTargets(1))
                {
                    target?.ReceiveHit(new Hit(owner.baseDamage, owner.totalDamageIncrement, owner.totalDamageMultiplier, owner, target));
                }
                cooldownTimer = COOLDOWN;
            }
            --cooldownTimer;

        }
    }


}