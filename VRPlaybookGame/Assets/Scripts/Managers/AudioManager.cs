using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource mainAudio;
    [SerializeField, Tooltip("0 = Relax, 1 = Battle")] private AudioClip[] music;
    [SerializeField] private float delaySounds;

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

    void Start()
    {
        mainAudio = GetComponent<AudioSource>();
    }

    private void BattleMusic()
    {
        mainAudio.clip = music[1];
        mainAudio.Play();
    }

    private void RelaxMusic()
    {
        StartCoroutine(DelayToChange());
    }

    IEnumerator DelayToChange()
    {
        yield return new WaitForSeconds(delaySounds);
        mainAudio.clip = music[0];
        mainAudio.Play();
    }

}
