using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockInfoMenu : MonoBehaviour
{
    TMP_Text _title;
    public TMP_Text title
    {
        get
        {
            if (_title == null)
            {
                _title = transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
            }
            return _title;
        }
    }
    TMP_Text _info;
    public TMP_Text info
    {
        get
        {
            if (_info == null)
            {
                _info = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
            }
            return _info;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTitle(string newTitle)
    {
        title.text = newTitle;
    }

    public void SetInfo(string newInfo)
    {
        info.text = newInfo;
    }
}
