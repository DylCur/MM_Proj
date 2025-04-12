using UnityEngine;

public class CombatController : MonoBehaviour
{
    public int health;
    

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
}
