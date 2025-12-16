using UnityEngine;
using UnityEngine.XR;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private PoolingSystem pool;
    [SerializeField] private Transform shootPos;
    [SerializeField] private float shakeThreshold = 3.5f;
    [SerializeField] private float shootCooldown = 0.5f;

    private InputData _inputData;
    private float _lastShootTime;

    private void Update()
    {
        if (!_inputData._rightController.TryGetFeatureValue(
                CommonUsages.deviceAngularAcceleration, out Vector3 angularAcceleration)) return;
        float shakeStrength = angularAcceleration.magnitude;

        if (shakeStrength > shakeThreshold && Time.time > _lastShootTime)
        {
            Shoot();
            _lastShootTime = Time.time + shootCooldown;
        }
    }

    private void Shoot()
    {
        pool.AskForObject(shootPos);
    }
}