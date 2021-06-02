using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CombatSystem;

[RequireComponent(typeof(CombatManager), typeof(Animator))]
public class Tower : MonoBehaviour
{
    private CombatManager combatManager;
    private Animator animator;
    public CombatData combatData;
    public List<Modifier> modifiers = new List<Modifier>();
    public Skill defaultAttack;
    public List<Skill> skills = new List<Skill>();
    private void Start()
    {
        combatManager = GetComponent<CombatManager>();
        animator = GetComponent<Animator>();
        //testing only
        defaultAttack = new TestDefaultAttackSkill(combatManager, animator);
        skills.Add(new TestFireAttackSkill(combatManager, animator));
        skills.Add(new TestPassiveSkill(combatManager,animator));
        //testing only
        combatManager.Initialize(combatData, modifiers, defaultAttack, skills);
    }

    private void Update()
    {

    }

    public void UseSkill(int skillIndex)
    {
        Debug.Log($"UsingSkill: {skills[skillIndex].DISPLAY_NAME}, it will last {skills[skillIndex].SKILL_DURATION} frames.");
        animator.Play($"skill_{skillIndex}");
    }

    public void SelfHit(){
        combatManager.ReceiveHit(new Hit(new Phases(100),new Phases(100),new Phases(100),combatManager,combatManager));
    }

}
