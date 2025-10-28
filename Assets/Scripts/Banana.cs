using UnityEngine;

public class Banana : MonoBehaviour
{
    public GameObject explosionEffectPrefab;

    private bool hasHit = false;


    void OnCollisionEnter(Collision collision)
    {

        if (hasHit)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            hasHit = true;
            Invoke("Explode", 1.0f);
        }
        else
        {
            Debug.Log(collision.gameObject.tag);
        }
    }

    void Explode()
    {
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}