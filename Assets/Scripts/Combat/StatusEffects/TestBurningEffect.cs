using UnityEngine;
namespace CombatSystem
{
    public class TestBurningStatusEffect : StatusEffect
    {

        int fireDamageNumber;
        public TestBurningStatusEffect(CombatManager origin, CombatManager target, float duration, int fireDamageNumber)
        : base("test_burning_statuseffect", "Test Burning Status Effect", "You are burning.....", 10f, origin, target, duration)
        {
            this.fireDamageNumber = fireDamageNumber;
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
            Debug.Log($"{target.name} is burning, taking {fireDamageNumber} fire damage per frame, {durationTimer} frames left.");
            target.combatData.life -= CombatManager.CalFinalDamgeFromValues(
                new Phases(0, 0, 0, fireDamageNumber, 0)
                , origin.combatData.totalDamageIncrement
                , origin.combatData.totalDamageMultiplier
                , target.combatData.resistence
                ).Total;
            base.Effect();
        }
    }
}