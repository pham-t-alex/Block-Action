using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleEndController : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameObject _gameEndText;
    public static GameObject gameEndText
    {
        get
        {
            if (_gameEndText == null)
            {
                _gameEndText = GameObject.FindGameObjectWithTag("GameEndText");
            }
            return _gameEndText;
        }
    }

    private static BattleEndController _battleEndController;
    public static BattleEndController battleEndController
    {
        get
        {
            if (_battleEndController == null)
            {
                _battleEndController = FindObjectOfType<BattleEndController>();
            }
            return _battleEndController;
        }
    }

    float delay = 4;
    public bool victorious;
    void Start()
    {
        gameEndText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Battle.b.bs == BattleState.End)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
        }
        if (delay < 0)
        {
            GameText.setTextPermanent("Click anywhere to continue.");
            delay = 0;
        }
        if (delay == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (victorious)
                {
                    SceneManager.LoadScene("Story");
                }
                else
                {
                    SceneManager.LoadScene("LevelSelection");
                }
            }
        }
    }

    public static void TriggerVictory()
    {
        if (Battle.b.bs != BattleState.End)
        {
            battleEndController.victorious = true;
            Battle.b.bs = BattleState.End;
            ScreenDarkener.DarkenScreen();
            gameEndText.SetActive(true);
            gameEndText.GetComponent<TMP_Text>().color = new Color(0.8f, 1, 1);
            battleEndController.StartCoroutine(ShowEndText(gameEndText.GetComponent<TMP_Text>(), "VICTORY"));
            GameObject.FindGameObjectWithTag("PauseButton").SetActive(false);
        }
    }

    public static void TriggerDefeat()
    {
        if (Battle.b.bs != BattleState.End)
        {
            battleEndController.victorious = false;
            Battle.b.bs = BattleState.End;
            ScreenDarkener.DarkenScreen();
            gameEndText.SetActive(true);
            gameEndText.GetComponent<TMP_Text>().color = new Color(0.6f, 0, 0);
            battleEndController.StartCoroutine(ShowEndText(gameEndText.GetComponent<TMP_Text>(), "DEFEAT"));
            GameObject.FindGameObjectWithTag("PauseButton").SetActive(false);
        }
    }

    static IEnumerator ShowEndText(TMP_Text textComponent, string text)
    {
        textComponent.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            textComponent.text += text[i];
            yield return new WaitForSeconds(0.3f);
        }
    }
}
