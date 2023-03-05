using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour, IPickableItem
{
    [Tooltip("アイテムタイプ"), SerializeField] ItemType _type;
    [SerializeField] ItemGrid _grid;
    public MeshRenderer _railColor;
    [Tooltip("設置されていないレールのマテリアル")] public Material _railMaterial;
    [SerializeField,Tooltip("設置されたレールのマテリアル")] Material _installedRailMaterial;
    bool _isMove = false;

    void Awake()
    {
        if( _grid == null )
        {
            _grid = FindObjectOfType<ItemGrid>();
        }

        _railColor = gameObject.GetComponent<MeshRenderer>();
    }
    public void Set(ItemType type)
    {
        _type = type;
    }
    
    void OnTriggerEnter(Collider other)
    {
        //レールを設置時にそこにアイテムがあったら近場に移動
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
                _isMove = true;　//ontriggerenter内の処理を開始
                Debug.Log("レール設置");
                _railColor.material = _installedRailMaterial; //設置したレールのマテリアルを張り替える
                RailManager.Instance._rails.Add(this);  //リストに追加
                //マス目に設置
                gameObject.transform.position
                     = new Vector3(hitObj.transform.position.x, -0.45f, hitObj.transform.position.z);
                //PointManagerクラスを参照してHaveObjResetメソッドを呼び出す
                PointManager pointManager= transform.parent.gameObject.GetComponent<PointManager>();
                pointManager.HaveObjReset();
                hintrail.ChangeSetActive(false); //hintrailのハイライト表示をオフ
            } 
        }
    }

    ItemType IPickableItem.GetType()
    {
        return _type;
    }
}