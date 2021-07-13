using System;
using UnityEngine;
namespace EntitySystem
{
    //TODO: add template gameobject to handle the display of Skill
    public abstract class Skill
    {
        public string Name { get; }
        public string DisplayName { get; }
        public string DisplayDiscription { get; }
        public string[] Tags { get; }

        public bool IsInDuration { get; protected set; }
        /// <summary>
        /// Not every Skill needs a duration, If you do, remember to subscribe your durationTimer method to "owner.updateSkillEvent".
        /// </summary>
        public float SkillDuration { get; protected set; }//in seconds
        protected float skillDurationTimer;

        public bool IsChargingSkill { get; protected set; }
        /// <summary>
        /// Not every Skill needs a cooldown. If you do, remember to subscribe your cooldown method to "owner.updateSkillEvent".
        /// </summary>
        public float FullSkillChargeTime { get; protected set; }//in seconds
        protected float skillChargingTimer;

        public Entity owner;
        protected Animator animator;
        protected Skill(string Name, string DisplayName, string DisplayDiscription, string[] Tags, Entity owner, Animator animator)
        {
            this.Name = Name;
            this.DisplayName = DisplayName;
            this.DisplayDiscription = DisplayDiscription;
            this.Tags = Tags;
            this.owner = owner;
            this.animator = animator;
            IsInDuration = false;
            IsChargingSkill = true;
        }
        public abstract void Initialize();
        // This is where you implement the function of your skill,
        public abstract void Use();

        protected virtual void SkillUpdate(){
            if (IsChargingSkill)
            {
                if (skillChargingTimer >= FullSkillChargeTime)//cooldown completed, switch state to duration
                {
                    //state settings
                    IsChargingSkill = false;
                    skillDurationTimer = SkillDuration;
                    //When charge completed, manual trigger may not want to activate the skill instantly.
                    //If you're making a skill with auto trigger, just call the OnDurationStarted() in the OnChargingEnded()
                    //And don't forget to set the IsInDuration to true!
                    OnChargingEnded();
                }
                skillChargingTimer += owner.combatData.skillCooldownSpeed.FinalValue * GameManager.instance.gameDeltaTime;
            }
            else if (IsInDuration)
            {
                if (skillDurationTimer <= 0f)//duration ended, switch state to cooldown
                {
                    //state settings
                    IsInDuration = false;
                    IsChargingSkill = true;
                    skillChargingTimer = 0;
                    OnDurationEnded();
                    OnChargingStarted();
                }
                skillDurationTimer -= GameManager.instance.gameDeltaTime;
            }
        }

        protected virtual void OnChargingStarted(){
            Debug.Log($"{DisplayName} has started charging.");
        }
        protected virtual void OnChargingEnded(){
            Debug.Log($"{DisplayName} has fully charged.");
        }
        protected virtual void OnDurationStarted(){
            Debug.Log($"{DisplayName}'s duration has started.");
        }
        protected virtual void OnDurationEnded(){
            Debug.Log($"{DisplayName}'s duration has ended.");
        }
    }
}