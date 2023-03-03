using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    BoxCollider _collider;
    [SerializeField] ItemGrid _itemGrid;
    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Rail rail))
        {
            _collider.size = new Vector3(1,1,1);
        }

        
    }

    private void OnTriggerStay(Collider other)
    {
        _itemGrid._grounds.Remove(this);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Rail rail))
        {
            _collider.size = new Vector3(1, 1.3f, 1);
        }

        _itemGrid._grounds.Add(this);
    }
}
