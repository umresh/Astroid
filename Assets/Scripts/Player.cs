using System;
using System.Collections;
using UnityEngine;

public class Player : WrapPlayableArea
{
    public event Action OnPlayerDied;

    [SerializeField]
    private GameObject shipExplosionGO;
    [SerializeField]
    private GameObject playerContainerGO;
    public Bullet currentActiveWeapon;
    Animator explosionAnimator;
    Coroutine explosionCoroutine;

    private PlayerController controller;

    public override void Start()
    {
        base.Start();
        explosionAnimator = shipExplosionGO.GetComponent<Animator>();
    }

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        AsteroidManager.OnAstriodHitPlayer += OnCollisionWithAsteroid;
    }

    public void Spawn()
    {
        gameObject.transform.position = Vector3.zero;
        playerContainerGO.SetActive(true);
        shipExplosionGO.SetActive(false);
    }

    private void OnCollisionWithAsteroid()
    {
        controller.Reset();
        shipExplosionGO.SetActive(true);
        playerContainerGO.SetActive(false);
        StartCoroutine(WaitForPlayerRespawn());
        AudioManager.Instance.PlaySFX(GameManager.Instance.audioFilesSO.playerExplode);
    }

    IEnumerator WaitForPlayerRespawn()
    {
        AnimatorStateInfo animState = explosionAnimator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animState.length);
        OnPlayerDied?.Invoke();
    }
}
