using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerDataScriptableObject playerControllerData;

    private float maxVelocity = 5.0f;
    private float rotationSpeed = 150.0f;
    private float friction = 1f;
    private float acceleration = 5.0f;
    private Vector3 velocity;
    private Vector3 clampedVelocity;

    void Start()
    {
        playerControllerData = GameManager.Instance.playerDataSO;

        maxVelocity = playerControllerData.maxVelocity;
        rotationSpeed = playerControllerData.rotationSpeed;
        friction = playerControllerData.friction;
        acceleration = playerControllerData.acceleration;

        Reset();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        //Clamp the value to prevent negative values.
        float inputY = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);

        transform.Rotate(0, 0, (-inputX * rotationSpeed * Time.deltaTime));
        if (inputY != 0.0f)     
            velocity += (inputY * (transform.up * acceleration)) * Time.deltaTime;

        // apply friction to stop the ship after sometime.
        if (inputY == 0.0f)
        {
            velocity *= friction;
        }

        clampedVelocity = Vector3.ClampMagnitude(velocity, maxVelocity);            //Clamp the velecity to max.
        transform.Translate(clampedVelocity * Time.deltaTime, Space.World);
    }

    public void Reset()
    {
        velocity = new Vector3(0, 0, 0);
        clampedVelocity = new Vector3(0, 0, 0);
    }
}
