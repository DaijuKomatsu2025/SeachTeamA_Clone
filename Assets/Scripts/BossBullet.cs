using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 3f;
    public float damage = 10f;

    private Transform target;

    void Start()
    {
        Destroy(gameObject, lifeTime); // 時間で自動消滅
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag.Contains("Player")))
        {
            var status = other.gameObject.GetComponent<CommonStatus>();
            status.Damage((int)damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.name != "SpawnerNormal2" || other.gameObject.name != "SpawnerNormal2 (1)" ||
            other.gameObject.name != "SpawnerNormal2 (2)" || other.gameObject.name != "SpawnerNormal2 (3)")
        {
            Destroy(gameObject);
        }

        Debug.Log("BulletHit: " + other.gameObject.name);
    }
}
