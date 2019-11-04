using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletTime; //minimum delay between each bullet fired
    public float bulletTimeCounter;
    private PlayerMovement p_controller;
    private Camera cam;
    private Vector3 target;
    public Vector3 dirVec;
    public float bulletOffset;
    public Vector3 mousePos;


    // Start is called before the first frame update
    void Start()
    {
        p_controller = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();

        cam = Camera.main;
        mousePos = Input.mousePosition;
        mousePos.z = 5;

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
        if (Input.GetMouseButton(0) && bulletTimeCounter >= bulletTime)
        {
            //Instantiate(bulletPrefab, transform.position + dirVec * bulletOffset, Quaternion.identity);
            CreateBullet(30f);
            CreateBullet();
            CreateBullet(-30f);
            bulletTimeCounter = 0;
        }
        bulletTimeCounter += Time.deltaTime;
    }

    private void CreateBullet(float angleOffset = 0f)
    {
        GameObject bullet = Instantiate<GameObject>(bulletPrefab);
        bullet.transform.position = transform.position + dirVec * bulletOffset;

        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.AddForce(Quaternion.AngleAxis(angleOffset, Vector3.forward) * dirVec * 100.0f, ForceMode.Acceleration);
    }
}
