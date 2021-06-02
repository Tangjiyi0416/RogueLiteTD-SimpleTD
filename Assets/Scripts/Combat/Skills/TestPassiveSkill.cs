using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public class TestPassiveSkill : Skill
    {
        TestTargetingMethod targetingMethod = new TestTargetingMethod();
        public TestPassiveSkill(CombatManager owner, Animator animator)
        : base("test_passive_skill", "Lively Shell", "You have a lively shell, the more hits you take, the stronger it grows.", new string[] { }, 0, 0, owner,animator)
        {
        }
        public override void OnAdded()
        {
            owner.whenHitEvent += WhenHit;
        }

        public override void OnRemoved()
        {
            owner.whenHitEvent -= WhenHit;
        }

        public void WhenHit(Hit hit)
        {
            owner.combatData.resistence+=new Phases(10);
        }
        public override void Use()
        {
            if (cooldownTimer <= 0f) skillDurationTimer = SKILL_DURATION;
        }
        
    }
}