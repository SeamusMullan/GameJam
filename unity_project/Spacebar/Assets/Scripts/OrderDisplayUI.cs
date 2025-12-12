using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderDisplayUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI orderItemsText;
    [SerializeField] private Image timerBar;
    [SerializeField] private Image moodIndicator;

    [Header("Colors")]
    [SerializeField] private Color happyColor = Color.green;
    [SerializeField] private Color neutralColor = Color.yellow;
    [SerializeField] private Color angryColor = Color.red;

    private Customer targetCustomer;

    void Update()
    {
        if (targetCustomer != null)
        {
            UpdateDisplay();
        }
    }

    public void SetCustomer(Customer customer)
    {
        targetCustomer = customer;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (targetCustomer == null) return;

        Order order = targetCustomer.GetOrder();
        if (order == null) return;

        if (orderItemsText != null)
        {
            orderItemsText.text = GetOrderItemsString(order);
        }

        if (timerBar != null)
        {
            timerBar.fillAmount = order.GetTimePercentage();
        }

        if (moodIndicator != null)
        {
            float timePercentage = order.GetTimePercentage();
            if (timePercentage > 0.5f)
            {
                moodIndicator.color = happyColor;
            }
            else if (timePercentage > 0.2f)
            {
                moodIndicator.color = neutralColor;
            }
            else
            {
                moodIndicator.color = angryColor;
            }
        }
    }

    private string GetOrderItemsString(Order order)
    {
        string result = "Order:\n";
        foreach (OrderItem item in order.items)
        {
            result += $"- {item.itemName}\n";
        }
        return result;
    }
}
