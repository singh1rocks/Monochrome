using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    private PlayerMovement p_controller;
    private Camera cam;
    public Vector3 target;
    public Vector3 dirVec;
    public float bulletOffset;
    private Vector3 mousePos;
    private float mouseZPos;
    [SerializeField] private WeaponPlacement weaponPlacementScript;

    [Header("Counter for reload UI")]
    public float UITimeCounter;
    public float UITime;
    public GameObject slider;

    [Header("Tater Tot")]
    public GameObject taterTotBulletPrefab;
    public float taterTotBulletTime; //minimum delay between each bullet fired
    private float taterTotBulletTimeCounter;
    public AudioSource taterSFX;

    [Header("Cookie Shuriken")]
    public GameObject cookieBulletPrefab;
    public float cookieBulletTime; //minimum delay between each bullet fired
    private float cookieBulletTimeCounter;
    public AudioSource cookieSFX;

    [Header("Bacon Bolt")]
    public GameObject baconBulletPrefab;
    public float baconBulletTime; //minimum delay between each bullet fired
    private float baconBulletTimeCounter;
    public AudioSource baconSFX;

    [Header("Popcorn Grenade Launcher")]
    public GameObject grenadePrefab;
    public float grenadeBulletTime; //minimum delay between each bullet fired
    private float grenadeBulletTimeCounter;
    public AudioSource fireGrenadeSFX;

    [Header("Flame Sauce")]
    public AudioSource flameSFX;
    public GameObject flameObject;

    [Header("Strawberry Drill")]
    public GameObject drillProjectile;
    public float drillTime;
    private float drillTimeCounter;

    [Header("Churro Sword")]
    public AudioSource swordSFX;
    public GameObject swordPrefab;
    public float swingTime; //minimum delay between each bullet fired
    private float swingTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        p_controller = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        cam = Camera.main;
        GetMousePosForShooting();
        dirVec = new Vector3(target.x - transform.position.x, target.y - transform.position.y, 0f);
        dirVec.Normalize();

        //set to invisible
        flameObject.SetActive(false);

        //init values for specific weapon type
        switch (GameManager.instance.equippedWeapon)
        {
            case GameManager.WeaponType.TaterTot: //basic
                taterTotBulletTimeCounter = 0;
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                cookieBulletTimeCounter = 0;
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                break;
            case GameManager.WeaponType.PopcornGrenade: //grenade launcher
                grenadeBulletTimeCounter = 0;
                break;
            case GameManager.WeaponType.ChurroSword: //sword
                swingTimeCounter = 0;
                break;
            case GameManager.WeaponType.StrawberryDrill:
                drillTimeCounter = 0f;
                break;
        }
    }

    private void GetMousePosForShooting()
    {
        mousePos = Input.mousePosition;
        int layerMask = 1 << 31;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Collide);
        target = hit.point;
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePosForShooting();
        dirVec = new Vector3(target.x - transform.position.x, target.y - transform.position.y, 0f);
        dirVec.Normalize();

        if (GameManager.instance.equippedWeapon != GameManager.WeaponType.HotSauceSquirtGun)
        {
            flameObject.SetActive(false);
        }

        //shoot bullet
        switch (GameManager.instance.equippedWeapon)
        {
            case GameManager.WeaponType.TaterTot: //basic
                TaterTot();
                UITime = taterTotBulletTime;
                UITimeCounter = taterTotBulletTimeCounter;
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                Shotgun();
                UITime = cookieBulletTime;
                UITimeCounter = cookieBulletTimeCounter;
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                Flamethrower();
                UITime = 0f;
                UITimeCounter = 0f;
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                BaconCrossBow();
                UITime = baconBulletTime;
                UITimeCounter = baconBulletTimeCounter;
                break;
            case GameManager.WeaponType.PopcornGrenade: // grenade launcher
                GrenadeLauncher();
                UITime = grenadeBulletTime;
                UITimeCounter = grenadeBulletTimeCounter;
                break;
            case GameManager.WeaponType.ChurroSword: //sword
                Sword();
                UITime = 0f;
                UITimeCounter = 0f;
                break;
            case GameManager.WeaponType.StrawberryDrill: // dril
                Strawberry();
                UITime = drillTime;
                UITimeCounter = drillTimeCounter;
                break;
        }

        if (UITime != 0f)
        {
            slider.SetActive(true);
            float xvalue;
            if (UITimeCounter < UITime)
            {
                xvalue = 0.2f * (UITimeCounter) / UITime;
            }
            else
            {
                xvalue = 0.2f;
            }

            slider.transform.localScale = new Vector3(xvalue, slider.transform.localScale.y, slider.transform.localScale.z);
        }
        else
        {
            slider.SetActive(false);
        }
        
    }

    private void Strawberry()
    {
        if (Input.GetMouseButton(0) && drillTimeCounter >= drillTime)
        {
            Instantiate(drillProjectile, transform.position + dirVec * bulletOffset, Quaternion.identity);
            drillTimeCounter = 0f;
            weaponPlacementScript.drillReloading = true;
        }
        else if (drillTimeCounter >= drillTime)
        {
            weaponPlacementScript.drillReloading = false;
        }
        drillTimeCounter += Time.deltaTime;
    }

    private void GrenadeLauncher()
    {
        if (Input.GetMouseButton(0) && grenadeBulletTimeCounter >= grenadeBulletTime)
        {
            Instantiate(grenadePrefab, transform.position + dirVec * bulletOffset, Quaternion.identity);
            grenadeBulletTimeCounter = 0;

            //play audio
            AudioManager.instance.PlaySingle(fireGrenadeSFX);
        }
        grenadeBulletTimeCounter += Time.deltaTime;
    }
    
    private void TaterTot()
    {
        if (Input.GetMouseButton(0) && taterTotBulletTimeCounter >= taterTotBulletTime)
        {
            Instantiate(taterTotBulletPrefab, transform.position + dirVec * bulletOffset, Quaternion.identity);
            taterTotBulletTimeCounter = 0;

            //play audio
            AudioManager.instance.PlaySingle(taterSFX);
        }
        taterTotBulletTimeCounter += Time.deltaTime;
    }

    private void Shotgun()
    {
        if (Input.GetMouseButton(0) && cookieBulletTimeCounter >= cookieBulletTime)
        {
            CreateBulletAtAngle(cookieBulletPrefab, 30f);
            CreateBulletAtAngle(cookieBulletPrefab, 15f);
            CreateBulletAtAngle(cookieBulletPrefab);
            CreateBulletAtAngle(cookieBulletPrefab, -15f);
            CreateBulletAtAngle(cookieBulletPrefab, -30f);
            cookieBulletTimeCounter = 0;

            //play audio
            AudioManager.instance.PlaySingle(cookieSFX);
        }
        cookieBulletTimeCounter += Time.deltaTime;
    }

    private void CreateBulletAtAngle(GameObject bulletPrefab, float angleOffset = 0f)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position + dirVec * bulletOffset;

        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * dirVec * 300.0f, ForceMode.Acceleration);
    }

    private void BaconCrossBow()
    {
        if (Input.GetMouseButton(0) && baconBulletTimeCounter >= baconBulletTime)
        {
            Instantiate(baconBulletPrefab, transform.position + dirVec * bulletOffset, Quaternion.identity);
            baconBulletTimeCounter = 0;

            //play audio
            AudioManager.instance.PlaySingle(baconSFX);
        }
        baconBulletTimeCounter += Time.deltaTime;
    }

    private void Flamethrower()
    {
        if (Input.GetMouseButton(0))
        {
            flameObject.SetActive(true);
        }
        else
        {
            flameObject.SetActive(false);
        }
    }

    private void Sword()
    {
        if (Input.GetMouseButton(0) && swingTimeCounter >= swingTime)
        {
            Instantiate(swordPrefab, transform.position + dirVec * 0.5f, Quaternion.LookRotation(swordPrefab.transform.forward, dirVec));
            swingTimeCounter = 0;

            //play audio
            //AudioManager.instance.PlaySingle(swordSFX);
        }
        swingTimeCounter += Time.deltaTime;
    }
}
