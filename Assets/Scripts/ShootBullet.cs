using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    private PlayerMovement p_controller;
    private Camera cam;
    private Vector3 target;
    public Vector3 dirVec;
    public float bulletOffset;
    public Vector3 mousePos;

    [Header("Tater Tot")]
    public GameObject taterTotBulletPrefab;
    public float taterTotBulletTime; //minimum delay between each bullet fired
    private float taterTotBulletTimeCounter;

    [Header("Cookie Shuriken")]
    public GameObject cookieBulletPrefab;
    public float cookieBulletTime; //minimum delay between each bullet fired
    private float cookieBulletTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        p_controller = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        cam = Camera.main;
        mousePos = Input.mousePosition;
        mousePos.z = 5;

        //init values for specific weapon type
        switch (GameManager.instance.equippedWeapon)
        {
            case GameManager.WeaponType.TaterTot: //basic
                taterTotBulletTimeCounter = 0;
                break;
            case GameManager.WeaponType.CookieShuriken: //spread shot
                break;
            case GameManager.WeaponType.HotSauceSquirtGun: // flamethrower
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
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
                break;
            case GameManager.WeaponType.BaconCrossbow: //piercing
                break;
        }
    }

    private void TaterTot()
    {
        if (Input.GetMouseButton(0) && taterTotBulletTimeCounter >= taterTotBulletTime)
        {
            Instantiate(taterTotBulletPrefab, transform.position + dirVec * bulletOffset, Quaternion.identity);
            taterTotBulletTimeCounter = 0;
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
        }
        cookieBulletTimeCounter += Time.deltaTime;
    }

    private void CreateBulletAtAngle(GameObject bulletPrefab, float angleOffset = 0f)
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position + dirVec * bulletOffset;

        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * dirVec * 100.0f, ForceMode.Acceleration);
    }
}
