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

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        healthSlider.maxValue = playerMovement.health;
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
        }
    }
}
