using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera cameraObj;
    Vector3 OriginalPos;

    private float _TimeCameraShake = 0.0f;

    private bool _GameOverActive;
    // Start is called before the first frame update
    void Start()
    {
        OriginalPos = cameraObj.transform.position;
        _GameOverActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _GameOverActive)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

    }

    public void GameOverIsActive()
    {
        _GameOverActive = true;
    }

    public void CameraShake()
    {
        StartCoroutine(CameraShakeRoutine());
    }

    IEnumerator CameraShakeRoutine()
    {
        _TimeCameraShake = 0.0f;

        while (_TimeCameraShake < 0.5f)
        {
            _TimeCameraShake += Time.deltaTime;
            cameraObj.transform.position = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), OriginalPos.z);
            yield return null;
        }
        
        cameraObj.transform.position = OriginalPos;
    }

}
