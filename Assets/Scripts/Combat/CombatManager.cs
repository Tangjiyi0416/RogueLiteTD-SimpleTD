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
    public delegate void UpdateSkillEvent();

    //Every tower and creep have one and only one CombatManager, it handles all combat related stuff except moving.
    //CombatManager fuctions as a container with some extra ability, so it doesn't "generate" stats like life, defence, baseDamage, etc. on its own.
    //When a tower/creep spawns, all those data needs to be assgined to the corresponding fields in CombatManager(Maybe a default modifier?).
    public abstract class CombatManager : MonoBehaviour
    {
        public CombatData combatData;

        public Skill defaultAttack;
        protected List<Modifier> modifiers = new List<Modifier>();
        protected List<StatusEffect> statusEffects = new List<StatusEffect>();
        protected List<Skill> skills = new List<Skill>();//If count >= 1, skills[0] should be the default attack
        public virtual void Initialize(CombatData combatData, List<Modifier> modifiers, Skill defaultAttack, List<Skill> skills)
        {
            this.combatData = combatData;
            foreach (var m in modifiers) ReceiveModifier(m);
            this.defaultAttack = defaultAttack;
            defaultAttack.OnAdded();
            foreach (var s in skills) ReceiveSkill(s);

        }
        protected void Update()
        {
            TriggerUpdateStatusEffectEvent();
            TriggerUpdateSkillEvent();
        }

        public event OnHitEvent onHitEvent;
        public event WhenHitEvent whenHitEvent;
        public event UpdateStatusEffectEvent updateStatusEffectEvent;
        public event UpdateSkillEvent updateSkillEvent;

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
            combatData.life -= CombatManager.CalFinalDamgeFromHit(hit).Total;
            Debug.Log($"{gameObject.name} 被 {hit.origin.gameObject} Hit, 受到 {CombatManager.CalFinalDamgeFromHit(hit).Total} 點傷害");
            TriggerWhenHitEvent(hit);
            if (IsDead()) Dead();
        }

        public virtual void ReceiveHeal(int number)
        {
            combatData.life += number;
            combatData.life = combatData.life > combatData.maxLife ? combatData.maxLife : combatData.life;
        }
        public virtual void LifeGain()
        {
            ReceiveModifier(new ExampleLifeGainOnHitModifier());
        }
        public virtual void LifeLose()
        {
            RemoveModifier("example_life_gain_on_hit_modifier");
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

        public virtual void UseSkill(int skillIndex)
        {
            skills[skillIndex].Use();
        }
        public virtual void ReceiveSkill(Skill skill)
        {

            skill.OnAdded();
            skills.Add(skill);
            Debug.Log($"{gameObject.name} get {skill.DISPLAY_NAME}");

        }
        protected virtual void TriggerUpdateSkillEvent()
        {
            updateSkillEvent?.Invoke();
        }
        public virtual void RemoveSkill(int skillIndex)
        {
            skills[skillIndex].OnRemoved();
            Debug.Log($"{gameObject.name} lose {skills[skillIndex].DISPLAY_NAME}");
            skills.RemoveAt(skillIndex);

        }
        public bool IsDead()
        {
            return combatData.life <= 0 ? true : false;
        }

        protected virtual void Dead()
        {

            GameObject.Destroy(gameObject);
        }

        //Utility Fuctions
        public static Phases CalFinalDamgeFromHit(Hit hit)
        {
            return hit.baseDamage * hit.totalDamageIncrement * hit.totalDamageMultiplier * (new Phases(100) - hit.target.combatData.resistence) / (new Phases(1000000));
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

    [Serializable]
    public class CombatData
    {
        public int life = 0;
        public int maxLife = 0;
        public int defence = 0;
        public float attackSpeed = 1;//in frames
        public Phases baseDamage = new Phases(0);
        public Phases totalDamageIncrement = new Phases(100);
        public Phases totalDamageMultiplier = new Phases(100);
        public Phases resistence = new Phases(0);

        public CombatData()
        {
            life = 0;
            maxLife = 0;
            defence = 0;
            attackSpeed = 100;
            baseDamage = new Phases(0);
            totalDamageIncrement = new Phases(100);
            totalDamageMultiplier = new Phases(100);
            resistence = new Phases(0);
        }

        public CombatData(int life, int maxLife, int defence, int attackSpeed, Phases baseDamage, Phases totalDamageIncrement, Phases totalDamageMultiplier, Phases resistence)
        {
            this.life = life;
            this.maxLife = maxLife;
            this.defence = defence;
            this.attackSpeed = attackSpeed;
            this.baseDamage = baseDamage;
            this.totalDamageIncrement = totalDamageIncrement;
            this.totalDamageMultiplier = totalDamageMultiplier;
            this.resistence = resistence;
        }
    }
}