using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Transform shotPrefab;
    public float shootingRate = 0.25f;

    private float shootCooldown;

    void Start()
    {
        shootCooldown = 0f;
    }

    void Update()
    {
        if (shootCooldown > 0)
            shootCooldown -= Time.deltaTime;
    }

    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            shootCooldown = shootingRate;

            Transform shotTransform = Instantiate(shotPrefab);
            shotTransform.position = transform.position;

            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            if (shot != null)
                shot.isEnemyShot = isEnemy;

            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
                move.direction = transform.right;
        }
    }

    public bool CanAttack => shootCooldown <= 0f;
}