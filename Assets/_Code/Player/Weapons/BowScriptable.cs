using UnityEngine;

[CreateAssetMenu(fileName = "Bow", menuName = "Bow/Create Empty Bow", order = 0)]
public class BowScriptable : ScriptableObject
{
    public int damage;
    public float range;
    public float pullOutSpeed;
    public float putAwaySpeed;
    public float atkCD;
    public Mesh mesh;

    [System.Serializable]
    public struct explosion{
        public bool explode;
        public float radius;
        // Player should take less damage than the enemies
        public int damage;
    }

    [System.Serializable]
    public struct homing{
        public bool home;
        public float range;
        public float force;
        public float maxTime;
    }

    [Header("Explosion")]
    public explosion e;
    [Header("Homing")]
    public homing h;
    
}
