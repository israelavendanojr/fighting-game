using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private HealthComponent _targetHealth;

    // Call this from the GameManager after spawning
    public void Setup(HealthComponent health)
    {
        _targetHealth = health;
        UpdateUI();
    }

    // This method will be called by your GameEvents (OnDamaged, OnHealed)
    public void UpdateUI()
    {
        if (_targetHealth == null || healthSlider == null) return;

        float current = _targetHealth.GetCurrentHealth();
        float max = _targetHealth.GetMaxHealth();
        
        healthSlider.value = current / max;
    }
}