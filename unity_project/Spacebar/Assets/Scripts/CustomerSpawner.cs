using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public static CustomerSpawner Instance { get; private set; }

    [Header("Spawning")]
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private Transform[] seatingPositions;
    [SerializeField] private float spawnDelay = 2f;

    [Header("State")]
    private List<Customer> activeCustomers = new List<Customer>();
    private int customersToSpawn = 0;
    private float spawnTimer = 0f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (customersToSpawn > 0)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnDelay)
            {
                SpawnCustomer();
                spawnTimer = 0f;
            }
        }
    }

    public void SpawnWave(int customerCount)
    {
        customersToSpawn = customerCount;
    }

    private void SpawnCustomer()
    {
        if (customerPrefab == null)
        {
            Debug.LogWarning("Customer prefab not assigned!");
            return;
        }

        int availableSeat = GetAvailableSeatingPosition();
        if (availableSeat == -1)
        {
            Debug.LogWarning("No available seating positions!");
            return;
        }

        GameObject customerObj = Instantiate(customerPrefab, seatingPositions[availableSeat].position, seatingPositions[availableSeat].rotation);
        Customer customer = customerObj.GetComponent<Customer>();

        if (customer != null)
        {
            customer.SetSeatingPosition(availableSeat);
            activeCustomers.Add(customer);
        }

        customersToSpawn--;

        AudioManager.Instance?.PlaySoundOneShot("CustomerArrive");
    }

    private int GetAvailableSeatingPosition()
    {
        if (seatingPositions == null || seatingPositions.Length == 0)
            return -1;

        for (int i = 0; i < seatingPositions.Length; i++)
        {
            bool isOccupied = false;
            foreach (Customer customer in activeCustomers)
            {
                if (customer != null && customer.GetSeatingPosition() == i)
                {
                    isOccupied = true;
                    break;
                }
            }

            if (!isOccupied)
                return i;
        }

        return -1;
    }

    public void RemoveCustomer(Customer customer)
    {
        if (activeCustomers.Contains(customer))
        {
            activeCustomers.Remove(customer);
        }

        if (customersToSpawn == 0 && activeCustomers.Count == 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.EndWave();
            }
        }
    }

    public List<Customer> GetActiveCustomers() => activeCustomers;
}
