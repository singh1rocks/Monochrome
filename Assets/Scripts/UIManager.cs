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
    }
}
