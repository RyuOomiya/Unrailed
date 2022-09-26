using UnityEngine;
public enum ItemType 
{
    Tool,
    Resource,
    Rail,
    NotItem,
}


public interface IPickableItem
{
    ItemType Type { get;  }
    void Action(GameObject hitObj);
}