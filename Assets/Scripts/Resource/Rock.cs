using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;
    int _rockLife = 3;
    [SerializeField] GameObject _stone;
    [SerializeField] GameObject _renderer;
    public int RockLife { get { return _rockLife; } set { _rockLife = value; } }

    void Update()
    {
        if (_rockLife == 3) _renderer.transform.localScale = new Vector3(3, 3, 3);
        if (_rockLife == 2) _renderer.transform.localScale = new Vector3(2, 2, 2);
        if (_rockLife == 1) _renderer.transform.localScale = new Vector3(1, 1, 1);
        if (_rockLife <= 0)
        {
            PointManager.Instance._hitItems.Remove(gameObject);
            Instantiate(_stone, gameObject.transform.position, Quaternion.identity);
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
