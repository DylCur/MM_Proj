using UnityEngine;
using System.Collections;

public abstract class DoorController : MonoBehaviour
{
    public abstract void OpenDoor();
    public abstract IEnumerator CloseDoor();

}
