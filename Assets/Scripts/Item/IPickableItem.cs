using UnityEngine;
public enum ItemType 
{
    Tool,
    Rail,
    NotItem,
}


public interface IPickableItem
{
    void Action(GameObject hitObj);
    ItemType GetType();
}