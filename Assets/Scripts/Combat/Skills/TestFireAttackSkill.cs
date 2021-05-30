using UnityEngine;
namespace CombatSystem
{
    public class TestFireAttackSkill : Skill
    {
        TestTargetingMethod targetingMethod = new TestTargetingMethod();
        public TestFireAttackSkill(CombatManager owner)
        : base("10sec_alt_attack", "Fire form", "Your attack is imbued with fire.", new string[] { }, 0, 60, 20, owner)
        {
        }

        public override void PrepareSkill()
        {
            skillDurationTimer = SKILL_DURATION;
            cooldownTimer = COOLDOWN;
        }
        public override void Use()
        {
            if (skillDurationTimer == 0) owner.ResetCurrentSkillToDefaultSkill();
            if (cooldownTimer == 0)
            {
                foreach (var target in targetingMethod.GetTargets(1))
                {
                    target?.ReceiveHit(new Hit(owner.baseDamage+new Phases(0,0,0,10,0), owner.totalDamageIncrement, owner.totalDamageMultiplier, owner, target));
                    target?.ReceiveStatusEffect(new TestBurningStatusEffect(owner, target, 25, 10));
                }
                cooldownTimer = COOLDOWN;
            }


            --skillDurationTimer;
            if (--cooldownTimer < 0) cooldownTimer = 0;
        }
    }


}