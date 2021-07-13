using UnityEngine;
namespace EntitySystem
{
    public class TestBurningStatusEffect : StatusEffect
    {

        int fireDamage;
        public TestBurningStatusEffect(Entity origin, Entity target, float duration, int fireDamage)
        : base("test_burning_statuseffect", "Test Burning Status Effect", "You are burning.....", 10f, origin, target, duration)
        {
            this.fireDamage = fireDamage;
        }
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
            Debug.Log($"{target.name} is burning, taking {fireDamage} fire damage per frame, {durationTimer} frames left.");

            target.combatData.life -= fireDamage;
            base.Effect();
        }
    }
}