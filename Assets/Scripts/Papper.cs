using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Papper : MonoBehaviour
{

    public GameObject impactEffectPrefab;
    public float damage = 20f;

    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

            if (impactEffectPrefab != null)
            {
                Instantiate(impactEffectPrefab, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
