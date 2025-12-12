using UnityEngine;

public class DrinkStation : Station
{
    [Header("Drink Settings")]
    [SerializeField] private StatusEffectType effectType = StatusEffectType.SpeedBoost;
    [SerializeField] private float effectDuration = 10f;
    [SerializeField] private float effectMagnitude = 0.5f;

    protected override void OnInteractionComplete()
    {
        DrinkBeverage();
    }

    private void DrinkBeverage()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            StatusEffectManager effectManager = player.GetComponent<StatusEffectManager>();
            if (effectManager != null)
            {
                effectManager.AddEffect(effectType, effectDuration, effectMagnitude);
                AudioManager.Instance?.PlaySoundOneShot("Drink");
            }
        }
    }
}
