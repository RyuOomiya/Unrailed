using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour , IPickableItem
{
    [Tooltip("�A�C�e���^�C�v"), SerializeField] ItemType _type;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Action(GameObject hitObj)
    {
        if (hitObj.gameObject.name == "StoneStorage")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _type = ItemType.NotItem;
                gameObject.transform.position =
                    new Vector3(hitObj.transform.position.x, hitObj.transform.position.y - 0.3f, hitObj.transform.position.z);
                FreightTrain.Instance._stones.Add(gameObject);
                //PointManager�N���X���Q�Ƃ���HaveObjReset���\�b�h���Ăяo��
                PointManager pointManager = transform.parent.gameObject.GetComponent<PointManager>();
                pointManager.HaveObjReset();
            }
        }
    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}
