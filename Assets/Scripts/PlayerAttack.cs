using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Common Spawn Settings")]
    [Tooltip("발사될 y축 높이 (플레이어 몸 중앙)")]
    public float spawnHeightOffset = 1.0f;

    [Header("Skill 1: Tomato")]
    public GameObject tomatoPrefab;
    public float tomatoForce = 1f;
    [Tooltip("플레이어 정면으로 생성될 거리")]
    public float tomatoForwardOffset = 0.5f;
    [Tooltip("토마토 사이의 간격")]
    public float tomatoSpacing = 0.5f;

    [Header("Skill 2: Banana")]
    public GameObject bananaPrefab;
    public float bananaForce = 1f;
    [Tooltip("플레이어 정면으로 생성될 거리")]
    public float bananaForwardOffset = 1.0f;

    Player playerComponet;

    private void Start()
    {
        playerComponet = GetComponent<Player>();
    }


    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (playerComponet.hasWomen)
            {
                GameObject.FindGameObjectsWithTag("Women")[0].GetComponent<Women>().ThrowPapper();
            }
            AttackSkill1();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (playerComponet.hasWomen)
            {
                GameObject.FindGameObjectsWithTag("Women")[0].GetComponent<Women>().ThrowPapper();
            }
            AttackSkill2();
        }
    }

    private void AttackSkill1()
    {
        Vector3 playerForward = transform.forward;
        Vector3 playerRight = transform.right;


        Vector3 spawnOrigin = transform.position + (Vector3.up * spawnHeightOffset);
        Vector3 basePos = spawnOrigin + (playerForward * tomatoForwardOffset);

        Quaternion spawnRotation = transform.rotation;

        Vector3 pos1 = basePos - (playerRight * tomatoSpacing);
        Vector3 pos2 = basePos;
        Vector3 pos3 = basePos + (playerRight * tomatoSpacing);

        GameObject tomato1 = Instantiate(tomatoPrefab, pos1, spawnRotation);
        GameObject tomato2 = Instantiate(tomatoPrefab, pos2, spawnRotation);
        GameObject tomato3 = Instantiate(tomatoPrefab, pos3, spawnRotation);

        Vector3 highArcDirection = (playerForward + transform.up * 0.7f).normalized;

        Rigidbody rb1 = tomato1.GetComponent<Rigidbody>();
        Rigidbody rb2 = tomato2.GetComponent<Rigidbody>();
        Rigidbody rb3 = tomato3.GetComponent<Rigidbody>();

        if (rb1) rb1.AddForce(highArcDirection * tomatoForce, ForceMode.Impulse);
        if (rb2) rb2.AddForce(highArcDirection * tomatoForce, ForceMode.Impulse);
        if (rb3) rb3.AddForce(highArcDirection * tomatoForce, ForceMode.Impulse);
    }

    private void AttackSkill2()
    {
        // y축 높이가 적용된 발사 기준 위치
        Vector3 spawnOrigin = transform.position + (Vector3.up * spawnHeightOffset);
        Vector3 spawnPos = spawnOrigin + (transform.forward * bananaForwardOffset);

        GameObject banana = Instantiate(bananaPrefab, spawnPos, transform.rotation);
        Rigidbody rb = banana.GetComponent<Rigidbody>();

        if (rb)
        {
            rb.AddForce(transform.forward * bananaForce, ForceMode.Impulse);
        }
    }
}