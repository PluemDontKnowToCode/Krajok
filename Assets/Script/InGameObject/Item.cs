using Unity.VisualScripting;
using UnityEngine;

public class Item
{
    private GameObject itemObject;
    Rigidbody2D rb;
    public Item(GameObject item)
    {
        itemObject = item;
        rb = item.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.mass = (item.tag == "HeavyItem") ? 60f : (item.tag == "LightItem") ? 45f : rb.mass;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    public void GotPickUp(Transform playerTransform)
    {
        // Set the player as the parent
        itemObject.transform.parent = playerTransform;

        // Set the item's position higher
        itemObject.transform.position = new Vector2(itemObject.transform.position.x +0.1625f*playerTransform.localScale.x, itemObject.transform.position.y + 0.8f);

        // Disable physics for the item
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
    }

    public void GotDrop()
    {
        // Remove the parent (player)
        itemObject.transform.parent = null;
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        // Enable physics for the item
        rb.isKinematic = false;
    }
}
