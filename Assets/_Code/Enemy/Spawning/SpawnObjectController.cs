using UnityEngine;

public class SpawnObjectController : MonoBehaviour
{
    void Start(){
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshFilter>());
    }
}
