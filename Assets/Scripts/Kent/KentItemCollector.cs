using UnityEngine;
using UnityEngine.AI;

public class KentItemCollector : MonoBehaviour
{
    public InventoryController inventoryController;
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if(item != null)
            {
                bool itemAdded = inventoryController.AddItem(collision.gameObject);
                if(itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
    
}
