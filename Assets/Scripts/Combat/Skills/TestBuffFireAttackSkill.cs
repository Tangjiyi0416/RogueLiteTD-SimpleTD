using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EntitySystem
{
    public class TestBuffFireAttackSkill : Skill
    {
        private ExampleAddedFireDamageModifier modifier = new ExampleAddedFireDamageModifier();
        public TestBuffFireAttackSkill(Entity owner, Animator animator) : base(
            "test_buff_fireattack_skill_auto"
            , "Fire Buff Skill"
            , "Your attack is imbued with fire."
            , new string[] { "auto", "buff", "fire" }
            , owner
            , animator)
        {
            FullSkillChargeTime = 10f;//seconds
            SkillDuration = 30f;//seconds
        }

        public override void Initialize()
        {
            owner.updateEvent -= SkillUpdate;
            owner.updateEvent += SkillUpdate;
            skillDurationTimer = 0;
            skillChargingTimer = 0;
        }
        public override void Use()
        {
            Debug.Log($"{owner.name}'s {DisplayName} is Charging: {IsChargingSkill} Timer:{skillChargingTimer}, is in Duration: {IsInDuration} Timer:{skillDurationTimer}");
        }

        protected override void OnChargingEnded()
        {
            base.OnChargingEnded();
            IsInDuration = true;
            OnDurationStarted();
        }
        protected override void OnDurationStarted()
        {
            base.OnDurationStarted();
            owner.ReceiveModifier(modifier);
        }
        protected override void OnDurationEnded()
        {
            base.OnDurationEnded();
            owner.RemoveModifier(modifier);
        }
    }
}