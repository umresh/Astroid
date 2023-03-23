using UnityEngine;

public class Bullet : WrapPlayableArea
{
    public delegate int OnAstroidHit();
    public static OnAstroidHit onAstroidHit;

    private BulletsPool bulletPool;
    private int damage;
    private float force = 3;

    public void Init(BulletsPool pool, int damageValue, Transform playerTransform, float speed)
    {
        destroyOnOutOfScreen = true;
        bulletPool = pool;
        damage = damageValue;
        force = speed;
        AddBulletForce(playerTransform);
    }

    public override void OnOutOfScreen()
    {
        if (bulletPool)
        {
            bulletPool.ReleaseObject(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HitAstroid()
    {
        OnOutOfScreen();
    }

    void AddBulletForce(Transform player)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.up * force, ForceMode2D.Impulse);
    }
}
