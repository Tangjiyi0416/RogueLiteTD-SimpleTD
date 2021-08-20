using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public abstract class Attack
    {
        public string Name { get; }
        public string DisplayName { get; }
        public string DisplayDiscription { get; }


        public bool IsAttacking { get; protected set; }
        /// <summary>
        /// Attack takes time, cooldown begins when attack time finished.
        /// </summary>
        /// <value></value>
        public float AttackTime { get; protected set; }//in seconds
        protected float attackTimeTimer;

        public bool IsInCooldown { get; protected set; }
        /// <summary>
        /// Rest time after an attack.
        /// </summary>
        /// <value></value>
        public float Cooldown { get; protected set; }//in seconds
        protected float cooldownTimer;

        protected TargetingHandler targetingHandler;
        public CombatManager owner;
        protected Animator animator;

        protected Attack(string Name, string DisplayName, string DisplayDiscription, float attackTime, float cooldown, TargetingHandler targetingHandler, CombatManager owner, Animator animator)
        {
            this.Name = Name;
            this.DisplayName = DisplayName;
            this.DisplayDiscription = DisplayDiscription;
            cooldownTimer = this.Cooldown = cooldown;
            attackTimeTimer = this.AttackTime = attackTime;
            IsAttacking = false;
            IsInCooldown = true;
            this.targetingHandler = targetingHandler;
            this.owner = owner;
            this.animator = animator;
        }

        public abstract void OnAdded();
        public abstract void OnRemoved();
        public virtual void AttackUpdate()
        {
            if (IsInCooldown)
            {
                if (cooldownTimer <= 0f)//cooldown completed, switch state to duration
                {
                    //state settings
                    IsAttacking = true;
                    IsInCooldown = false;
                    attackTimeTimer = AttackTime;
                    OnCooldownEnded();
                    OnAttackStarted();
                }
                cooldownTimer -= owner.combatData.attackSpeed.FinalValue * GameManager.Instance.gameDeltaTime;
            }
            else if (IsAttacking)
            {
                if (attackTimeTimer <= 0f)//duration ended, switch state to cooldown
                {
                    //state settings
                    IsAttacking = false;
                    IsInCooldown = true;
                    cooldownTimer = Cooldown;
                    OnAttackEnded();
                    OnCooldownStarted();
                }
                attackTimeTimer -= owner.combatData.attackSpeed.FinalValue * GameManager.Instance.gameDeltaTime;

            }
        }
        protected virtual void OnCooldownStarted()
        {
            Debug.Log($"{DisplayName}'s cooldown has started.");
        }
        protected virtual void OnCooldownEnded()
        {
            Debug.Log($"{DisplayName}'s cooldown has ended.");
        }
        protected virtual void OnAttackStarted()
        {
            Debug.Log($"{DisplayName}'s Attack has started.");
        }
        protected virtual void OnAttackEnded()
        {
            Debug.Log($"{DisplayName}'s Attack has ended.");
        }
    }

}
