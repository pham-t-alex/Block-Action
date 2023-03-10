using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fighter : MonoBehaviour
{
    public int health;
    public double buff;
    public double defenseBuff;
    public List<BuffCounter> buffLeft;
    public List<DefenseBuffCounter> defenseBuffLeft;
    public List<Status> statusEffects;
    public int maxHealth;
    public Healthbar healthBar;
    GameObject healthPrefab;
    public bool dead = false;
    public float timeHovered;
    public FighterInfoMenu infoMenu;
    void Start()
    {
        timeHovered = 0;
    }

    public void makeHealthBar()
    {
        healthPrefab = Resources.Load<GameObject>("Healthbar");
        Vector3 healthBarPosition = new Vector3(transform.position.x, transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2) - 0.5f, 0);
        Vector3 healthBarPos2 = WorldToScreenSpace(healthBarPosition, Camera.main, Healthbar.healthCanvas.GetComponent<RectTransform>());
        GameObject g = Instantiate(healthPrefab, Vector3.zero, Quaternion.identity);
        g.transform.SetParent(Healthbar.healthCanvas.transform);
        g.GetComponent<RectTransform>().anchoredPosition = healthBarPos2;
        g.transform.localScale = new Vector3(0.3f, 0.25f, 1);
        healthBar = g.GetComponent<Healthbar>();
    }

    public void updateHealthBar()
    {
        healthBar.setHealth(100 * health / maxHealth);
    }

    public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera cam, RectTransform area)
    {
        Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
        screenPoint.z = 0;

        Vector2 screenPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out screenPos))
        {
            return screenPos;
        }

        return screenPoint;
    }

    void update() { 
    
    }

    void OnMouseOver()
    {
        if (timeHovered >= 1)
        {
            CreateInfoMenu();
        }
        else
        {
            timeHovered += Time.deltaTime;
        }
    }

    public void CreateInfoMenu()
    {
        if (infoMenu == null)
        {
            Vector3 infoPosition = transform.position;
            GameObject infoCanvas = GameObject.FindGameObjectWithTag("InfoMenus");
            float minX = (-1 * Camera.main.orthographicSize * Screen.width / Screen.height) + 3.5f;
            float maxX = (Camera.main.orthographicSize * Screen.width / Screen.height) - 3.5f;
            infoPosition.y += 2;
            infoPosition.x = Mathf.Max(infoPosition.x, minX);
            infoPosition.x = Mathf.Min(infoPosition.x, maxX);
            Vector3 infoPosition2 = WorldToScreenSpace(infoPosition, Camera.main, infoCanvas.GetComponent<RectTransform>());
            GameObject g = Instantiate(FighterController.fighterController.fighterInfoMenu, Vector3.zero, Quaternion.identity);
            g.transform.SetParent(infoCanvas.transform);
            g.GetComponent<RectTransform>().anchoredPosition = infoPosition2;
            g.transform.localScale = new Vector3(1, 1, 1);
            infoMenu = g.GetComponent<FighterInfoMenu>();

            infoMenu.SetTitle(GetName());
            infoMenu.SetInfo(GetInfo());
        }
    }

    public void DestroyInfoMenu()
    {
        if (infoMenu != null)
        {
            Destroy(infoMenu.gameObject);
            infoMenu = null;
        }
    }

    public abstract string GetName();

    public abstract string GetInfo();
}
