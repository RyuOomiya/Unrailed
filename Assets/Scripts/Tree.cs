using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    int _treeLife = 3;
    public int TreeLife { get { return _treeLife; } set { _treeLife = value; } }
   
    void Update()
    {
        if(_treeLife <= 0)
        {
            Destroy(gameObject);
        }
    }
}
