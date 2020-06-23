using UnityEngine;
using UnityEngine.UIElements;

public class MisileController : MonoBehaviour
{
    private float _speedMisile = 4;
    GameObject[] enemygameobject;
    GameObject enemyTarget;
    Collider2D enemyCollider;

    float xMaxLimit = 9.2f;
    float xMinLimit = -9.2f;
    float yMaxLimit = 7.2f;
    float yMinLimit = -2.0f;

    private void Start()
    {
        enemygameobject = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemygameobject == null)
        {
            Debug.LogError("enemygamoobject:: is NULL");
        }
    }
    private void Update()
    {
        TargetEnemy();
        FindEnemyOnScreen();
        enemyCollider = enemyTarget.GetComponent<Collider2D>();
    }

    private void TargetEnemy() //Algorith to rotate the player depending on the closest Enemy position
    {
        if (enemyCollider != null)
        {
            Vector3 DirectionToFace = (enemyCollider.transform.position - transform.position).normalized;

            float rotation_z = Mathf.Atan2(DirectionToFace.y, DirectionToFace.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotation_z - 90);

            MovementHeatSeeking(DirectionToFace);
        }
        else
        {
            Movement();
        }

    }
    
    private void MovementHeatSeeking(Vector3 DirectionToFace) //Movement toward to the Enemy Closest
    {
        transform.Translate(DirectionToFace * _speedMisile * Time.deltaTime);

        if(transform.position.y >= 7.0f)
        {
            Destroy(this.gameObject);
        }else if (transform.position.y <= -7.0f)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.x >= 11.0f)
        {
            Destroy(this.gameObject);
        }
        else if (transform.position.x <= -11.0f)
        {
            Destroy(this.gameObject);
        }

    }

    private void Movement() //Movement when are not enemies in the scene
    {
        transform.Translate(Vector3.up * _speedMisile * Time.deltaTime);

        if (transform.position.y >= 7.0f)
        {
            Destroy(this.gameObject);
        }
        else if (transform.position.y <= -7.0f)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.x >= 11.0f)
        {
            Destroy(this.gameObject);
        }
        else if (transform.position.x <= -11.0f)
        {
            Destroy(this.gameObject);
        }

    }

    private void FindEnemyOnScreen() //Algorith to find only one Enemy which is on the screen
    {        
        for (int i = enemygameobject.Length-1; i >= 0; --i)
        {
            if (enemygameobject[i] != null)
            {

                if (enemygameobject[i].transform.position.x > xMinLimit & enemygameobject[i].transform.position.x < xMaxLimit)
                {
                    if (enemygameobject[i].transform.position.y > yMinLimit & enemygameobject[i].transform.position.y < yMaxLimit)
                    {
                        enemyTarget = enemygameobject[i];
                        break;
                    }
                    else
                    {
                        enemyTarget = null;
                    }
                }
                else
                {
                    enemyTarget = null;
                }
            }
            else
            {
                enemyTarget = null;
            }            
        }
    }
}
