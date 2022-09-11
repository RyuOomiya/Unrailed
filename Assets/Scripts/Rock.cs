using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    int _rockLife = 3;
    public int RockLife { get { return _rockLife; } set { _rockLife = value; } }
   
    void Update()
    {
        if (_rockLife <= 0)
        {
            Destroy(gameObject);
        }
    }
}
