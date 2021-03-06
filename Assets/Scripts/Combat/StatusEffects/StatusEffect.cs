namespace CombatSystem
{
    //StatusEffect is the base class for any combat-related Effects you want to implement
    public abstract class StatusEffect
    {
        public readonly string NAME;
        public readonly string DISPLAY_NAME;
        public string DISPLAY_DESCRIPTION;
        public readonly float DEFAULT_DURATION;

        public float duration;
        public float durationTimer;

        protected CombatManager origin;
        protected CombatManager target;
        /// <param name = "duration">in frames</param>
        /// <summary>Sets origin, and the duration of this effect, the effect will expire when the effectDurationTimer goes zero.</summary>

        protected StatusEffect(string NAME, string DISPLAY_NAME, string DISPLAY_DISCRIPTION, float DEFAULT_DURATION, CombatManager origin, CombatManager target, float duration)
        {
            this.NAME = NAME;
            this.DISPLAY_NAME = DISPLAY_NAME;
            this.DISPLAY_DESCRIPTION = DISPLAY_DISCRIPTION;
            this.DEFAULT_DURATION = DEFAULT_DURATION;
            this.origin = origin;
            this.target = target;
            this.durationTimer = this.duration = duration;

        }

        public abstract void OnAdded();
        public abstract void OnRemoved();
        public virtual void Effect()
        {
            if (durationTimer <= 0f)
            {
                target.RemoveStatusEffect(this.NAME);
                OnRemoved();
            }
            durationTimer--;
        }

    }
}