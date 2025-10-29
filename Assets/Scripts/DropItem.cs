using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
public class DropItem : MonoBehaviour
{
    public enum itemType
    {
        Potion,
        Timer
    }
    [SerializeField] private itemType type;//アイテムの種類

    public void Initialized()
    {
        var colliderCache = GetComponent<Collider>();//Colliderコンポーネントを取得
        colliderCache.enabled = false;//当たり判定を無効にする
        var transformCache = transform;//Transformコンポーネントを取得
        //落下位置をランダムに決定
        var dropPosition = transform.localPosition + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
        transformCache.DOLocalMove(dropPosition, 0.5f);//0.5秒かけて落下位置まで移動
        var defaultScale = transformCache.localScale;//元の大きさを保存
        transformCache.localScale = Vector3.zero;//大きさを0にする
        transformCache.DOScale(defaultScale, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            colliderCache.enabled = true;//当たり判定を有効にする
        });//0.5秒かけて元の大きさまで拡大、バウンドしながら拡大する
    }
   
}