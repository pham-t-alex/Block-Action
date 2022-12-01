using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameText : MonoBehaviour
{
    private static GameObject _gameText;
    public static GameObject gameText
    {
        get {
            if (_gameText == null)
            {
                _gameText = GameObject.FindGameObjectWithTag("GameText");
            }
            return _gameText;
        }
    }
    float textDisplayTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        setText("");
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplayTime > 0)
        {
            textDisplayTime -= Time.deltaTime;
            if (textDisplayTime > 0 && textDisplayTime <= 0.5f)
            {
                GetComponent<TMP_Text>().color = new Color(1, 1, 1, textDisplayTime * 2);
            }
            else if (textDisplayTime <= 0)
            {
                GetComponent<TMP_Text>().text = "";
                textDisplayTime = 0;
            }
        }
    }

    public static void setText(string text)
    {
        gameText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        gameText.GetComponent<TMP_Text>().text = text;
        gameText.GetComponent<GameText>().textDisplayTime = 1 + (text.Length / 15);
    }

    public static void setTextPermanent(string text)
    {
        gameText.GetComponent<TMP_Text>().color = new Color(1, 1, 1, 1);
        gameText.GetComponent<TMP_Text>().text = text;
    }
}
