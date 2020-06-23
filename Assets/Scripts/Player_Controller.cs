using System;
using System.Collections;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private GameObject _LaserPrefab;
    [SerializeField]
    private GameObject _TripleShotLaser;
    [SerializeField]
    private GameObject _MissileShot;
    [SerializeField]
    private GameObject _ShieldProtection;
    [SerializeField]
    private GameObject _AudioAmmunation;

    [SerializeField]
    private GameObject _RightEngine;
    [SerializeField]
    private GameObject _LeftEngine;
    [SerializeField]
    private GameObject _ExplotionDeath;

    private AudioSource _AudioLaserShot;
    [SerializeField]
    private AudioClip _AudioLaserShotClip;

    GameManager gameManager;

    UIManager _UIManagerObj;

    private bool IsTripleShotActive = false;
    [SerializeField]
    private bool IsMissileActive = false;

    private bool _IsPowerUpSpeedActive = false;
    [SerializeField]
    private bool _IsPowerUpShieldActive = false;

    private SpawnManager _spawnManager; 

    
    private float _firerate = 0.2f;
    private float _canfire = -1f;

    private float _canfireMissile = -1;
    private float _firerateMissile = 1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _ScorePlayer = 0;

    float _thrusterValue = 10.0f;
    float _ThrusterDecreaseRate = 0.05f;
    bool _CanThrusterWork = true;
    float _timeToThruster = -1.0f;
    float _ThruterRate = 15.0f;


    float xMaxLimit = 11.0f;
    float xMinLimit = -11.0f;
    float yMaxLimit = 0;
    float yMinLimit = -4.3f;

    private int _hitShieldcounter; //Variable which count the number of times that is hit the shield

    private int _AmmunitionLaserLimit; //Variable which is the limit od ammunation

    // Start is called before the first frame update
    void Start()
    {
        _hitShieldcounter = 0;
        _AmmunitionLaserLimit = 15;
        IsTripleShotActive = false;        
        IsMissileActive = false;        
        transform.position = Vector3.zero;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>(); //find the object. Get the Component
        _UIManagerObj = GameObject.Find("UI Manager").GetComponent<UIManager>(); //find the object. Get the Component Script
        _AudioLaserShot = GetComponent<AudioSource>();

        if (gameManager == null)
        {
            Debug.LogError("gameManager:: is NULL");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_UIManagerObj == null)
        {
            Debug.LogError("The UIManager is NULL");
        }

        if (_AudioLaserShot == null)
        {
            Debug.LogError("AudioSource is NULL");
        }
        else
        {
            _AudioLaserShot.clip = _AudioLaserShotClip;
        }

    }


    void Update()
    {
        ControlMovement();
        FireLaser();
        SpeedPowerUpMethod();
        _SpeedIncreaseShift();
        UpdateMunitonText();

    }

    void UpdateMunitonText()
    {
        _UIManagerObj.MunitionCountText(_AmmunitionLaserLimit.ToString());
    }

    void ControlMovement()
    {
        float Movefactor = _speed * Time.deltaTime;

        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.Translate(direction * Movefactor);

        if (transform.position.x >= xMaxLimit)
        {
            transform.position = new Vector3(xMinLimit, transform.position.y, 0);
        }
        else if (transform.position.x <= xMinLimit)
        {
            transform.position = new Vector3(xMaxLimit, transform.position.y, 0);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, yMinLimit, yMaxLimit), 0.0f);
    }

    //Move the player at an increased rate when the 'Left Shift' key is pressed down
    void _SpeedIncreaseShift() //The idea is desactivate this boost when Power Up Speed is active
    {
            if (Input.GetKey(KeyCode.LeftShift) && !_IsPowerUpSpeedActive)
            {                
                if (_CanThrusterWork)
                {
                    _speed = 8.0f;
                    _thrusterValue -= _ThrusterDecreaseRate;
                    _thrusterValue = Mathf.Clamp(_thrusterValue, 0.0f, 10.0f);
                    _UIManagerObj.ThrusterBarCharge(_thrusterValue);

                    if (_thrusterValue <= 0)
                    {
                        _CanThrusterWork = false;
                    }
                    
                }
                
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift) && !_IsPowerUpSpeedActive)
            {
                _speed = 5.0f;
                
            }


        if (!_CanThrusterWork)
        {

            if (_thrusterValue < 10.0f)
            {
                _thrusterValue += _ThrusterDecreaseRate;
                _thrusterValue = Mathf.Clamp(_thrusterValue, 0.0f, 10.0f);
                _UIManagerObj.ThrusterBarCharge(_thrusterValue);
                _timeToThruster = Time.time + _ThruterRate;
            }
            else if (Time.time > _timeToThruster)
            {
                _timeToThruster = Time.time + _ThruterRate;
                Debug.Log("_timeToThruster");
                _CanThrusterWork = true;
            }

        }


        
    }

    void FireLaser()
    {
        if (_AmmunitionLaserLimit > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
            {
                
                _canfire = Time.time + _firerate;
                // if triple_shot is true
                if (IsTripleShotActive && !IsMissileActive)
                {
                    _AmmunitionLaserLimit--;
                    //Vector3 offsetTriple = transform.position + new Vector3(-0.007733114f, -0.02844546f, -13.66089f);
                    Instantiate(_TripleShotLaser, transform.position, Quaternion.identity);
                    //play the laser audio clip
                    _AudioLaserShot.Play();
                } else if (IsMissileActive && !IsTripleShotActive)
                {
                    //Missile can be shot slowly to enhance the effect of heat-seeking
                    if (Time.time > _canfireMissile)
                    {
                        _AmmunitionLaserLimit--;
                        _canfireMissile = Time.time + _firerateMissile;
                        Instantiate(_MissileShot, transform.position, Quaternion.identity);
                        //play the laser audio clip
                        _AudioLaserShot.Play();
                    }
                    
                }
                else 
                {
                    _AmmunitionLaserLimit--;
                    Vector3 offset = transform.position + Vector3.up;
                    Instantiate(_LaserPrefab, offset, Quaternion.identity); //else fire 1 laser
                    //play the laser audio clip
                    _AudioLaserShot.Play();
                }


            }
        }else if (_AmmunitionLaserLimit <= 0)
        {
            _AudioAmmunation.gameObject.SetActive(true);
        }
    }

    void SpeedPowerUpMethod()
    {
        if (_IsPowerUpSpeedActive)
        {
            _speed = 10.0f;
        }
    }

    public void Damage()
    {
        gameManager.CameraShake();
        //if shield is active
        if (_IsPowerUpShieldActive)
        {
            _hitShieldcounter++;
            if (_hitShieldcounter == 1) //First hit activate shield effect
            {
                _ShieldProtection.GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 255);
                return;
            }else if(_hitShieldcounter == 2) //Second hit activate shield effect
            {
                _ShieldProtection.GetComponent<SpriteRenderer>().color = new Color(255, 0, 255, 255);
                return;
            }else if(_hitShieldcounter == 3) //Desactivate shield
            {
                _IsPowerUpShieldActive = false; //deactive shield
                _ShieldProtection.SetActive(false);
                _ShieldProtection.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                _hitShieldcounter = 0;
                return; //do nothing
            }

        }

        _lives--;
        if (_lives <= 0)
        {
            _lives = 0;
            _UIManagerObj.UpdateLives(_lives);
        }
        else if(_lives >= 3)
        {
            _lives = 3;
            _UIManagerObj.UpdateLives(_lives);
        }
        else
        {            
            _UIManagerObj.UpdateLives(_lives);
        }

        

        switch (_lives)
        {
            case 0:
                GameObject _ExplotionDeathClone = Instantiate(_ExplotionDeath, transform.position, Quaternion.identity);
                Destroy(_ExplotionDeathClone, 3.0f);
                break;
            case 1:
                _RightEngine.gameObject.SetActive(true);
                break;
            case 2:
                _LeftEngine.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _spawnManager.PowerUpStop();
          

            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        IsTripleShotActive = true;
        //Start Coroutine to power down coroutine for triple shot
        StartCoroutine(PowerDownTripleShotRoutine());
    }

    //IEnumerator TripleShot
    IEnumerator PowerDownTripleShotRoutine()
    {           
            yield return new WaitForSeconds(5.0f);
            IsTripleShotActive = false;
    }

    public void SpeedPowerUpActive()
    {
        _IsPowerUpSpeedActive = true;

        StartCoroutine(PowerUpSpeedRoutine());
    }

    IEnumerator PowerUpSpeedRoutine()
    {
        yield return new WaitForSeconds(7.0f);
        _IsPowerUpSpeedActive = false;
    }

    public void ShieldPowerUpActive()
    {
        _IsPowerUpShieldActive = true;
        _ShieldProtection.SetActive(true); //Active visualizer of Shield
        //StartCoroutine(PowerUpShieldRoutine()) ;
    }

    IEnumerator PowerUpShieldRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _IsPowerUpShieldActive = false;
    }

    public void AddPointsToScore()
    {
        _ScorePlayer += 10;
        _UIManagerObj.UpdateScore(_ScorePlayer.ToString());

    }

    public void RechargeAmmunition()
    {
            _AmmunitionLaserLimit += 15;        
    }

    public void RestoreHealth()
    {
        if (_lives < 3)
        {
            _lives++;
        }
        else
        {
            _lives = 3;
        }
        
        _UIManagerObj.UpdateLives(_lives);
        switch (_lives)
        {
            case 1:
                _RightEngine.gameObject.SetActive(false);
                break;
            case 2:
                _LeftEngine.gameObject.SetActive(false);
                break;
            case 3:
                _RightEngine.gameObject.SetActive(false);
                _LeftEngine.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void MissileActive()
    {
        IsMissileActive = true;
        //Start Coroutine to power down coroutine for triple shot
        StartCoroutine(PowerUpMisileRoutine());
    }

    //IEnumerator TripleShot
    IEnumerator PowerUpMisileRoutine()
    {
        yield return new WaitForSeconds(5f);
        IsMissileActive = false;
    }

}
