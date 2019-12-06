using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeWhenChangingFloors : MonoBehaviour
{
    #region FIELDS
    public Image fadeOutUIImage;
    public float fadeSpeed = 0.8f;
    public enum FadeDirection
    {
        In, //Alpha = 1
        Out // Alpha = 0
    }
    private Transform player_transform;

    #endregion
    #region MONOBHEAVIOR
    void OnEnable()
    {
        if (GameObject.FindWithTag("Player"))
        {
            player_transform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        
        StartCoroutine(Fade(FadeDirection.Out));
    }
    #endregion
    #region FADE
    private IEnumerator Fade(FadeDirection fadeDirection)
    {
        float alpha = (fadeDirection == FadeDirection.Out) ? 1 : 0;
        float fadeEndValue = (fadeDirection == FadeDirection.Out) ? 0 : 1;
        if (fadeDirection == FadeDirection.Out)
        {
            while (alpha >= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
            fadeOutUIImage.enabled = false;
        }
        else
        {
            fadeOutUIImage.enabled = true;
            while (alpha <= fadeEndValue)
            {
                SetColorImage(ref alpha, fadeDirection);
                yield return null;
            }
        }
    }
    #endregion
    #region HELPERS
    public IEnumerator FadeAndMovePlayerTransform(Vector3 position, bool healPlayer)
    {
        yield return Fade(FadeDirection.In);
        if (position != new Vector3(1, 2, 3))
        {
            player_transform.position = position;
        }
        
        yield return Fade(FadeDirection.Out);
        if (healPlayer)
        {
            player_transform.gameObject.GetComponent<PlayerMovement>().health = player_transform.gameObject.GetComponent<PlayerMovement>().maxHealth;
        }

    }

    public IEnumerator FadeAndChangeLevel()
    {
        yield return Fade(FadeDirection.In);
        SceneManager.LoadScene("Level");
            //);
        //yield return Fade(FadeDirection.Out);

    }
    private void SetColorImage(ref float alpha, FadeDirection fadeDirection)
    {
        fadeOutUIImage.color = new Color(fadeOutUIImage.color.r, fadeOutUIImage.color.g, fadeOutUIImage.color.b, alpha);
        alpha += Time.deltaTime * (1.0f / fadeSpeed) * ((fadeDirection == FadeDirection.Out) ? -1 : 1);
    }
    #endregion
}