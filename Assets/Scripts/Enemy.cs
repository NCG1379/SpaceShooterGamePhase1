using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float _speedEnemy = 2f;
    private float yMaxLimitEnemy = 7.0f;
    private float yMinLimitEnemy = -7.0f;


    Player_Controller playerController;
    [SerializeField]
    private GameObject _EnemyShot;

    private float _firerate = 3.0f;
    private float _canfire = -1;

    [SerializeField]
    private AudioClip _EnemyExplotionClip;
    private AudioSource _EnemyExplotion;

    Animator _EnemyDestroy;

    // Start is called before the first frame update
    void Start()
    {

        _EnemyExplotion = GetComponent<AudioSource>();
        if (_EnemyExplotion == null)
        {
            Debug.LogError("AudioSource is NULL");
        }

        _EnemyExplotion.clip = _EnemyExplotionClip;

        _EnemyDestroy = GetComponent<Animator>();
        if (_EnemyDestroy == null)
        {
            Debug.LogError("Animator::SetTrigger() is NULL");
        }

        playerController = GameObject.Find("SpaceShipPlayer").GetComponent<Player_Controller>();
        transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 6, 0);
    }

    // Update is called once per frame
    void Update()
    {
        ControlMovement();
        ShootingLasers();
    }

    void ShootingLasers()
    {
        if(Time.time > _canfire)
        {
            _firerate = Random.Range(3f, 7f);
            _canfire = Time.time + _firerate;

            //Fixed the problem when the Enemy is destroyed and 
            //Enemy shot laser in middle of Explotion.
            if (GetComponent<Collider2D>() != null) 
            {
                GameObject EnemyLaserClone = Instantiate(_EnemyShot, transform.position, Quaternion.identity);
                Laser[] lasers = EnemyLaserClone.GetComponentsInChildren<Laser>();
                for (int i = 0; lasers.Length > i; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }

        }
    }

    void ControlMovement()
    {
        float MoveSpeed = _speedEnemy * Time.deltaTime;

        transform.Translate(Vector3.down * MoveSpeed);

        if (transform.position.y <= yMinLimitEnemy)
        {
            transform.position = new Vector3(Random.Range(-9.0f, 9.0f), yMaxLimitEnemy, 0.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            _EnemyExplotion.Play();

            Player_Controller playerCon = other.transform.GetComponent<Player_Controller>();

            if (playerCon != null)
            {
                playerCon.Damage();
            }
            _EnemyDestroy.SetTrigger("OnEnemyDead");
            _speedEnemy = 0.0f;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);

        }

        //If other is laser
        //destroy laser
        //destroy us
        if (other.tag == "Laser")
        {
            _EnemyExplotion.Play();

            Destroy(other.gameObject);
            if (playerController != null)
            {
                playerController.AddPointsToScore();
            }
            _EnemyDestroy.SetTrigger("OnEnemyDead");
            _speedEnemy = 0.0f;
            Destroy(GetComponent<Collider2D>()); //Add a time to destroy de Collider to destroy all missiles that impact
            Destroy(this.gameObject, 2.8f);
            
        }

    }

}
