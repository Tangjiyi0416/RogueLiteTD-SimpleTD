namespace CombatSystem
{
    public abstract class Skill
    {
        public readonly string NAME;
        public readonly string DISPLAY_NAME;
        public string DISPLAY_DISCRIPTION;
        public readonly string[] TAGS = { };
        public readonly int CAST_TIME;//in frames
        public readonly int SKILL_DURATION;//in frames
        protected int skillDurationTimer = 0;
        public readonly int COOLDOWN;//in frames
        protected int cooldownTimer = 0;

        protected CombatManager owner;

        protected Skill(string NAME, string DISPLAY_NAME, string DISPLAY_DESCRIPTION, string[] TAGS, int CAST_TIME, int SKILL_DURATION, int COOLDOWN, CombatManager owner)
        {
            this.NAME = NAME;
            this.DISPLAY_NAME = DISPLAY_NAME;
            this.DISPLAY_DISCRIPTION = DISPLAY_DESCRIPTION;
            this.TAGS = TAGS;
            this.CAST_TIME = CAST_TIME;
            skillDurationTimer = this.SKILL_DURATION = SKILL_DURATION;
            cooldownTimer = this.COOLDOWN = COOLDOWN;
            this.owner = owner;
        }
        public abstract void PrepareSkill();
        public abstract void Use();
    }
}