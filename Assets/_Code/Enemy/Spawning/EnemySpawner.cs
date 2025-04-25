using System.Collections;
using System.Collections.Generic;
using Globals;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{

    public enum EType{
        SwordsMan,
        Archer,
        Menace
    }

    [Header("Prefabs")]
    [SerializeField] GameObject swordsMan;
    [SerializeField] GameObject archer;
    [SerializeField] GameObject menace;
    Dictionary<EType, GameObject> EnemyTypeMappings = new Dictionary<EType, GameObject>();

    List<BaseEnemy> currentEnemies = new List<BaseEnemy>();
    [SerializeField] List<GameObject> doors; 

    int wave = 0;
    bool waveSpawned;

    [System.Serializable]
    public struct Enemy{
        public EType baseEnemy;             // This contains the enemy type
        public GameObject spawnObject;      // This contains the spawn position of the object, stored in a Game Object
    }

    [System.Serializable]
    public struct Wave{
        public List<Enemy> Enemies;
        public float waveDelay;
    }

    [SerializeField] List<Wave> Waves;

    void Start(){

        if(swordsMan == null || archer == null || menace == null){
            Debug.LogError($"Once or more enemies are null in {gameObject.name}");
        }

        EnemyTypeMappings = new Dictionary<EType, GameObject>(){
            {EType.SwordsMan , swordsMan},
            {EType.Archer , archer},
            {EType.Menace , menace},
        };
    }

    void Update(){
        if(isWaveDead()){
            // THis might be wrong i cant maths today (13/04/2025 | 9:44PM)
            // This should see if the next wave exists
            // Every enemy spawner - 1 will open a door so i have to make a whole abstract system 
            // (on the doors) oml
            if(wave + 1 > Waves.Count - 1){
                foreach(GameObject door in doors){
                    DoorController dc = door.GetComponent<DoorController>();
                    if(dc != null){
                        dc.OpenDoor();
                    }
                    else{
                        Debug.Log($"The door controller on {door} is null");
                    }
                }
            }
            else{
                IncrementWave();
            }
        }
    }

    bool isWaveDead(){
        if(currentEnemies.Count > 0 && waveSpawned){
            // If there is an alive enemy, return false
            foreach(BaseEnemy be in currentEnemies){
                if(be.s != BaseEnemy.EState.dead){
                    return false;
                }
            }

            // Else return true
            return true;
        }

        // If there are no enemies or the wave hasnt fully spawned
        return false;
    }


    IEnumerator SpawnWave(){

        waveSpawned = false;
        Wave currentWave = Waves[wave];

        yield return new WaitForSeconds(currentWave.waveDelay);
        
        List<Enemy> ce = currentWave.Enemies;

        for(int i = 0; i < currentWave.Enemies.Count; i++){
            // Genuinely no idea why the enemies are spawning twice but they are and this fixed it
            if(currentEnemies.Count == currentWave.Enemies.Count){
                break;
            }
            else{
                GameObject e = Instantiate(EnemyTypeMappings[ce[i].baseEnemy], ce[i].spawnObject.transform.position, Quaternion.identity);
                currentEnemies.Add(e.GetComponent<BaseEnemy>());
                Debug.Log($"Spawned {e} at {i}");
            }
            
        }

        waveSpawned = true;
    }

    void IncrementWave(){
        wave++;
        StartCoroutine(SpawnWave());
    }


   

    void OnTriggerEnter(Collider other){
        if(other.tag == glob.playerTag){
            
            foreach(GameObject door in doors){
                DefaultDoor d = door.GetComponent<DefaultDoor>();
                StartCoroutine(d.CloseDoor());
            }

            StartCoroutine(SpawnWave());
            Destroy(GetComponent<BoxCollider>());
        }
    }
    
}
