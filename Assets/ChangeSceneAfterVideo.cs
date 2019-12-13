using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ChangeSceneAfterVideo : MonoBehaviour
{
    VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(WaitForVideo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitForVideo()
    {
        yield return new WaitForSeconds((float)videoPlayer.length + 1f);
        SceneManager.LoadScene("Level");
        Debug.Log("change scene");
    }
}
