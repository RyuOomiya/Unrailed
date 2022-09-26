using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemGrid : MonoBehaviour
{
    GameObject _point;
    public GameObject Point { get => _point; }
    List<GameObject> _points = new List<GameObject>();
   
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Ground>())
        {
            _points.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ground>())
        {
            _points.Remove(other.gameObject);
        }
    }
    void Update()
    {
        _point = _points.OrderBy(x =>
                Vector3.SqrMagnitude(gameObject.transform.position - x.transform.position)
                ).FirstOrDefault();
    }
}
