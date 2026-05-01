using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudio;
    [SerializeField] private AudioClip relaxMusic;
    [SerializeField] private AudioClip battleMusic;
    [SerializeField] private float delaySounds = 2f;
    [SerializeField] private float fadeSpeed = 1f;

    private int enemiesDetecting = 0;
    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        EnemyDetector.onEnemyEnter += EnemyDetected;
        EnemyDetector.onEnemyExit += EnemyLost;
        Enemy.OnDeath += EnemyLost;
    }

    private void OnDisable()
    {
        EnemyDetector.onEnemyEnter -= EnemyDetected;
        EnemyDetector.onEnemyExit -= EnemyLost;
        Enemy.OnDeath -= EnemyLost;
    }

    private void Start()
    {
        mainAudio = GetComponent<AudioSource>();
        mainAudio.clip = relaxMusic;
        mainAudio.volume = 1f;
        mainAudio.Play();
    }

    void EnemyDetected()
    {
        enemiesDetecting++;

        if (enemiesDetecting == 1)
            ChangeMusic(battleMusic);
    }

    void EnemyLost()
    {
        enemiesDetecting = Mathf.Max(0, enemiesDetecting - 1);

        if (enemiesDetecting == 0)
            ChangeMusic(relaxMusic);
    }

    void ChangeMusic(AudioClip newClip)
    {
        if (mainAudio.clip == newClip) return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeAndSwitch(newClip));
    }

    IEnumerator FadeAndSwitch(AudioClip newClip)
    {
        while (mainAudio.volume > 0.01f)
        {
            mainAudio.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        mainAudio.clip = newClip;
        mainAudio.Play();

        while (mainAudio.volume < 1f)
        {
            mainAudio.volume += fadeSpeed * Time.deltaTime;
            yield return null;
        }
    }
}