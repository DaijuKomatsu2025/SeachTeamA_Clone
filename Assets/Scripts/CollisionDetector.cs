using UnityEngine;
using System;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class CollisionDetector : MonoBehaviour
{
    //トリガーに入ったときのイベント
    [SerializeField] private TriggerEvent onTriggerEnter = new TriggerEvent();
    //トリガーに入ったときの処理
    [SerializeField] private TriggerEvent onTriggerStay = new TriggerEvent();
    private void OnTriggerStay(Collider other)//トリガーに入ったときの処理
    {
        onTriggerStay.Invoke(other);
    }
    private void OnTriggerEnter(Collider other)//トリガーに入ったときの処理
    {
        onTriggerEnter.Invoke(other);
    }
    //カスタムイベントクラス
    [Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {
    }
}
    

