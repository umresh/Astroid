using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public event Action<Asteroid, int, Vector3, int, bool> EventDie;

    private int pointsValue = 30;
    private AudioClip collisionSound;
    private float spin = 1.5f;
    private float force = 3f;
    Rigidbody2D rb;
    private int health;

    public int asteroidLevel = 3;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        force /= asteroidLevel;
        pointsValue /= asteroidLevel;
        spin /= asteroidLevel;
        rb.AddTorque(UnityEngine.Random.Range(-spin, spin), ForceMode2D.Impulse);
        AddForce();
        if (collisionSound == null)
            collisionSound = GameManager.Instance.audioFilesSO.asteroidDestroy;
        health = 1;
    }

    public void Collision(bool didHitPlayer = false)
    {
        if (didHitPlayer)
            health = 0;
        else
            health--;

        if (health != 0) return;

        if (EventDie != null)
        {
            EventDie(this, pointsValue, transform.position, asteroidLevel, didHitPlayer);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case Constants.ScreenBounds_Tag:
                AddForce();
                break;
            case Constants.Player_Tag:
                Collision(true);
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //sometimes the actroids don't bounce off and stayes on the edge
        if (other.gameObject.CompareTag(Constants.ScreenBounds_Tag))
            AddForce();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(Constants.Bullet_Tag))
        {
            other.GetComponent<Bullet>().HitAstroid();
            AudioManager.Instance.PlaySFX(collisionSound);
            Collision();
        }
    }

    private void AddForce()
    {
        if (rb.velocity.magnitude < 5)
            rb.AddForce(transform.up * (force), ForceMode2D.Impulse);
    }

    private void OnDisable()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        force = 3;
        pointsValue = 30;
        spin = 1.5f;
    }
}
