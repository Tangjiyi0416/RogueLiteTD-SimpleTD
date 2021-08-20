using UnityEngine;
namespace CombatSystem
{
    public class TestStatusEffect : StatusEffect
    {

        public TestStatusEffect(CombatManager origin, CombatManager target, int duration)
        : base("", "DISPLAY_NAME", "DISPLAY_DISCRIPTION", 10f, origin, target, duration) { }
        public override void OnAdded()
        {
            target.updateStatusEffectEvent += Effect;
        }
        public override void OnRemoved()
        {
            target.updateStatusEffectEvent -= Effect;
        }

        public override void Effect()
        {
            Debug.Log(durationTimer);
            base.Effect();
        }
    }
}