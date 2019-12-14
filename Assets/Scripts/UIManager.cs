using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance { private set; get; }//Singleton

    private void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    private PlayerMovement playerMovement;

    public Canvas canvas;
    public Slider healthSlider;
    public Image weaponIcon;

    [Header("Weapon Icon Sprites")]
    public Sprite TaterTot_icon;
    public Sprite BaconCrossbow_icon;
    public Sprite HotSauceSquirtGun_icon;
    public Sprite CookieShuriken_icon;
    public Sprite PopcornGrenade_icon;
    public Sprite Drill_icon;

    [Header("Boss Health")]
    public Slider bossHealthSlider;
    public GameObject bossHealthBarObject;
    public GameObject bossObject;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        healthSlider.maxValue = playerMovement.health;
        bossHealthBarObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //health slider
        healthSlider.value = healthSlider.maxValue - playerMovement.health;

        //switch weapon icon
        switch (GameManager.instance.equippedWeapon)
        {
            case GameManager.WeaponType.TaterTot: //basic
                weaponIcon.sprite = TaterTot_icon;
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                weaponIcon.sprite = CookieShuriken_icon;
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                weaponIcon.sprite = HotSauceSquirtGun_icon;
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                weaponIcon.sprite = BaconCrossbow_icon;
                break;
            case GameManager.WeaponType.PopcornGrenade: //piercing
                weaponIcon.sprite = PopcornGrenade_icon;
                break;
            case GameManager.WeaponType.StrawberryDrill:
                weaponIcon.sprite = Drill_icon;
                break;

        }

        if (bossObject)
        {
            if (bossObject.activeSelf)
            {
                bossHealthBarObject.SetActive(true);
                bossHealthSlider.maxValue = bossObject.GetComponent<Boss>().maxHealth;
                bossHealthSlider.value = bossObject.GetComponent<Boss>().health;
            }
            else
            {
                bossHealthBarObject.SetActive(false);
            }
        }
        
    }
}
