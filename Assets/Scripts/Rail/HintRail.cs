using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintRail : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PointManager>() != null)
        {
            ChangeSetActive();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent <PointManager>() != null)
        {
            ChangeSetActive();
        }
    }
    public void ChangeSetActive()
    {
        //プレイヤーが触れているときは表示して、離れたら表示しない
        gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
    }
}
