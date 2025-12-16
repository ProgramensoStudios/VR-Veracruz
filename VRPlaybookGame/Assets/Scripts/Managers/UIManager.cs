using TMPro;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    private int _enemiesKilled = 0;
    [SerializeField] private TMP_Text countText;

    private void OnEnable()
    {
        Enemy.OnDeath += AddCounter;
    }

    private void OnDisable()
    {
        Enemy.OnDeath -= AddCounter;
    }

    private void AddCounter()
    {
        _enemiesKilled++;
        countText.text = $"Kills: {_enemiesKilled}";
    }
}
