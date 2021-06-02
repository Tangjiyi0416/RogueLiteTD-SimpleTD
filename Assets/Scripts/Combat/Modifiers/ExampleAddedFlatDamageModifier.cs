using UnityEngine;

namespace CombatSystem
{
    public class ExampleAddedFlatDamageModifier : Modifier
    {
        private Phases addedBaseDamage = new Phases(0, 100, 111, 456, 45);

        public ExampleAddedFlatDamageModifier()
         : base("example_added_flat_damage_modifier", "Example Added Flat Damage Modifier", "NO DESCRIPTION", new string[]{})
        {
        }

        //Call when added to a CombatManager
        public override void OnAdded(CombatManager owner)
        {
            owner.combatData.baseDamage += addedBaseDamage;
        }
        //Call when removed from a CombatManager
        public override void OnRemoved(CombatManager owner)
        {
            owner.combatData.baseDamage += addedBaseDamage;
        }
    }


}