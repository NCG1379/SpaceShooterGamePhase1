using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text ScoreTextObj;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _LivesSprites;
    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _RToRestart;
    [SerializeField]
    private Text _MunitionCount;
    [SerializeField]
    private Slider _ThrusterBar;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager::GameOver() is NULL");
        }

        ScoreTextObj.text = "Score: " + 0;
        _GameOverText.gameObject.SetActive(false);
        _RToRestart.gameObject.SetActive(false);

        _ThrusterBar.value = 10.0f;
    }


    public void UpdateScore(string scoreMessage)
    {
        ScoreTextObj.text = "Score: " + scoreMessage;
    }

    public void UpdateLives(int currentLives)
    {
        
        _LivesImg.sprite = _LivesSprites[currentLives];

        

        if (currentLives == 0)
        {
            gameManager.GameOverIsActive();
            _RToRestart.gameObject.SetActive(true);
            _GameOverText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
        }
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _GameOverText.text = "Game Over";
            yield return new WaitForSeconds(0.5f);
            _GameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void MunitionCountText(string Munition)
    {
        _MunitionCount.text = Munition;
    }

    public void ThrusterBarCharge(float ThrusterValue)
    {
        _ThrusterBar.value = Mathf.Clamp(ThrusterValue, 0.0f, 10.0f);
    }


}
