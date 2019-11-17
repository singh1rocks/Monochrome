using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private Vector3 target;
    private Transform grenade_t;
    private Rigidbody grenade_rb;
    private SphereCollider grenade_collider;
    private Transform player_t;
    private float time; //time for ball to reach hoop
    private float timeCounter;
    private float timeBall; //time for ball to reach highest point
    private float timeHoop; //time for ball to reach highest point if released from height of hoop
    private float initialVerticalVelocity;
    private float initialHorizontalVelocity;
    [SerializeField] private float trajectoryMaxHeight;
    [SerializeField] private float grenadeDelay; //time from grenade landing, to detonating
    private float grenadeDelayCounter;
    [SerializeField] private GameObject explosionPrefab;

    private bool hasLanded;

    // Start is called before the first frame update
    void Start()
    {
        grenade_t = GetComponent<Transform>();
        grenade_rb = GetComponent<Rigidbody>();
        grenade_collider = GetComponent<SphereCollider>();

        //set trajectory height of grenade
        player_t = GameObject.FindWithTag("Player").transform;
        float trajectoryOffset = Mathf.Abs(player_t.position.y - target.y) * 0.3f;
        if (player_t.position.y >= target.y)
        {
            trajectoryMaxHeight = trajectoryOffset;
        }
        else
        {
            trajectoryMaxHeight = target.y - player_t.position.y + trajectoryOffset;
        }

        SetFireVel(trajectoryMaxHeight);
        timeCounter = 0;
        grenadeDelayCounter = 0;
        hasLanded = false;
        target = GameObject.FindWithTag("Player").GetComponent<ShootBullet>().target;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter >= time)
        {
            grenade_rb.velocity = new Vector3(0f, 0f, 0f);
            grenade_collider.enabled = true;
            hasLanded = true;
        }
        timeCounter += Time.deltaTime;

        if (!hasLanded)
        {
            grenade_collider.enabled = false;
            grenade_rb.useGravity = false;
        }
        else
        {
            grenade_collider.enabled = true;

            if (grenadeDelayCounter >= grenadeDelay)
            {
                //detonate grenade
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            grenadeDelayCounter += Time.deltaTime;
        }
    }

    public void SetFireVel(float tragectoryMaxHeight)
    {
        #region First SUVAT implementation explaination
        /*
         * Implement SUVAT equations for vertical (when ball reaches highest point)
         * 
         * S = tragectoryMaxHeight - ball.position.y
         * U = ?                                        //initialVerticalVelocity
         * V = 0
         * A = -9.8 * gravScale
         * T = ?                                        //timeBall
         * 
         * Using v^2 = u^2 + 2as
         *       u = sqrt(2as)
         *       
         * Using s = 0.5(u + v) * t
         *       t = 2s/u
         * 
         */
        #endregion

        initialVerticalVelocity = Mathf.Sqrt(Mathf.Abs((float)(2 * (9.8) * (tragectoryMaxHeight - grenade_t.position.y))));
        timeBall = 2 * (tragectoryMaxHeight - grenade_t.position.y) / initialVerticalVelocity;

        #region Second SUVAT implementation explaination
        /*
         * Second SUVAT equation for vertical when ball is released from hoop height
         * 
         * S = tragectoryMaxHeight - target.position.y
         * U = ?                                        //vertVelHoop
         * V = 0
         * A = -9.8 * gravScale
         * T = ?                                        //timeHoop
         * 
         * Using v^2 = u^2 + 2as
         *       u = sqrt(2as)
         * 
         * Using s = 0.5(u + v) * t
         *       t = 2s/u
         *       
         */
        #endregion

        float vertVelHoop;//initial vertical velocity if released from hoop height

        vertVelHoop = Mathf.Sqrt(Mathf.Abs((float)(2 * (9.8) * (tragectoryMaxHeight - target.y))));
        timeHoop = 2 * (tragectoryMaxHeight - target.y) / vertVelHoop;

        //time for tragectory from release until hoop
        time = (timeBall - timeHoop) + timeHoop * 2;

        #region Last SUVAT implementation explaination
        /*
         * Using SUVAT horizontally
         * 
         * S = target.position.x - ball.position.x
         * U = ?                                        //initialHorizontalVelocity to be found
         * V = ?
         * A = 0
         * T = time
         * 
         * Using s = ut + 0.5 at^2
         *       u = s/t
         * 
         */
        #endregion
        initialHorizontalVelocity = (target.x - grenade_t.position.x) / time;

        //set ball velocity to initialHorizontalVelocity and initialVerticalVelocity
        grenade_rb.velocity = new Vector3(initialHorizontalVelocity, initialVerticalVelocity, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && hasLanded)
        {
            //detonate grenade of has landed and has hit grenade even if delay time has not elapsed
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
