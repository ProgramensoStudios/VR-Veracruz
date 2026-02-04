using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    [SerializeField] private Image[] visualCounter;
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
        visualCounter[_enemiesKilled].enabled = true;
        _enemiesKilled++;
        countText.text = _enemiesKilled.ToString();
    }
}
