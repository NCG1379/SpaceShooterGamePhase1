using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speedLaser = 3;

    [SerializeField]
    private bool _IsEnemyLaser = false;


    // Update is called once per frame
    void Update()
    {
        if (_IsEnemyLaser == false)
        {
            MovementUp();
        }
        else
        {
            MovementDown();
        }

    }

    void MovementUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speedLaser);

        if (transform.position.y >= 6.0f)
        {
            //check if this object has a parent
            //destroy the parent too!

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void MovementDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speedLaser);

        if (transform.position.y <= -6.0f)
        {
            //check if this object has a parent
            //destroy the parent too!

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _IsEnemyLaser = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _IsEnemyLaser == true)
        {
            Player_Controller _PlayerController = other.GetComponent<Player_Controller>();
            if (_PlayerController != null)
            {
                _PlayerController.Damage();
            }            
            Destroy(this.gameObject);
        }
    }
}
