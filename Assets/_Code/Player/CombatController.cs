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

    bool shouldAttack => canAttack && Input.GetKeyDown(atkKey);
    [SerializeField] float atkCD = 0.2f;
    float attackOut = 5f;
    [SerializeField] float atkRadius = 0.5f;
    [SerializeField] KeyCode killallKey = KeyCode.E;

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

        canAttack = false;

        List<Collider> hitObjs = Physics.OverlapSphere
        (
            transform.position + transform.forward * attackOut, 
            atkRadius
        ).ToList();
        
        Debug.Log("Player Attacked");

        mas.RemovePlayerFromList(hitObjs);

        foreach(Collider col in hitObjs){
            BaseEnemy be = col.GetComponent<BaseEnemy>();
            if(be != null){
                Debug.Log($"Hit {be.gameObject.name}");
                be.TakeDamage(damage);
            }
        }

        


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
}
