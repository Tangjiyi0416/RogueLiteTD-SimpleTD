using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnitAISystem
{
    public abstract class State
    {
        protected UnitAI unitAI;
        public State(UnitAI unitAI)
        {
            this.unitAI = unitAI;
        }
        public virtual void Start() { }
        public virtual void Update() { }
    }
}