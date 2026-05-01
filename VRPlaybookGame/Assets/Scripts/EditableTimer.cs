using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class EditableTimer : MonoBehaviour
{
    [Header("Configuraci�n del Timer")]
    [Tooltip("Duracion del temporizador en segundos.")]
    [SerializeField] private float timerDuration = 180f;
    [SerializeField] private float timeAdded;

    [Header("Eventos del Timer")]
    [Tooltip("Evento que se dispara cuando el temporizador termina.")]
    public static Action onTimerEnd;

    [SerializeField] private GameObject endGame;

    private float remainingTime;
    private bool isRunning = false;

    [SerializeField] private Image image;
    [SerializeField] private ContinuousMoveProvider _continuousMoveProvider;

    void Start()
    {
        StartTimer();
    }

    private void OnEnable()
    {
        Collectable.onCollect += AddTime;
    }

    private void OnDisable()
    {
        Collectable.onCollect -= AddTime;
    }

    void Update()
    {
        if (isRunning)
        {
            remainingTime -= Time.deltaTime;
            ConvertirATiempo(remainingTime);

            if (remainingTime <= 0)
            {
                isRunning = false;
                remainingTime = 0;
                EndTime();
                onTimerEnd?.Invoke();
                Time.timeScale = 0.1f;
            }
        }
    }

    /// <summary>
    /// Inicia el temporizador.
    /// </summary>
    public void StartTimer()
    {
        remainingTime = timerDuration;
        isRunning = true;
    }

    [ContextMenu("ResetTime")]
    public void ResetTimer()
    {
        remainingTime = timerDuration;
        isRunning = false;
    }

    public void EndTime()
    {
       endGame.gameObject.SetActive(true);
       _continuousMoveProvider.moveSpeed = 0;
       //endGame.gameObject.transform.SetParent(gameObject.transform);
       StartCoroutine(DelayToRestar());
    }

    public void AddTime()
    {
        remainingTime += timeAdded;
    }
    
    public void ConvertirATiempo(float tiempoEnSegundos)
    {
        image.fillAmount = tiempoEnSegundos/timerDuration;
    
    }

    IEnumerator DelayToRestar()
    {
        yield return new WaitForSeconds(0.05f);
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}