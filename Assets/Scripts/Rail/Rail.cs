using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;
    [SerializeField] ItemGrid _grid;
    bool _isMove = false;

    public void Set(ItemType type)
    {
        _type = type;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(_isMove && other.TryGetComponent(out IPickableItem ip) && ip.GetType() == ItemType.Tool)
        {
                Debug.Log("a");
                other.transform.position = new Vector3(_grid.Ground.transform.position.x
                                                        , -0.45f
                                                        , _grid.Ground.transform.position.z);
                _isMove = false;
        }
    }
    public void Action(GameObject hitObj)
    {
        //HintRailがあってスペースを押したらレールを設置する
        if (hitObj.TryGetComponent(out HintRail hintrail) && hintrail.CanSet && hintrail.GetComponent<MeshRenderer>().enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isMove = true;
                Debug.Log("設置ー");
                RailManager.Instance._rails.Add(this);
                gameObject.transform.position
                     = new Vector3(hitObj.transform.position.x, -0.45f, hitObj.transform.position.z);
                //PointManagerクラスを参照してHaveObjResetメソッドを呼び出す
                PointManager pointManager= transform.parent.gameObject.GetComponent<PointManager>();
                pointManager.HaveObjReset();
                hintrail.ChangeSetActive(false);
            } 
        }
    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}