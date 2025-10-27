using UnityEngine;

public class Banana : MonoBehaviour
{
    public GameObject explosionEffectPrefab;

    // "폭발은 한번만" 요구사항을 처리하기 위한 플래그
    private bool hasHit = false;

    // (Start 함수는 비어있으므로 삭제해도 무방합니다)

    void OnCollisionEnter(Collision collision)
    {
        // 1. 이미 폭발이 예약되었다면(hasHit == true)
        //    이후의 모든 충돌을 무시합니다.
        if (hasHit)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy"))
        {
            // 2. "폭발은 한번만"을 위해 즉시 플래그를 true로 설정합니다.
            hasHit = true;

            // 3. 현재 코드를 실행하는 대신,
            //    2초 뒤에 "Explode"라는 이름의 함수를 실행하도록 예약합니다.
            Invoke("Explode", 1.0f);
        }
        else
        {
            Debug.Log(collision.gameObject.tag);
        }
    }

    // 4. 2초 뒤에 'Invoke'에 의해 호출될 함수
    void Explode()
    {
        // 5. 2초가 지났으므로, 여기서 폭발 이펙트를 생성합니다.
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        // 6. 폭발과 동시에 바나나(자기 자신)를 파괴합니다.
        //    (여기서는 지연 시간을 주면 안 됩니다)
        Destroy(gameObject);
    }
}