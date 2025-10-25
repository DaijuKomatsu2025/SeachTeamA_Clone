using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private float _offset = 2.0f;

    // Update is called once per frame
    void Update()
    {
        var pos = _target.position;
        pos.y += _offset;
        transform.position = pos;
    }
}
