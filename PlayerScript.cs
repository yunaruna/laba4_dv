using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Vector2 speed = new Vector2(15, 15);
    public float tiltAngle = 15f;
    public float tiltSpeed = 8f;

    private Vector2 movement;
    private Rigidbody2D rb;
    private WeaponScript weapon;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponent<WeaponScript>();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        movement = new Vector2(speed.x * inputX, speed.y * inputY);

        float targetAngle = inputY * tiltAngle;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);

        bool shoot = Input.GetButton("Fire1") || Input.GetKey(KeyCode.Space);
        if (shoot && weapon != null)
        {
            weapon.Attack(false);
        }

        ClampToCameraBounds();
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.linearVelocity = movement;
    }

    private void ClampToCameraBounds()
    {
        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(transform.position.y, bottomBorder, topBorder),
            transform.position.z
        );
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();
        if (enemy != null)
        {
            HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
            if (enemyHealth != null) enemyHealth.Damage(enemyHealth.hp);

            HealthScript playerHealth = GetComponent<HealthScript>();
            if (playerHealth != null) playerHealth.Damage(1);
        }
    }
}