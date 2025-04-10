using UnityEngine;
using MathsAndSome;
using Globals;
using System.Collections;

public abstract class DefaultEnemy : MonoBehaviour
{
    public enum eState{
        roaming,
        hunting,
        hooked,
        attacking
    }   

    public float health = 100f;
    public float damage = 10f;
    public float attackTime = 0.2f;

    public float attackRange = 100f;
    public float detectionRange = 1000f;

    public eState s = eState.roaming;

    // The code should work so that the action is determined by the state alone, and the state is set based on conditions
    
    public abstract bool shouldRoam();
    public abstract bool shouldHunt();
    public abstract bool inAttackRange();

    public abstract void Roam();
    public abstract void Hunt();
    public abstract void Attack();

    [HideInInspector] public bool canAttack = true;

    [HideInInspector] public bool hunting;
    [HideInInspector] public bool roaming;
    [HideInInspector] public bool attacking;

    public CapsuleCollider playerCollider;


    public IEnumerator attackCD(){
        canAttack = false;
        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }


    public void GenericRoam(){
        if(s == eState.roaming){
            Roam();
        }
    }

    public void GenericAttack(){
        if(s == eState.attacking){
            Attack();
        }
    }

    public void GenericHunt(){
        if(s == eState.hunting){
            Hunt();
        }
    }

    void CallActions(){
       
        if(s == eState.roaming && !roaming){
            roaming=true;
            Roam();
        }

        if(s == eState.hunting && !hunting){
            hunting=true;
            Hunt();
        }

        if(s == eState.attacking && !attacking){
            attacking = true;
            Attack();
        }
    }

    void Update(){
        if(shouldRoam()){
            s = eState.roaming;
            hunting = false;
            attacking = false;
        }
        
        if(shouldHunt()){
            s = eState.hunting;
            roaming = false;
            attacking = false;
        }

        CallActions();
    }


}
