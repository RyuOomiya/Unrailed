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
        //�v���C���[���G��Ă���Ƃ��͕\�����āA���ꂽ��\�����Ȃ�
        gameObject.GetComponent<MeshRenderer>().enabled = !gameObject.GetComponent<MeshRenderer>().enabled;
    }
}
