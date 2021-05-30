using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CombatSystem
{
    public delegate void OnHitEvent(Hit hit);
    public delegate void WhenHitEvent(Hit hit);
    public delegate void UpdateStatusEffectEvent();

    //Every tower and creep have one and only one CombatManager, it handles all combat related stuff except moving.
    //CombatManager fuctions as a container with some extra ability, so it doesn't "generate" stats like life, defence, baseDamage, etc. on its own.
    //When a tower/creep spawns, all those data needs to be assgined to the corresponding fields in CombatManager(Maybe a default modifier?).
    public abstract class CombatManager : MonoBehaviour
    {
        public int life = 0;
        public int maxLife = 0;
        public int defence = 0;
        public Phases baseDamage = new Phases(0);
        public Phases totalDamageIncrement = new Phases(100);
        public Phases totalDamageMultiplier = new Phases(100);
        public Phases Resistence = new Phases(0);
        protected List<Modifier> modifiers = new List<Modifier>();
        protected List<StatusEffect> statusEffects = new List<StatusEffect>();
        public List<Skill> skills = new List<Skill>();//If count >= 1, skills[0] should be the default attack
        public Skill currentSkill;
        public virtual void UseCurrentSkill()
        {
            currentSkill?.Use();
        }
        //Schedule a skill that will replace the currentSkill, so it will be called by the next Attack().
        //Notice that after you replace the currentSkill, it will not automatically change back
        //, this allows you to make more complex skill while requires more caution when dealing with.
        //JUST DON'T FORGET TO ADD AND CALL SOME SORT OF "RESET CURRENT SKILL METHOD" IN YOUR SKILLS!!!
        public virtual void ScheduleSkill(string skillName)
        {
            Debug.Log($"ScheduleSkill: {skillName}");
            currentSkill = skills.Where(x => x.NAME == skillName).First();
            currentSkill.PrepareSkill();
            Debug.Log($"CurrentSkill: {currentSkill.DISPLAY_NAME}, it will last {currentSkill.SKILL_DURATION} frames.");

        }
        public virtual void ResetCurrentSkillToDefaultSkill()
        {
            currentSkill = skills.Count > 0 ? skills[0] : null;
            currentSkill.PrepareSkill();
        }
        public event OnHitEvent onHitEvent;
        public event WhenHitEvent whenHitEvent;
        public event UpdateStatusEffectEvent updateStatusEffectEvent;

        public void TriggerOnHitEvent(Hit hit)
        {
            onHitEvent?.Invoke(hit);
        }

        public void TriggerWhenHitEvent(Hit hit)
        {
            whenHitEvent?.Invoke(hit);
        }
        public virtual void ReceiveHit(Hit hit)
        {
            hit.origin.TriggerOnHitEvent(hit);
            life -= CombatManager.CalFinalDamgeFromHit(hit).Total;
            Debug.Log($"{gameObject.name} 被 {hit.origin.currentSkill} Hit, 受到 {CombatManager.CalFinalDamgeFromHit(hit).Total} 點傷害");
            TriggerWhenHitEvent(hit);
            if (IsDead()) Dead();
        }

        public virtual void ReceiveHeal(int number)
        {
            life += number;
            life = life > maxLife ? maxLife : life;
        }

        public CombatManager tmp;
        public virtual void LifeGain()
        {
            ReceiveModifier(new ExampleLifeGainOnHitModifier());
        }
        public virtual void LifeLose()
        {
            RemoveModifier("example_life_gain_on_hit_modifier");
        }
        public virtual void ttt()
        {
            tmp.ReceiveHit(new Hit(baseDamage, totalDamageIncrement * (new Phases(50)), totalDamageMultiplier, this, tmp));
        }
        public virtual void ReceiveStatusEffect(StatusEffect effect)
        {
            foreach (StatusEffect e in statusEffects)
            {
                if (e.NAME == effect.NAME)
                {
                    e.durationTimer = e.duration;
                    return;
                }
            }
            effect.OnAdded();
            statusEffects.Add(effect);
        }
        protected virtual void TriggerUpdateStatusEffectEvent()
        {
            updateStatusEffectEvent?.Invoke();
        }
        public virtual void RemoveStatusEffect(string effectName)
        {
            foreach (StatusEffect e in statusEffects)
            {
                if (e.NAME == effectName)
                {
                    e.OnRemoved();
                    statusEffects.Remove(e);
                    return;
                }
            }
        }
        public virtual void ReceiveModifier(Modifier modifier)
        {

            modifier.OnAdded(this);
            modifiers.Add(modifier);
            Debug.Log($"{gameObject.name} get {modifier.DISPLAY_NAME}");

        }
        public virtual void RemoveModifier(string modifierName)
        {
            foreach (Modifier m in modifiers)
            {
                if (m.NAME == modifierName)
                {
                    m.OnRemoved(this);
                    modifiers.Remove(m);
                    Debug.Log($"{gameObject.name} lose {m.DISPLAY_NAME}");

                    return;
                }
            }

        }

        public bool IsDead()
        {
            return life <= 0 ? true : false;
        }

        protected virtual void Dead()
        {

            GameObject.Destroy(gameObject);
        }

        //Utility Fuctions
        public static Phases CalFinalDamgeFromHit(Hit hit)
        {
            return hit.baseDamage * hit.totalDamageIncrement * hit.totalDamageMultiplier * (new Phases(100) - hit.target.Resistence) / (new Phases(1000000));
        }
        public static Phases CalFinalDamgeFromValues(Phases baseDamage, Phases totalDamageIncrement, Phases totalDamageMultiplier, Phases targetResistence)
        {
            return baseDamage * totalDamageIncrement * totalDamageMultiplier * (new Phases(100) - targetResistence) / (new Phases(1000000));
        }
    }
    [Serializable]
    public class Phases //五行
    {
        public int metal, wood, water, fire, earth;
        public int Total { get => metal + wood + water + fire + earth; }
        public Phases(int n)
        {
            this.metal = this.wood = this.water = this.fire = this.earth = n;
        }
        public Phases(int metal, int wood, int water, int fire, int earth)
        {
            this.metal = metal;
            this.wood = wood;
            this.water = water;
            this.fire = fire;
            this.earth = earth;
        }

        public static Phases operator +(Phases a) => a;
        public static Phases operator -(Phases a) => new Phases(-a.metal, -a.wood, -a.water, -a.fire, -a.earth);
        public static Phases operator +(Phases a, Phases b)
            => new Phases(a.metal + b.metal, a.wood + b.wood, a.water + b.water, a.fire + b.fire, a.earth + b.earth);
        public static Phases operator -(Phases a, Phases b)
            => a + (-b);
        public static Phases operator *(Phases a, Phases b)
            => new Phases(a.metal * b.metal, a.wood * b.wood, a.water * b.water, a.fire * b.fire, a.earth * b.earth);
        public static Phases operator /(Phases a, Phases b)
        {
            if (b.metal == 0 || b.wood == 0 || b.water == 0 || b.fire == 0 || b.earth == 0) throw new DivideByZeroException();

            return new Phases(a.metal / b.metal, a.wood / b.wood, a.water / b.water, a.fire / b.fire, a.earth / b.earth);
        }
    }

    public class Hit
    {
        public Phases baseDamage;
        public Phases totalDamageIncrement;
        public Phases totalDamageMultiplier;
        public CombatManager origin;
        public CombatManager target;

        public Hit(Phases baseDamage, Phases totalDamageIncrement, Phases totalDamageMultiplier, CombatManager origin, CombatManager target)
        {
            this.baseDamage = baseDamage;
            this.totalDamageIncrement = totalDamageIncrement;
            this.totalDamageMultiplier = totalDamageMultiplier;
            this.origin = origin;
            this.target = target;
        }
    }
}