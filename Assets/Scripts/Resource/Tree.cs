using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour , IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;
    int _treeLife = 3;
    public int TreeLife { get { return _treeLife; } set { _treeLife = value; } }
    [SerializeField] GameObject _wood;
    [SerializeField] GameObject _renderer;
   
    void Update()
    {
        if (_treeLife == 3) _renderer.transform.localScale = new Vector3(3, 3, 3);
        if (_treeLife == 2) _renderer.transform.localScale = new Vector3(2, 2, 2);
        if (_treeLife == 1) _renderer.transform.localScale = new Vector3(1, 1, 1);
        if (_treeLife <= 0)
        {
            
            PointManager.Instance._hitItems.Remove(gameObject);
            Instantiate(_wood, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void Action(GameObject hitObj)
    {

    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}
