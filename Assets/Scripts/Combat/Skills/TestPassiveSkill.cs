using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EntitySystem
{
    public class TestPassiveSkill : Skill
    {
        public TestPassiveSkill(Entity owner, Animator animator) : base(
            "test_skill_passive"
            , "Lively Shell"
            , "You have a lively shell, the more hits you take, the stronger it grows."
            , new string[] { "passive", "resistence", "metal", "wood", "water", "fire", "earth" }
            , owner
            , animator)
        {
        }

        public override void Initialize()
        {
            owner.whenHitEvent -= WhenHit;
            owner.whenHitEvent += WhenHit;
        }

        public override void Use()
        {
            Debug.Log("Guess what? I'm a >>PASSIVE<< Skill.");
        }

        public void WhenHit(Hit hit)
        {
            for (int i = 1; i < DamageType.damageTypeCount; i++)
            {
                owner.combatData.rawResistence[i].ScaleBaseValue(10);
            }
        }


    }
}