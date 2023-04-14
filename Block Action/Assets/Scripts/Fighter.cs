using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class Fighter : MonoBehaviour
{
    public int health;
    public double buff;
    public double defenseBuff;
    public Element.Elements baseElement;
    public Element.Elements currentElement;
    public int currentElementStack;
    public List<Status> statusEffects;
    public int stunChargeMax;
    public int stunCharge;
    public bool stunned
    {
        get
        {
            return stunCharge == stunChargeMax;
        }
    }
    public int maxHealth;
    public Healthbar healthBar;
    GameObject healthPrefab;
    public bool dead
    {
        get
        {
            return (health <= 0);
        }
    }
    public bool fadeOnDefeat = true;
    public bool faded = false;
    public float timeHovered;
    public FighterInfoMenu infoMenu;
    void Start()
    {
        timeHovered = 0;
    }

    public void makeHealthBar()
    {
        if (healthBar != null)
        {
            return;
        }
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
        if (health > 0)
        {
            healthBar.setHealth(health, maxHealth);
        }
        else
        {
            healthBar.setHealth(health, maxHealth);
        }
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

    void Update()
    {
        updateHealthBar();
    }

    void OnMouseOver()
    {
        if (timeHovered >= 0.3)
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

    public async Task Fade()
    {
        buff = 0;
        defenseBuff = 0;
        statusEffects.Clear();
        if (fadeOnDefeat)
        {
            await Task.Delay(200);
            SoulParticle s = null;
            if (this is Enemy enemy)
            {
                s = ParticleHandler.CreateSoulParticle(transform.position, enemy.soulColor);
            }
            SpriteRenderer fighterRenderer = GetComponent<SpriteRenderer>();
            float baseOpacity = fighterRenderer.color.a;
            for (int i = 0; i < 10; i++)
            {
                float newOpacity = fighterRenderer.color.a - (0.1f * baseOpacity);
                if (newOpacity < 0)
                {
                    newOpacity = 0;
                }
                fighterRenderer.color = new Color(fighterRenderer.color.r, fighterRenderer.color.g, fighterRenderer.color.b, newOpacity);
                if (s != null)
                {
                    s.IncreaseOpacity();
                }
                await Task.Delay(50);
            }
            if (s != null)
            {
                s.StartCoroutine(s.FlyToPlayer());
            }
            healthBar.gameObject.SetActive(false);
            if (infoMenu != null)
            {
                infoMenu.gameObject.SetActive(false);
            }
            gameObject.SetActive(false);
        }
        else
        {
            await Task.Delay(500);
        }
        faded = true;
    }
}
