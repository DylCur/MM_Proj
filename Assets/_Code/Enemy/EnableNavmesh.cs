using Unity.AI.Navigation;
using UnityEngine;

public class EnableNavmesh : MonoBehaviour
{
    void Start(){
        GetComponent<NavMeshSurface>().enabled = true;
    }
}
