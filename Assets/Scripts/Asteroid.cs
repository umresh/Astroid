using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public event Action<Asteroid, int, Vector3, int> EventDie;
    public event Action<Asteroid> OnPlayerHit;

    [SerializeField]
    private int pointsValue;

    [SerializeField]
    private GameObject[] childAsteroids;

    [SerializeField]
    private AudioClip collisionSound;

    [SerializeField]
    private GameObject explosionParticlesPrefab;

    private Animator asteroidAnimation;

    [SerializeField]
    private float spin = 0.5f;
    private float force = 2;
    Rigidbody2D rb;

    public int asteroidLevel = 3;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        force /= asteroidLevel;
        rb.AddTorque(UnityEngine.Random.Range(-spin, spin), ForceMode2D.Impulse);
        AddForce();
        collisionSound = GameManager.Instance.audioFilesSO.asteroidDestroy;
    }

    public void Collision(bool didHitPlayer = false)
    {
        if(health != 0 && !didHitPlayer) return;

        if (EventDie != null && !didHitPlayer)
        {
            EventDie(this, pointsValue, transform.position, asteroidLevel);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "game_bounds":
                AddForce();
                break;
            case "Player":
                OnPlayerHit?.Invoke(this);
                Collision(true);
                break;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("game_bounds"))         //sometimes the actroids don't bounce off and stayes on the edge
            AddForce();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            other.GetComponent<Bullet>().HitAstroid();
            AudioManager.Instance.PlaySFX(collisionSound);
            Collision();
        }
    }

    private void AddForce()
    {
		if(rb.velocity.magnitude < 5)
        	rb.AddForce(transform.up * (force), ForceMode2D.Impulse);
    }
}
