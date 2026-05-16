using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip swapClip;
    public AudioClip matchClip;
    public AudioClip bonusClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip uiClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySwap()
    {
        PlaySfx(swapClip);
    }

    public void PlayMatch()
    {
        PlaySfx(matchClip);
    }

    public void PlayBonus()
    {
        PlaySfx(bonusClip);
    }

    public void PlayWin()
    {
        PlaySfx(winClip);
    }

    public void PlayLose()
    {
        PlaySfx(loseClip);
    }

    public void PlayUI()
    {
        PlaySfx(uiClip);
    }
}
