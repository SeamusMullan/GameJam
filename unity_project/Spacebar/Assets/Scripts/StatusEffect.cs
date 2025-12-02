using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public enum StatusEffectType
{
    SpeedBoost,
    SpeedDebuff,
    DoublePoints,
    ExtraTime,
    Confusion
}

[System.Serializable]
public class StatusEffect
{
    public StatusEffectType type;
    public float duration;
    public float magnitude;
    public float timeRemaining;

    public StatusEffect(StatusEffectType effectType, float effectDuration, float effectMagnitude)
    {
        type = effectType;
        duration = effectDuration;
        magnitude = effectMagnitude;
        timeRemaining = effectDuration;
    }

    public void UpdateTimer(float deltaTime)
    {
        timeRemaining -= deltaTime;
    }

    public bool IsExpired()
    {
        return timeRemaining <= 0;
    }
}

public class StatusEffectManager : MonoBehaviour
{
    [Header("Active Effects")]
    private List<StatusEffect> activeEffects = new List<StatusEffect>();

    [Header("Effect Visuals")]
    [SerializeField] private GameObject speedBoostVFX;
    [SerializeField] private GameObject confusionVFX;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        UpdateEffects();
    }

    private void UpdateEffects()
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            activeEffects[i].UpdateTimer(Time.deltaTime);

            if (activeEffects[i].IsExpired())
            {
                RemoveEffect(activeEffects[i]);
                activeEffects.RemoveAt(i);
            }
        }
    }

    public void AddEffect(StatusEffectType type, float duration, float magnitude)
    {
        StatusEffect newEffect = new StatusEffect(type, duration, magnitude);
        activeEffects.Add(newEffect);
        ApplyEffect(newEffect);

        AudioManager.Instance?.PlaySoundOneShot("StatusEffect");
    }

    private void ApplyEffect(StatusEffect effect)
    {
        switch (effect.type)
        {
            case StatusEffectType.SpeedBoost:
                if (speedBoostVFX != null) speedBoostVFX.SetActive(true);
                break;
            case StatusEffectType.Confusion:
                if (confusionVFX != null) confusionVFX.SetActive(true);
                break;
        }
    }

    private void RemoveEffect(StatusEffect effect)
    {
        switch (effect.type)
        {
            case StatusEffectType.SpeedBoost:
                if (speedBoostVFX != null) speedBoostVFX.SetActive(false);
                break;
            case StatusEffectType.Confusion:
                if (confusionVFX != null) confusionVFX.SetActive(false);
                break;
        }
    }

    public float GetSpeedModifier()
    {
        float modifier = 1f;

        foreach (StatusEffect effect in activeEffects)
        {
            if (effect.type == StatusEffectType.SpeedBoost)
            {
                modifier += effect.magnitude;
            }
            else if (effect.type == StatusEffectType.SpeedDebuff)
            {
                modifier -= effect.magnitude;
            }
        }

        return modifier;
    }

    public bool HasEffect(StatusEffectType type)
    {
        foreach (StatusEffect effect in activeEffects)
        {
            if (effect.type == type)
            {
                return true;
            }
        }
        return false;
    }

    public List<StatusEffect> GetActiveEffects() => activeEffects;
}
