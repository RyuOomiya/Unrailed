using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;

    public ItemType Type { get => _type; }

    void Start()
    {
        //最初のレールをリストに入れる
        RailManager.Instance.AddRail(this);
    }

    void Update()
    {

    }
    public void Action(GameObject hitObj)
    {
        //HintRailがあってスペースを押したらレールを設置する
        if (hitObj.TryGetComponent(out HintRail hintrail))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                gameObject.transform.position
                     = new Vector3(hitObj.transform.position.x, 0, hitObj.transform.position.z);
                //PointManagerクラスを参照してHaveObjResetメソッドを呼び出す
                PointManager pointManager= transform.parent.gameObject.GetComponent<PointManager>();
                pointManager.HaveObjReset();
            }
            
        }
    }
}