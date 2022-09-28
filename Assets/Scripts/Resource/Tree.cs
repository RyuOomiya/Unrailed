using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour , IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;
    public ItemType Type { get => _type; }
    int _treeLife = 3;
    public int TreeLife { get { return _treeLife; } set { _treeLife = value; } }
    [SerializeField]GameObject _wood;
   
    void Update()
    {
        if(_treeLife <= 0)
        {
            
            PointManager.Instance._hitItems.Remove(gameObject);
            Instantiate(_wood, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void Action(GameObject hitObj)
    {

    }
}
