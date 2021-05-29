namespace CombatSystem
{
    public abstract class Skill
    {
        public readonly string NAME;
        public readonly string DISPLAY_NAME;
        public readonly string DISPLAY_DISCRIPTION;
        public readonly string[] TAGS = { };
        public readonly int CAST_TIME;//in frames
        public readonly int PRIMARY_DURATION;//in frames
        public readonly int COOLDOWN;//in frames

        protected Skill(string NAME, string DISPLAY_NAME, string DISPLAY_DESCRIPTION, string[] TAGS, int CAST_TIME, int PRIMARY_DURATION, int COOLDOWN)
        {
            this.NAME = NAME;
            this.DISPLAY_NAME = DISPLAY_NAME;
            this.DISPLAY_DISCRIPTION = DISPLAY_DESCRIPTION;
            this.TAGS = TAGS;
            this.CAST_TIME = CAST_TIME;
            this.PRIMARY_DURATION = PRIMARY_DURATION;
            this.COOLDOWN = COOLDOWN;
        }

        public abstract void Use();
    }
}