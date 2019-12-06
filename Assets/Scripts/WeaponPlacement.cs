using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlacement : MonoBehaviour
{
    public float horizontalOffset;
    public float verticalOffset;
    public ShootBullet shootBulletScript;
    private Vector3 dirVec;
    public SpriteRenderer spriteRend;
    private Transform player_t;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        player_t = GameObject.FindWithTag("Player").GetComponent<Transform>();
        shootBulletScript = player_t.gameObject.GetComponent<ShootBullet>();
    }
    

    // Update is called once per frame
    void Update()
    {
        dirVec = shootBulletScript.dirVec;

        //set to render infront of player when pointing down, render behind player when pointing up
        if (dirVec.y >= 0)
        {
            spriteRend.sortingOrder = -5;
        }
        else
        {
            spriteRend.sortingOrder = 5;
        }

        //flip sprite for left and right sides
        if (dirVec.x >= 0)
        {
            spriteRend.flipY = true;
        }
        else
        {
            spriteRend.flipY = false;
        }

        //displace weapon icon horizontally and vertically
        transform.position = new Vector3(player_t.position.x + dirVec.x * horizontalOffset, player_t.position.y + dirVec.y * verticalOffset - 0.1f, player_t.position.z);

        //point at target
        Vector2 direction = new Vector2(dirVec.x - transform.position.x, dirVec.y - transform.position.y);
        transform.right = -dirVec;

        spriteRend.flipX = false;
        //sprite switch
        switch (GameManager.instance.equippedWeapon)
        {
            case GameManager.WeaponType.TaterTot: //basic
                spriteRend.sprite = GameManager.instance.TaterTot_sp;
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                spriteRend.sprite = GameManager.instance.CookieShuriken_sp;
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                spriteRend.sprite = GameManager.instance.HotSauceSquirtGun_sp;
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                spriteRend.sprite = GameManager.instance.BaconCrossbow_sp;
                break;
            case GameManager.WeaponType.PopcornGrenade: //piercing
                spriteRend.sprite = GameManager.instance.PopcornGrenade_sp;
                spriteRend.flipX = true;
                break;
        }
    }
}
