using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private bool hasSpawn;
    private MoveScript moveScript;
    private WeaponScript[] weapons;
    private Collider2D col;
    private Renderer rend;

    void Awake()
    {
        weapons = GetComponentsInChildren<WeaponScript>();
        moveScript = GetComponent<MoveScript>();
        col = GetComponent<Collider2D>();
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
        hasSpawn = false;
        col.enabled = false;
        moveScript.enabled = false;
        foreach (WeaponScript weapon in weapons)
            weapon.enabled = false;
    }

    void Update()
    {
        if (!hasSpawn)
        {
            if (rend.IsVisibleFrom(Camera.main))
                Spawn();
        }
        else
        {
            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                    weapon.Attack(true);
            }

            if (!rend.IsVisibleFrom(Camera.main))
                Destroy(gameObject);
        }
    }

    private void Spawn()
    {
        hasSpawn = true;
        col.enabled = true;
        moveScript.enabled = true;
        foreach (WeaponScript weapon in weapons)
            weapon.enabled = true;
    }
}