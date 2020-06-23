using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    [SerializeField]
    private float _speedrotation;
    [SerializeField]
    private float _speedAsteroid;
    [SerializeField]
    private GameObject _ExplotionEffect;

    SpawnManager _spawnManager;
    //Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //rotate objects on the zed axis
        transform.Rotate(Vector3.forward*_speedrotation*Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_ExplotionEffect, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawnManager();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
