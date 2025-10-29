using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public float damage = 10f;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifeTime); // éûä‘Ç≈é©ìÆè¡ñ≈
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            Debug.Log("Player hit! Damage: " + damage);
            var status = other.gameObject.GetComponent<CommonStatus>();
            status.Damage((int)damage);
            Destroy(gameObject);
        }
    }
}
