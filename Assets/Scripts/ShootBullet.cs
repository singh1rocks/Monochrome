using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    private PlayerMovement p_controller;
    private Camera cam;
    public Vector3 target;
    public Vector3 dirVec;
    public float bulletOffset;
    private Vector3 mousePos;
    

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

    // Start is called before the first frame update
    void Start()
    {
        p_controller = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        cam = Camera.main;
        mousePos = Input.mousePosition;
        mousePos.z = 5;
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

        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePos.z = 5;
        target = cam.ScreenToWorldPoint(mousePos);
        dirVec = new Vector3(target.x - transform.position.x, target.y - transform.position.y, 0f);
        dirVec.Normalize();

        //shoot bullet
        switch (GameManager.instance.equippedWeapon)
        {
            case GameManager.WeaponType.TaterTot: //basic
                TaterTot();
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                Shotgun();
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                Flamethrower();
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                BaconCrossBow();
                break;
            case GameManager.WeaponType.PopcornGrenade: // grenade launcher
                GrenadeLauncher();
                break;
        }
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
            CreateBulletAtAngle(cookieBulletPrefab);
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
            //flameObject.SetActive(false);
        }
    }
}
