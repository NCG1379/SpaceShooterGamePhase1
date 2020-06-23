using UnityEngine;

public class ControlPowerUp : MonoBehaviour
{
    private float _speedPowerUp = 3.0f;
    private float _speedFactor;

    [SerializeField]
    private AudioClip _PickedUpPowerUpClip;

    [SerializeField]
    private int _PowerUpID; // 0 to define TripleShot, 1 Speed PowerUp, 2 Shield

    // Start is called before the first frame update
    void Start()
    {

        transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 6, 0);
    }

    // Update is called once per frame
    void Update()
    {
        _speedFactor = _speedPowerUp * Time.deltaTime;
        transform.Translate(Vector3.down*_speedFactor);

        if(transform.position.y < -6.0f)
        {
            transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 6, 0);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            Player_Controller playerController = other.transform.GetComponent<Player_Controller>();

            AudioSource.PlayClipAtPoint(_PickedUpPowerUpClip, transform.position);

            if(playerController != null)
            {
                
                switch (_PowerUpID)
                {
                    case 0:
                        playerController.TripleShotActive();
                        break;
                    case 1:
                        playerController.SpeedPowerUpActive();
                        break;
                    case 2:
                        playerController.ShieldPowerUpActive();
                        break;
                    case 3:
                        playerController.RechargeAmmunition();
                        break;
                    case 4:
                        playerController.RestoreHealth();
                        break;
                    case 5:
                        playerController.MissileActive();
                        break;
                    default:
                        Debug.Log("default Value ");
                        break;
                }
            }
            
            Destroy(this.gameObject);
        }
    }

}
