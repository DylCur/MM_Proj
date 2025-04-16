using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathsAndSome;
using UnityEngine;


public class CombatController : MonoBehaviour
{
    public int health;
    public int damage;
 
    
    [HideInInspector] public KeyCode atkKey = KeyCode.Mouse0;
    bool canAttack;

    bool shouldAttack => canAttack && Input.GetKey(atkKey);
    [SerializeField] float atkCD = 0.2f;
    float attackOut = 5f;
    [SerializeField] float atkRadius = 0.5f;

    void Die(){
        // PlayDeathAnimation
        // DoAdminStuff
        // AllowForRespawn
        
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

        List<Collider> hitObjs = Physics.OverlapSphere
        (
            transform.position + transform.forward * attackOut, 
            atkRadius
        ).ToList();

       mas.RemovePlayerFromList(hitObjs);

       foreach(Collider col in hitObjs){
            BaseEnemy be = col.GetComponent<BaseEnemy>();
            if(be != null){
                be.TakeDamage(damage);
            }
       }

        


        yield return new WaitForSeconds(atkCD);
    }

   

    void Update(){
        if(shouldAttack){
            StartCoroutine(Attack());
        }
    }
}
