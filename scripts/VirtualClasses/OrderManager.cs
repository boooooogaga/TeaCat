using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Order
{
    public int ID;
    public GameObject Client;
    public int redTeaRequired;
    public int greenTeaRequired;
    public int blackTeaRequired;

    // Конструктор для удобного создания заказа одной строчкой
    public Order(int id,GameObject obj, int red = 0, int green = 0, int black = 0)
    {
        ID = id;
        Client = obj;
        redTeaRequired = red;
        greenTeaRequired = green;
        blackTeaRequired = black;
    }
}
public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    // Список всех текущих активных заказов
    [Header("Active Orders")]
    public List<Order> activeOrders = new List<Order>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Добавить новый заказ в общую очередь
    public void AddOrder(int newID,GameObject Object, int red, int green, int black = 0)
    {
        foreach(var order in activeOrders)
        {
            if(order.ID == newID) return;
        }
        Order newOrder = new Order(newID, Object, red, green, black);
        activeOrders.Add(newOrder);
    }

    // Получить заказ конкретного клиента по имени
    public Order? GetOrder(int currentID) //знак вопроса чтобы можно было вернуть null
    {
        foreach (var order in activeOrders)
        {
            if (order.ID == currentID)
                return order;
        }
        return null; // Если заказ не найден
    }

    // Удалить заказ (например, когда мини-игра для этого клиента успешно завершена)
    public void CompleteOrder(int currentID)
    {
        Order? orderToComplete = GetOrder(currentID);

        if (orderToComplete != null && orderToComplete.Value.Client != null)
        {
            Destroy(orderToComplete.Value.Client);
            Debug.Log($"Client GameObject with ID {currentID} destroyed from scene.");
        }

        activeOrders.RemoveAll(order => order.ID == currentID);
    }

    // Полная очистка всех заказов
    public void ClearAllOrders()
    {
        activeOrders.Clear();
    }
}