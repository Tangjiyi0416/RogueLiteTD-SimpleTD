using UnityEngine;
namespace CombatSystem
{
    public class TestStatusEffect : StatusEffect
    {
        public TestStatusEffect(CombatManager origin, CombatManager target, int duration)
            : base(origin, target, duration) { }
        public override void Effect()
        {
            throw new System.NotImplementedException();
        }
    }
}