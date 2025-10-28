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

        if (healthCanvas != null && mainCamera != null)
        {

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

        if (healthCanvas != null)
        {
            Destroy(healthCanvas.gameObject);
        }

        Destroy(gameObject);
    }
}