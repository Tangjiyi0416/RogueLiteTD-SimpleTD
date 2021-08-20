using UnityEngine;

namespace CombatSystem
{
    public class ExampleAddedFireDamageModifier : Modifier
    {
        private int addedFireDamage = 50;

        public ExampleAddedFireDamageModifier() : base("example_added_fire_damage_modifier"
         , "Example Added Fire Damage Modifier"
         , "NO DESCRIPTION"
         , new string[] { "added_damage", "fire" })
        {
        }

        //Call when added to a CombatManager
        public override void OnAdded(CombatManager owner)
        {
            owner.combatData.damage[((int)DamageType.Types.Fire)].ScaleBaseValue(addedFireDamage);
        }
        //Call when removed from a CombatManager
        public override void OnRemoved(CombatManager owner)
        {
            owner.combatData.damage[((int)DamageType.Types.Fire)].ScaleBaseValue(-addedFireDamage);

        }
    }


}