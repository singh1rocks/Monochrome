using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance { private set; get; }//Singleton

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

    public WeaponType equippedWeapon;

    public enum WeaponType
    {
        TaterTot,
        BaconCrossbow,
        HotSauceSquirtGun,
        CookieShuriken
    }

    [Header("Weapon Sprites")]
    public Sprite TaterTot_sp;
    public Sprite BaconCrossbow_sp;
    public Sprite HotSauceSquirtGun_sp;
    public Sprite CookieShuriken_sp;

    // Start is called before the first frame update
    void Start()
    {
        equippedWeapon = WeaponType.TaterTot;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
}
