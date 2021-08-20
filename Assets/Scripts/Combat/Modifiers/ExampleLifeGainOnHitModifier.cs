using UnityEngine;
namespace CombatSystem
{
    public class ExampleLifeGainOnHitModifier : Modifier
    {

        private const int lifeGainAmount = 100;

        public ExampleLifeGainOnHitModifier() 
        : base("example_life_gain_on_hit_modifier", "Example Life Gain On Hit Modifier", $" You gain {lifeGainAmount}", new string[]{})
        {
        }

        public override void OnAdded(CombatManager owner)
        {
            owner.onHitEvent += LifeGainOnHit;
        }
        public override void OnRemoved(CombatManager owner)
        {
            owner.onHitEvent -= LifeGainOnHit;
        }

        //The method that do stuff when there is an incoming hit
        public void LifeGainOnHit(Hit hit)
        {
            hit.origin.ReceiveHeal(lifeGainAmount);
            Debug.Log($"{hit.origin.gameObject.name} 擊回 {lifeGainAmount} 點血");

        }
    }
}