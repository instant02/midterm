using UnityEngine;
using UnityEngine.UI; // UI.Image 컴포넌트를 사용하기 위해 추가

public class GameManager : MonoBehaviour
{
    [Header("Key Spawning")] // 인스펙터에서 구역 나누기
    public GameObject keyPrefab;
    public Transform[] spawnPoints = new Transform[5];

    [Header("UI Minimap")] // 인스펙터에서 구역 나누기
    public Transform playerTransform; // 1. Player 오브젝트를 이 슬롯에 드래그
    public Image playerIcon;          // 2. UI Image 아이콘을 이 슬롯에 드래그

    // --- Key Spawning (원본 코드) ---
    void Start()
    {
        if (keyPrefab == null)
        {
            Debug.LogError("GameManager: 'keyPrefab'이 등록되지 않았습니다!");
            return;
        }

        if (spawnPoints.Length < 2)
        {
            Debug.LogError("GameManager: 'spawnPoints'가 2개 미만입니다! 키를 생성할 수 없습니다.");
            return;
        }

        int index1 = Random.Range(0, spawnPoints.Length);
        int index2 = Random.Range(0, spawnPoints.Length);

        while (index1 == index2)
        {
            index2 = Random.Range(0, spawnPoints.Length);
        }

        Instantiate(keyPrefab, spawnPoints[index1].position, Quaternion.identity);
        Instantiate(keyPrefab, spawnPoints[index2].position, Quaternion.identity);

        Debug.Log($"Key가 {spawnPoints[index1].name}와(과) {spawnPoints[index2].name} 위치에 생성되었습니다.");
    }

    // --- UI Icon Mapping (추가된 코드) ---
    void Update()
    {
        // 3. Player나 Icon이 설정되지 않았다면 에러 방지를 위해 실행 중지
        if (playerTransform == null || playerIcon == null)
        {
            return;
        }

        // 4. 플레이어와 아이콘의 좌표 범위를 정의

        // Player 월드 좌표
        float playerX_Min = -31.1f;
        float playerX_Max = 9.7f;
        float playerZ_Min = -14.7f;
        float playerZ_Max = 32.5f;

        // Icon UI 좌표
        float iconX_Min = -118f;
        float iconX_Max = 130f;
        float iconY_Min = -128f;
        float iconY_Max = 130f;

        // 5. MapRange 함수를 사용해 새 UI 좌표 계산
        // Player의 X -> Icon의 X
        float newIconX = MapRange(playerTransform.position.x, playerX_Min, playerX_Max, iconX_Min, iconX_Max);

        // Player의 Z -> Icon의 Y
        float newIconY = MapRange(playerTransform.position.z, playerZ_Min, playerZ_Max, iconY_Min, iconY_Max);

        // 6. 계산된 좌표를 Icon의 RectTransform.anchoredPosition에 적용
        playerIcon.rectTransform.anchoredPosition = new Vector2(newIconX, newIconY);
    }

    /**
     * 값을 한 범위에서 다른 범위로 변환(매핑)하는 헬퍼 함수
     * 예: MapRange(5, 0, 10, 0, 100) -> 50 반환
     */
    private float MapRange(float value, float inMin, float inMax, float outMin, float outMax)
    {
        // 플레이어가 지정된 범위를 벗어났을 때 아이콘이 맵 밖으로 나가지 않도록 고정(Clamp)
        float clampedValue = Mathf.Clamp(value, inMin, inMax);

        // 입력 범위에서의 비율(0~1) 계산
        float percentage = (clampedValue - inMin) / (inMax - inMin);

        // 해당 비율을 출력 범위에 적용
        float outputValue = outMin + (percentage * (outMax - outMin));

        return outputValue;
    }
}