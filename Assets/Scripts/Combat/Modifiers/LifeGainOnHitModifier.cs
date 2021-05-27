using UnityEngine;
namespace CombatSystem
{
    public class LifeGainOnHitModifier : Modifier
    {
        private int duration = 20;
        private int lifeGainAmount = 100;

        public override void Modify(CombatManager owner)
        {
            owner.onHitEffects += LifeGainOnHit;
        }
        public override void UnModify(CombatManager owner)
        {
            owner.onHitEffects -= LifeGainOnHit;
        }

        //The method that do stuff when there is an incoming hit
        public void LifeGainOnHit(Hit hit)
        {
            hit.origin.life += lifeGainAmount;
        }
    }
}