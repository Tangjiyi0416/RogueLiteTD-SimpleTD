using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;
using MovementSystem;
namespace UnitAISystem
{
    [RequireComponent(typeof(UnitCombatManager), typeof(MovementManager))]
    public class UnitAI : MonoBehaviour//transitions defined in states, not very extensible, might change it if I have time.
    {
        private State currentState;

        public StartState startState { get; private set; }
        public MoveState moveState { get; private set; }
        public CombatState combatState { get; private set; }
        public KiledState kiledState { get; private set; }
        private void Awake()
        {
            startState = new StartState(this);
            moveState = new MoveState(this);
            combatState = new CombatState(this);
            kiledState = new KiledState(this);

            SetState(startState);

        }

        public void SetState(State state)
        {
            currentState = state;
            currentState.Start();
        }
        private void Update() => currentState.Update();

    }
}