using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlockInfoMenuHandler : MonoBehaviour
{
    public bool set;
    private static BlockInfoMenuHandler _infoMenuHandler;
    public static BlockInfoMenuHandler InfoMenuHandler
    {
        get
        {
            if (_infoMenuHandler == null)
            {
                _infoMenuHandler = FindObjectOfType<BlockInfoMenuHandler>();
            }
            return _infoMenuHandler;
        }
    }
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
                _info = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            }
            return _info;
        }
    }
    private void Awake()
    {
        Remove();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Set(string title, string info)
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        set = true;
        SetTitle(title);
        SetInfo(info);
    }
    public void Remove()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        SetTitle("");
        SetInfo("");
        set = false;
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
