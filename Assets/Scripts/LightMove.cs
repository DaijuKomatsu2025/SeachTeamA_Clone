using UnityEngine;

public class LightMove : MonoBehaviour
{
    [SerializeField] private Transform _cam;
    [SerializeField] private float offsetX = 30f;
    [SerializeField] private float offsetY = 30f;

    // Update is called once per frame
    void Update()
    {
        transform.position = _cam.position;
        var rotation = _cam.rotation;
        rotation.x += offsetX;
        rotation.y += offsetY;
        transform.rotation = rotation;
    }
}
