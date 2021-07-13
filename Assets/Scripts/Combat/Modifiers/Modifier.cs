using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EntitySystem
{
    //TODO: add template gameobject to handle the display of modifier
    public abstract class Modifier
    {
        public readonly string NAME;
        public readonly string DISPLAY_NAME;
        public string DISPLAY_DESCRIPTION;
        public readonly string[] TAGS;

        protected Modifier(string NAME, string DISPLAY_NAME, string DISPLAY_DESCRIPTION, string[] TAGS)
        {
            this.NAME = NAME;
            this.DISPLAY_NAME = DISPLAY_NAME;
            this.DISPLAY_DESCRIPTION = DISPLAY_DESCRIPTION;
            this.TAGS = TAGS;
        }

        /// <summary>Called when added to a CombatManager.</summary>
        public abstract void OnAdded(Entity owner);
        /// <summary>
        /// <para>Called when removed from a CombatManager, remember to undo anything that you have done in Modify() here.</para>
        /// <para>If you want to do some permanent effect, THEN GO USE STATUS EFFECT !</para>
        /// </summary>
        public abstract void OnRemoved(Entity owner);
    }
}