using UnityEngine;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    [Header("Customer Settings")]
    [SerializeField] private float baseOrderTime = 30f;
    [SerializeField] private int minItems = 1;
    [SerializeField] private int maxItems = 3;

    [Header("Visual Feedback")]
    [SerializeField] private GameObject happyIndicator;
    [SerializeField] private GameObject angryIndicator;
    [SerializeField] private GameObject orderUI;

    [Header("State")]
    private Order currentOrder;
    private CustomerState state = CustomerState.Waiting;
    private int seatingPosition = 0;

    public enum CustomerState
    {
        Waiting,
        Happy,
        Angry,
        Leaving
    }

    void Start()
    {
        GenerateOrder();
    }

    void Update()
    {
        if (currentOrder != null && !currentOrder.isComplete)
        {
            currentOrder.UpdateTimer(Time.deltaTime);
            UpdateMoodIndicators();

            if (currentOrder.timeRemaining <= 0)
            {
                BecomeAngry();
            }
        }
    }

    private void GenerateOrder()
    {
        List<OrderItem> items = new List<OrderItem>();
        int itemCount = Random.Range(minItems, maxItems + 1);

        for (int i = 0; i < itemCount; i++)
        {
            ItemType randomType = (ItemType)Random.Range(0, System.Enum.GetValues(typeof(ItemType)).Length);
            items.Add(new OrderItem(randomType));
        }

        currentOrder = new Order(items, baseOrderTime);
    }

    private void UpdateMoodIndicators()
    {
        if (currentOrder == null) return;

        float timePercentage = currentOrder.GetTimePercentage();

        if (timePercentage > 0.5f)
        {
            state = CustomerState.Happy;
            if (happyIndicator != null) happyIndicator.SetActive(true);
            if (angryIndicator != null) angryIndicator.SetActive(false);
        }
        else if (timePercentage > 0.2f)
        {
            state = CustomerState.Waiting;
            if (happyIndicator != null) happyIndicator.SetActive(false);
            if (angryIndicator != null) angryIndicator.SetActive(false);
        }
        else
        {
            state = CustomerState.Angry;
            if (happyIndicator != null) happyIndicator.SetActive(false);
            if (angryIndicator != null) angryIndicator.SetActive(true);
        }
    }

    public void ServeItem(ItemType item)
    {
        if (currentOrder == null || currentOrder.isComplete) return;

        for (int i = 0; i < currentOrder.items.Count; i++)
        {
            if (currentOrder.items[i].itemType == item)
            {
                currentOrder.items.RemoveAt(i);
                AudioManager.Instance?.PlaySoundOneShot("ServeItem");

                if (currentOrder.items.Count == 0)
                {
                    CompleteOrder();
                }
                return;
            }
        }

        AudioManager.Instance?.PlaySoundOneShot("WrongItem");
    }

    private void CompleteOrder()
    {
        currentOrder.Complete();
        int tips = currentOrder.GetCurrentTipAmount();
        int score = 100;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddTips(tips);
            GameManager.Instance.AddScore(score);
        }

        AudioManager.Instance?.PlaySoundOneShot("OrderComplete");
        Leave();
    }

    private void BecomeAngry()
    {
        state = CustomerState.Angry;
        AudioManager.Instance?.PlaySoundOneShot("CustomerAngry");
        Leave();
    }

    private void Leave()
    {
        state = CustomerState.Leaving;
        CustomerSpawner.Instance?.RemoveCustomer(this);
        Destroy(gameObject, 1f);
    }

    public Order GetOrder() => currentOrder;
    public CustomerState GetState() => state;
    public void SetSeatingPosition(int position) => seatingPosition = position;
    public int GetSeatingPosition() => seatingPosition;
}
