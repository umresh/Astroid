using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerScriptableObject", menuName = "Learning_yogi/PlayerControllerScriptableObject", order = 0)]
public class PlayerDataScriptableObject : ScriptableObject {
    public int playerLives;
	public float maxVelocity;
	public float rotationSpeed;
	public float friction;
	public float acceleration;
}

