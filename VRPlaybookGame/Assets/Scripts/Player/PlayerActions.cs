using UnityEngine;
using UnityEngine.XR;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private PoolingSystem pool;
    [SerializeField] private Transform shootPos;

    [Header("Gesture Settings")]
    [SerializeField] private float startSwingThreshold = 0.6f;
    [SerializeField] private float stopThreshold = 0.15f;
    [SerializeField] private float shootCooldown = 0.5f;

    private InputData _inputData;
    private float _lastShootTime;

    private bool isSwinging;
    private float peakVelocity;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _inputData = GetComponent<InputData>();
    }

    private void Update()
    {
        if (!_inputData._rightController
            .TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
            return;

        float speed = velocity.magnitude;

        // 1️ Detectar inicio del gesto
        if (!isSwinging && speed > startSwingThreshold)
        {
            isSwinging = true;
            peakVelocity = speed;
        }

        // 2️ Registrar pico
        if (isSwinging)
        {
            peakVelocity = Mathf.Max(peakVelocity, speed);

            // 3️ Detectar frenado fuerte
            if (speed < stopThreshold && Time.time > _lastShootTime)
            {
                Shoot();
                ResetGesture();
            }
        }
    }

    private void ResetGesture()
    {
        isSwinging = false;
        peakVelocity = 0f;
        _lastShootTime = Time.time + shootCooldown;
    }

    private void Shoot()
    {
        pool.AskForObject(shootPos);
        _audioSource.Play();
    }
}
