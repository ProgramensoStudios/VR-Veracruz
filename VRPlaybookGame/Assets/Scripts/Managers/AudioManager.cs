using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudio;
    [SerializeField, Tooltip("0 = Relax, 1 = Battle")] private AudioClip[] music;
    [SerializeField] private float delaySounds = 2f;
    [SerializeField] private float fadeSpeed = 1f;

    private Coroutine fadeCoroutine;

    private void OnEnable()
    {
        EnemyDetector.onEnemy += BattleMusic;
        Enemy.onDeath += RelaxMusic;
    }

    private void OnDisable()
    {
        EnemyDetector.onEnemy -= BattleMusic;
        Enemy.onDeath -= RelaxMusic;
    }

    private void Start()
    {
        mainAudio = GetComponent<AudioSource>();
        mainAudio.volume = 1f;
        mainAudio.clip = music[0];
        mainAudio.Play();
    }

    private void BattleMusic()
    {
        StopFade();

        mainAudio.volume = 1f;
        mainAudio.clip = music[1];
        mainAudio.Play();
    }

    private void RelaxMusic()
    {
        StopFade();
        fadeCoroutine = StartCoroutine(RelaxSequence());
    }

    private IEnumerator RelaxSequence()
    {
        // Fade out
        yield return StartCoroutine(FadeVolume(0f));

        // Espera
        yield return new WaitForSeconds(delaySounds);

        // Cambia música
        mainAudio.clip = music[0];
        mainAudio.Play();

        // Fade in
        yield return StartCoroutine(FadeVolume(1f));
    }

    private IEnumerator FadeVolume(float targetVolume)
    {
        while (!Mathf.Approximately(mainAudio.volume, targetVolume))
        {
            mainAudio.volume = Mathf.MoveTowards(
                mainAudio.volume,
                targetVolume,
                fadeSpeed * Time.deltaTime
            );

            yield return null;
        }
    }

    private void StopFade()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}
