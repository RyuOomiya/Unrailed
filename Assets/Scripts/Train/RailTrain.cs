using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RailTrain : MonoBehaviour
{
    [SerializeField] GameObject _rail;
    
    void Update()
    {
        InstantiateRail();
    }

    void InstantiateRail()
    {
        if (FreightTrain.Instance._instanceRail)
        {
            Instantiate
                (_rail, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z),
                Quaternion.identity);
            FreightTrain.Instance._instanceRail = false;
        }
    }
}
