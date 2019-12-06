using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupPrompt : MonoBehaviour
{
    SpriteRenderer spriteRend;
    [HideInInspector] public Coroutine coroutine;

    [HideInInspector] public bool coroutineRunning;

    // Start is called before the first frame update
    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
        //coroutineRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!coroutineRunning)
        {
            coroutine = StartCoroutine(FlashSprite());
        }
        
    }

    IEnumerator FlashSprite()
    {
        coroutineRunning = true;
        spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 0f);
        yield return new WaitForSeconds(0.5f);

        spriteRend.color = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, 1f);
        yield return new WaitForSeconds(0.5f);
        coroutineRunning = false;
    }
}
