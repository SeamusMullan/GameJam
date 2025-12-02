using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public enum ItemType
{
    CrispSandwich,
    Flat7up,
    Pint,
    Cocktail,
    Monster
}

[System.Serializable]
public class OrderItem
{
    public ItemType itemType;
    public string itemName;

    public OrderItem(ItemType type)
    {
        itemType = type;
        itemName = GetItemName(type);
    }

    private string GetItemName(ItemType type)
    {
        switch (type)
        {
            case ItemType.CrispSandwich: return "Crisp Sandwich";
            case ItemType.Flat7up: return "Flat 7up";
            case ItemType.Pint: return "Pint";
            case ItemType.Cocktail: return "Cocktail";
            case ItemType.Monster: return "Gigachad Monster";
            default: return "Unknown Item";
        }
    }
}

public class Order
{
    public List<OrderItem> items;
    public float timeRemaining;
    public float maxTime;
    public int tipAmount;
    public bool isComplete;

    public Order(List<OrderItem> orderItems, float time)
    {
        items = orderItems;
        maxTime = time;
        timeRemaining = time;
        tipAmount = CalculateTipAmount();
        isComplete = false;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (isComplete) return;

        timeRemaining -= deltaTime;
        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
        }
    }

    public float GetTimePercentage()
    {
        return timeRemaining / maxTime;
    }

    public int GetCurrentTipAmount()
    {
        float percentage = GetTimePercentage();
        return Mathf.RoundToInt(tipAmount * percentage);
    }

    private int CalculateTipAmount()
    {
        int baseTip = 10;
        return baseTip * items.Count;
    }

    public void Complete()
    {
        isComplete = true;
    }
}
