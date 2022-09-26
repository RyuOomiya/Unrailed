using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour , IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
    public ItemType Type { get => _type; }
    int _treeLife = 3;
    public int TreeLife { get { return _treeLife; } set { _treeLife = value; } }
   
    void Update()
    {
        if(_treeLife <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Action(GameObject hitObj)
    {

    }
}
