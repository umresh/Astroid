using UnityEngine;

[CreateAssetMenu(fileName = "WeaponsAttributesSO", menuName = "Learning_yogi/WeaponAttributes", order = 0)]
public class WeaponDataScriptableObject : ScriptableObject
{
    public bool burst;
    public int burstCount;
    public float fireRate;
    public int damage;
    public float bulletSpeed;
    public AudioClip fireAudioClip;
}
