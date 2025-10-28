using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Key Spawning")]
    public GameObject keyPrefab;
    public GameObject women;
    public GameObject fireworks;

    public GameObject startUI;
    public GameObject cam2;
    bool isStart = false;

    public Transform[] spawnPoints = new Transform[5];

    [Header("UI Minimap")]
    public Transform playerTransform;
    public Image playerIcon;

    void Start()
    {
        if (keyPrefab == null)
        {
            Debug.LogError("GameManager: 'keyPrefab'이 등록되지 않았습니다!");
            return;
        }
        if (women == null)
        {
            Debug.LogError("GameManager: 'women' 프리팹이 등록되지 않았습니다!");
            return;
        }
        if (fireworks == null)
        {
            Debug.LogError("GameManager: 'fireworks' 프리팹이 등록되지 않았습니다!");
            return;
        }

        if (spawnPoints.Length < 3)
        {
            Debug.LogError("GameManager: 'spawnPoints'가 3개 미만입니다! 오브젝트를 생성할 수 없습니다.");
            return;
        }

        int index1 = Random.Range(0, spawnPoints.Length);

        int index2 = Random.Range(0, spawnPoints.Length);
        while (index1 == index2)
        {
            index2 = Random.Range(0, spawnPoints.Length);
        }

        int index3 = Random.Range(0, spawnPoints.Length);
        while (index3 == index1 || index3 == index2)
        {
            index3 = Random.Range(0, spawnPoints.Length);
        }

        Instantiate(keyPrefab, spawnPoints[index3].position, Quaternion.identity);
        Instantiate(keyPrefab, spawnPoints[index2].position, Quaternion.identity);

        Vector3 spawnPosition3 = spawnPoints[index1].position;
        Instantiate(women, spawnPosition3, Quaternion.identity);
        Instantiate(fireworks, spawnPosition3, Quaternion.identity);

        Debug.Log($"Key가 {spawnPoints[index1].name}와(과) {spawnPoints[index2].name}에, Women/Fireworks가 {spawnPoints[index3].name} 위치에 생성되었습니다.");
    }

    void Update()
    {
        if (playerTransform == null || playerIcon == null)
        {
            return;
        }

        if (isStart == false && Input.GetKeyDown(KeyCode.Space))
        {
            // 3. isStart를 true로 바꿔서 이 코드가 다시 실행되지 않게 함
            isStart = true;

            // 4. 두 개의 오브젝트를 비활성화(deactivate)
            startUI.SetActive(false);
            cam2.SetActive(false);
        }

        float playerX_Min = -31.1f;
        float playerX_Max = 9.7f;
        float playerZ_Min = -14.7f;
        float playerZ_Max = 32.5f;

        float iconX_Min = -118f;
        float iconX_Max = 130f;
        float iconY_Min = -128f;
        float iconY_Max = 130f;

        float newIconX = MapRange(playerTransform.position.x, playerX_Min, playerX_Max, iconX_Min, iconX_Max);
        float newIconY = MapRange(playerTransform.position.z, playerZ_Min, playerZ_Max, iconY_Min, iconY_Max);

        playerIcon.rectTransform.anchoredPosition = new Vector2(newIconX, newIconY);
    }

    private float MapRange(float value, float inMin, float inMax, float outMin, float outMax)
    {
        float clampedValue = Mathf.Clamp(value, inMin, inMax);
        float percentage = (clampedValue - inMin) / (inMax - inMin);
        float outputValue = outMin + (percentage * (outMax - outMin));
        return outputValue;
    }
}