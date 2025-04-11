using System;
using System.Collections;
using Globals;
using MathsAndSome;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class SwordEnemy : DefaultEnemy
{
    NavMeshAgent agent;
    PlayerController pc;
    CombatController cc;
    [SerializeField] float maxVerticalDistance = 10f;
    [SerializeField] float maxVerticalAttackDistance = 10f;
    bool goingToPlayer;
    [SerializeField] float goToPlayerTime = 0.1f;

    void Start(){
        agent = GetComponent<NavMeshAgent>();
        pc = mas.GetPlayer();
        playerCollider = pc.gameObject.GetComponent<CapsuleCollider>();
        cc = pc.gameObject.GetComponent<CombatController>();
    }

    public override bool shouldRoam()
    {
        // Note player distance is an abs vector
        Vector3 playerDistance = mas.PlayerDistance(pc.gameObject, gameObject);
        
        bool notInRoamRange = 
           playerDistance.x > detectionRange
        || playerDistance.z > detectionRange
        || playerDistance.y > maxVerticalDistance;

        if(s != eState.hooked && !roaming){
            return notInRoamRange;
        }
        
        return false;
    }

    public override bool shouldHunt()
    {
        Vector3 playerDistance = mas.PlayerDistance(pc.gameObject, gameObject);

        bool notInAttackRange = 
           playerDistance.x > attackRange
        || playerDistance.z > attackRange
        || playerDistance.y > maxVerticalAttackDistance;

        bool inRoamRange = 
           playerDistance.x <= detectionRange
        || playerDistance.z <= detectionRange
        || playerDistance.y <= maxVerticalDistance;

        if(s != eState.hooked && !hunting){
            return notInAttackRange && inRoamRange;
        }

        return false;
    }  

    public override void Roam()
    {
        // Debug.Log("Roaming");
        // This might have some idle code in it, for now i will leave it blank
        StartCoroutine(RoamDelay());
    }

    public override bool inAttackRange(){
        return mas.isInRadiusToPoint(transform.position, pc.transform.position, glob.playerTag, attackRange);
    }

    public override void Attack()
    {
        // If player is still in range
        
        if(canAttack){
            if(Physics.Raycast(transform.position, pc.transform.position - transform.position, out RaycastHit hit, attackRange)){
                if(hit.collider == playerCollider){
                    cc.health -= Mathf.FloorToInt(damage);
                }
                
                StartCoroutine(attackCD());
            }
        }

        // GenericAttack();
        
    }

    IEnumerator HuntDelay(){
        yield return new WaitForSeconds(0.1f);
        GenericHunt();
    }

    IEnumerator RoamDelay(){
        yield return new WaitForSeconds(0.1f);
        GenericRoam();
    }

    public override void Hunt()
    {
        
        if(!goingToPlayer){
            StartCoroutine(GetPlayer());
        }

        if(inAttackRange()){
            s = eState.attacking;
            agent.SetDestination(transform.position);
        }
        
        StartCoroutine(HuntDelay());
    }

    IEnumerator GetPlayer(){
        goingToPlayer = true;
        //
            agent.SetDestination(pc.transform.position);
       // }
       // catch(Exception e){
            
      //  }
        yield return new WaitForSeconds(goToPlayerTime);
        goingToPlayer = false;
    }
        
       
}
