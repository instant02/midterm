using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    [Tooltip("Enemy를 따라다닐 Slider (hp)를 여기에 끌어다 놓으세요.")]
    public Slider healthSlider;

    // UI 캔버스와 메인 카메라 참조
    private Canvas healthCanvas;
    private Camera mainCamera;

    void Start()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;

            // 슬라이더의 부모 캔버스를 찾아서 저장
            healthCanvas = healthSlider.GetComponentInParent<Canvas>();

            if (healthCanvas == null)
            {
                Debug.LogError("Health Slider에 부모 Canvas가 없습니다!", this);
            }
            else if (healthCanvas.renderMode != RenderMode.WorldSpace)
            {
                Debug.LogWarning("Enemy 체력바 Canvas의 Render Mode를 World Space로 설정해야 합니다.");
            }
        }
        else
        {
            Debug.LogError("Health Slider가 Inspector에 할당되지 않았습니다!", this);
        }
    }

    void LateUpdate()
    {
        // 캔버스가 존재하고, 카메라가 할당되었을 때
        if (healthCanvas != null && mainCamera != null)
        {
            // 캔버스의 회전값을 카메라의 회전값과 강제로 동일하게 맞춤
            // 이렇게 하면 Enemy(부모)가 회전해도 캔버스는 카메라만 바라봄
            healthCanvas.transform.rotation = mainCamera.transform.rotation;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        // 캔버스가 할당되어 있다면 캔버스 오브젝트를 파괴
        if (healthCanvas != null)
        {
            Destroy(healthCanvas.gameObject);
        }

        Destroy(gameObject);
    }
}