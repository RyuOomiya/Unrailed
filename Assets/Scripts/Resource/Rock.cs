using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour, IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
    int _rockLife = 3;
    [SerializeField] GameObject _stone;
    public int RockLife { get { return _rockLife; } set { _rockLife = value; } }
    
    void Update()
    {
        if (_rockLife <= 0)
        {
            PointManager.Instance._hitItems.Remove(gameObject);
            Instantiate(_stone , gameObject.transform.position , Quaternion.identity);
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
