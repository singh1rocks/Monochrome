using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public GameManager.WeaponType weaponType;
    private SpriteRenderer spriteRend;

    private void Start()
    {
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.equippedWeapon = weaponType;
            Destroy(gameObject);
        }
    }
}
