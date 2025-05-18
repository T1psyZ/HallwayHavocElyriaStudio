using System.Collections;
using UnityEngine;

public class Enemy_Attack : MonoBehaviour
{
    public GameObject bullet;
    GameObject player;
    float bulletSpeed = 10f;
    float angleOffset = 0f; // Offset for rotating the bullet pattern

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        var dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist <= 30)
        {
            int bulletCount = 16;
            float radius = 1.5f;

            for (int i = 0; i < bulletCount; i++)
            {
                // Add angleOffset to rotate the pattern each attack
                float angle = i * Mathf.PI * 2 / bulletCount + angleOffset;
                Vector2 spawnPos = new Vector2(
                    transform.position.x + Mathf.Cos(angle) * radius,
                    transform.position.y + Mathf.Sin(angle) * radius
                );

                GameObject b = Instantiate(bullet, spawnPos, Quaternion.identity);

                Vector2 direction = (spawnPos - (Vector2)transform.position).normalized;
                b.transform.right = direction;

                Rigidbody2D rb = b.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = direction * bulletSpeed;
                }
            }

            // Increment the offset for the next attack (e.g., 15 degrees in radians)
            angleOffset += Mathf.Deg2Rad * 15f;
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(Attack());
    }
}
