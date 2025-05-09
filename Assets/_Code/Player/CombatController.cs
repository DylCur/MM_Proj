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
    
    [HideInInspector] public KeyCode atkKey = KeyCode.Mouse0;
    bool canAttack;
    bool shouldAttack => canAttack && Input.GetKeyDown(atkKey);
    MeshRenderer bowMesh;

    [SerializeField] KeyCode killallKey = KeyCode.E;

    [Header("Weapons")]

    [SerializeField] BowScriptable[] bows;
    BowScriptable currentWeapon; 
    bool canSwitchWeapon = true;


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

    // Parses so that if you switch weapon, it will end the cd of other weapon and not this one (this is probably bad)
    IEnumerator Attack(BowScriptable cw){

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
        if(Physics.Raycast(transform.position, Camera.main.transform.forward, out RaycastHit hit, currentWeapon.range)){
            
            BaseEnemy be = hit.collider.GetComponent<BaseEnemy>();
            
            if(be != null){
                Debug.Log($"Hit {be.gameObject.name}");
                be.TakeDamage(cw.damage);
            }
            
            else{
                ProjectileController pj = hit.collider.GetComponent<ProjectileController>();
                if(pj!=null){
                    if(pj.canBeAttacked){
                        Destroy(pj);
                    }   
                }

            }

            if(cw.e.explode){
                Collider[] cols = Physics.OverlapSphere(hit.point, cw.e.radius);

                foreach(Collider col in cols){
                    BaseEnemy te = col.GetComponent<BaseEnemy>();

                    if(te != null){
                        te.TakeDamage(cw.e.damage);
                    }
                }
            }


            
            if(hit.collider.tag == glob.enemyTag){
                Debug.Log("Hit Enemy");
            }
        }   

        yield return new WaitForSeconds(cw.atkCD);

        canAttack = true;

    }

    void Start(){
        canAttack = true;
        currentWeapon = bows[0];
    }
   
    void SwitchWeapons(){
        if(canSwitchWeapon){
            if(Input.GetKeyDown(KeyCode.Alpha1) && bows[0] != null){
                currentWeapon = bows[0];  
            }
            if(Input.GetKeyDown(KeyCode.Alpha2) && bows[1] != null){
                currentWeapon = bows[1];  
            }
            if(Input.GetKeyDown(KeyCode.Alpha3) && bows[2] != null){
                currentWeapon = bows[2];  
            }
            if(Input.GetKeyDown(KeyCode.Alpha4) && bows[3] != null){
                currentWeapon = bows[3];  
            }

            if(gameObject.transform.GetChild(2).GetComponent<MeshFilter>().mesh != currentWeapon.mesh){
                gameObject.transform.GetChild(2).GetComponent<MeshFilter>().mesh = currentWeapon.mesh;
            }


        }
    }

    void Update(){
        if(shouldAttack){
            Debug.Log("Should");
            StartCoroutine(Attack(currentWeapon));
        }

        if(Input.GetKeyDown(killallKey)){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

            foreach(GameObject gol in enemies){
                gol.GetComponent<BaseEnemy>().TakeDamage(1_000_000_000);
            }
        }

        SwitchWeapons();
    }

   
}
