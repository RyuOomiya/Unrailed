using UnityEngine;
public enum ItemType 
{
    Tool,
    Resource,
}


interface IPickableItem
{
    ItemType Type { get; }
    void Action(GameObject hitObj);
}