using System.Collections;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int hp = 2;
    public bool isEnemy = true;
    public GameObject explosionPrefab;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void Damage(int damageCount)
    {
        hp -= damageCount;

        if (spriteRenderer != null && hp > 0)
            StartCoroutine(HitFlash());

        if (hp <= 0)
        {
            if (explosionPrefab != null)
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

    private IEnumerator HitFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        ShotScript shot = otherCollider.gameObject.GetComponent<ShotScript>();
        if (shot != null && shot.isEnemyShot != isEnemy)
        {
            Damage(shot.damage);
            Destroy(shot.gameObject);
        }
    }
}