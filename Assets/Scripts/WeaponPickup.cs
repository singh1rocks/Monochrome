using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameManager.WeaponType weaponType;
    private SpriteRenderer spriteRend;
    [SerializeField] private GameObject pickupPrompt;

    private void Start()
    {
        pickupPrompt.SetActive(false);

        spriteRend = GetComponent<SpriteRenderer>();
        //switch weapon icon
        switch (weaponType)
        {
            case GameManager.WeaponType.TaterTot: //basic
                spriteRend.sprite = UIManager.instance.TaterTot_icon;
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                spriteRend.sprite = UIManager.instance.CookieShuriken_icon;
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                spriteRend.sprite = UIManager.instance.HotSauceSquirtGun_icon;
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                spriteRend.sprite = UIManager.instance.BaconCrossbow_icon;
                break;
            case GameManager.WeaponType.PopcornGrenade:
                spriteRend.sprite = UIManager.instance.PopcornGrenade_icon;
                break;
            case GameManager.WeaponType.StrawberryDrill:
                spriteRend.sprite = UIManager.instance.Drill_icon;
                break;
        }
    }

    private void Update()
    {
        switch (weaponType)
        {
            case GameManager.WeaponType.TaterTot: //basic
                spriteRend.sprite = UIManager.instance.TaterTot_icon;
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                spriteRend.sprite = UIManager.instance.CookieShuriken_icon;
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                spriteRend.sprite = UIManager.instance.HotSauceSquirtGun_icon;
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                spriteRend.sprite = UIManager.instance.BaconCrossbow_icon;
                break;
            case GameManager.WeaponType.PopcornGrenade:
                spriteRend.sprite = UIManager.instance.PopcornGrenade_icon;
                break;
            case GameManager.WeaponType.StrawberryDrill:
                spriteRend.sprite = UIManager.instance.Drill_icon;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickupPrompt.SetActive(true);
            if (pickupPrompt.GetComponent<WeaponPickupPrompt>().coroutineRunning)
            {
                StopCoroutine(pickupPrompt.GetComponent<WeaponPickupPrompt>().coroutine);
                pickupPrompt.GetComponent<WeaponPickupPrompt>().coroutineRunning = false;
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("Pickup");
                GameManager.WeaponType tempWeaponType = weaponType;
                weaponType = GameManager.instance.equippedWeapon;
                GameManager.instance.equippedWeapon = tempWeaponType;
                other.gameObject.GetComponent<ShootBullet>().flameObject.SetActive(false);
                //Destroy(gameObject);
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            pickupPrompt.SetActive(false);
        }
    }
}
