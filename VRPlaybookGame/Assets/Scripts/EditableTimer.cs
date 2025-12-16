using UnityEngine;
using TMPro;
using System;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;

public class EditableTimer : MonoBehaviour
{
    [Header("Configuraci�n del Timer")]
    [Tooltip("Duracion del temporizador en segundos.")]
    [SerializeField] private float timerDuration = 180f;

    [Header("Eventos del Timer")]
    [Tooltip("Evento que se dispara cuando el temporizador termina.")]
    public static Action onTimerEnd;

    [SerializeField] private Canvas endGame;

    private float remainingTime;
    private bool isRunning = false;

    [SerializeField] private TextMeshProUGUI tmp;
    [SerializeField] private ContinuousMoveProvider _continuousMoveProvider;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            remainingTime -= Time.deltaTime;
            var remTime = ConvertirATiempo(remainingTime);
            tmp.text = remTime.ToString();

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
    }
    
    public string ConvertirATiempo(float tiempoEnSegundos)
    {
        int minutos = Mathf.FloorToInt(tiempoEnSegundos / 60f);
        int segundos = Mathf.FloorToInt(tiempoEnSegundos % 60f);
    
        return string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}