using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(CommonStatus))]

public class ItemDropper : MonoBehaviour
{
    [SerializeField][Range(0, 1)] private float dropRate = 0.1f;//アイテムドロップ率
    [SerializeField] private DropItem itemPrefab;//ドロップするアイテムのプレハブ
    [SerializeField] private int number = 1;//ドロップするアイテムの数

    private CommonStatus status;
    private bool isDdropInVoked;//ドロップ処理が呼び出されたかどうか

    private void Start()
    {
        status = GetComponent<CommonStatus>();
    }
    private void Update()
    {
        if (status.hp <= 0)
        {
            DropIfNeeded();//必要に応じてドロップ処理を呼び出す
        }
    }
    private void DropIfNeeded()
    {
        if (isDdropInVoked) return;//すでにドロップ処理が呼び出されている場合は無視
        isDdropInVoked = true;//ドロップ処理が呼び出されたことを記録
        if (Random.Range(0, 1f) >= dropRate) return;//ドロップ率に基づいてドロップするかどうかを決定
        for (var i = 0; i < number; i++)
        {
            var item = Instantiate(itemPrefab, transform.position, Quaternion.identity);//アイテムを生成
            item.Initialized();//アイテムの初期化処理を呼び出す
        }
    }
}
