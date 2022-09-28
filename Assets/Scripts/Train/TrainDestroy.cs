using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainDestroy : MonoBehaviour
{
    [SerializeField] GameObject _train;
   
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Ground ground))
        {
            Debug.Log(other.gameObject.name);
            Destroy(_train);
        }
    }
 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
