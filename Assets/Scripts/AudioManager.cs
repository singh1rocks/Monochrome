using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance { private set; get; }//Singleton

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

    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.           
    public float lowPitchRange = .9f;              //The lowest a sound effect will be randomly pitched.
    public float highPitchRange = 1.1f;            //The highest a sound effect will be randomly pitched.

    //plays a single sound clip
    public void PlaySingle(AudioSource source)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        source.pitch = 1f;
        source.pitch *= randomPitch;

        source.Play();
    }

    //randomize sfx chooses randomly between various audio clips and slightly changes their pitch.
    public void RandomizeSfx(params AudioSource[] clips)
    {
        //generate a random number between 0 and the length of our array of clips passed in.
        int randomIndex = Random.Range(0, clips.Length);

        //play the clip with random pitch
        PlaySingle(clips[randomIndex]);
    }
}
