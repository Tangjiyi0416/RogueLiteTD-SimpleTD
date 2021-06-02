using UnityEngine;
namespace CombatSystem
{
    public abstract class Skill
    {
        public readonly string NAME;
        public readonly string DISPLAY_NAME;
        public string DISPLAY_DISCRIPTION;
        public readonly string[] TAGS = { };
        /// <summary>
        /// Not every Skill needs a duration, If you do, remember to subscribe your durationTimer method to "owner.updateSkillEvent".
        /// </summary>
        public readonly float SKILL_DURATION;//in frames
        protected float skillDurationTimer = 0;
        /// <summary>
        /// Not every Skill needs a cooldown. If you do, remember to subscribe your cooldown method to "owner.updateSkillEvent".
        /// </summary>
        public readonly float COOLDOWN;//in frames
        protected float cooldownTimer = 0;

        protected CombatManager owner;
        protected Animator animator;
        protected Skill(string NAME, string DISPLAY_NAME, string DISPLAY_DESCRIPTION, string[] TAGS, float SKILL_DURATION, float COOLDOWN, CombatManager owner, Animator animator)
        {
            this.NAME = NAME;
            this.DISPLAY_NAME = DISPLAY_NAME;
            this.DISPLAY_DISCRIPTION = DISPLAY_DESCRIPTION;
            this.TAGS = TAGS;
            skillDurationTimer = 0;
            this.SKILL_DURATION = SKILL_DURATION;
            cooldownTimer = this.COOLDOWN = COOLDOWN;
            this.owner = owner;
            this.animator = animator;
        }

        public abstract void OnAdded();
        public abstract void OnRemoved();
        // This is where you implement the function of your skill,
        public abstract void Use();
    }
}