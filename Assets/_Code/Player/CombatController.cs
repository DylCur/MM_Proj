using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MathsAndSome;
using UnityEngine;
using Globals;

public class CombatController : MonoBehaviour
{
    public int health;
    public int damage;
 
    
    [HideInInspector] public KeyCode atkKey = KeyCode.Mouse0;
    bool canAttack;

    bool shouldAttack => canAttack && Input.GetKeyDown(atkKey);
    [SerializeField] float atkCD = 0.2f;
    [SerializeField] float attackOut = 5f;
    
    [Description("This will dictate how far up and across")]
    [SerializeField] float atkRadius = 0.5f;

    [Description("This will dictate how far out the attack goes")]
    [SerializeField] float attackRange = 3f;
    [SerializeField] KeyCode killallKey = KeyCode.E;

    void Die(){
        //TODO PlayDeathAnimation
        //TODO DoAdminStuff
        //TODO AllowForRespawn
        
    }

    public void TakeDamage(int damage){
        health -= damage;
        if(health <= 0){
            // So displays dont show it as negative
            health = 0;
            Die();
        }
    }

    IEnumerator Attack(){

        canAttack = false;
        /*
        Vector3 boxPos = transform.position + transform.forward * attackOut;
        Vector3 boxSize = new Vector3(attackRange,atkRadius,atkRadius);

        List<Collider> hitObjs = Physics.OverlapBox
        (
            boxPos, 
            boxSize,
            mas.GetPlayer().forwardObject.transform.rotation
        ).ToList();
        */
        if(Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, attackRange)){
            
            BaseEnemy be = hit.collider.GetComponent<BaseEnemy>();
            
            
            if(be != null){
                Debug.Log($"Hit {be.gameObject.name}");
                be.TakeDamage(damage);
            }
            
            else{
                ProjectileController pj = hit.collider.GetComponent<ProjectileController>();
                if(pj!=null){
                    if(pj.canBeAttacked){
                        Destroy(pj);
                    }
                }

            }
            
            if(hit.collider.tag == glob.enemyTag){
                Debug.Log("Hit Enemy");
            }
        }   

        
        /*
        Debug.Log("Player Attacked");

        mas.RemovePlayerFromList(hitObjs);

        foreach(Collider col in hitObjs){
            BaseEnemy be = col.GetComponent<BaseEnemy>();
            if(be != null){
                Debug.Log($"Hit {be.gameObject.name}");
                be.TakeDamage(damage);
            }
            else{
                ProjectileController pj = col.GetComponent<ProjectileController>();
                if(pj!=null){
                    if(pj.canBeAttacked){
                        Destroy(pj);
                    }
                }

            }
        */

        


        yield return new WaitForSeconds(atkCD);

        canAttack = true;

    }

    void Start(){
        canAttack = true;
    }
   

    void Update(){
        if(shouldAttack){
            Debug.Log("Should");
            StartCoroutine(Attack());
        }

        if(Input.GetKeyDown(killallKey)){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

            foreach(GameObject gol in enemies){
                gol.GetComponent<BaseEnemy>().TakeDamage(1_000_000_000);
            }
        }
    }

    void OnDrawGizmos()

    {  
        Vector3 boxPos = transform.position + transform.forward * attackOut;
        Vector3 boxSize = new Vector3(attackRange,atkRadius,atkRadius);

        List<Collider> hitObjs = Physics.OverlapBox
        (
            boxPos, 
            boxSize,
            Camera.main.transform.localRotation
        ).ToList();

        Gizmos.DrawWireCube(boxPos, boxSize);
    }
}
