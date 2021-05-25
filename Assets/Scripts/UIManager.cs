using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreTextHandle;

    [SerializeField]
    private Image _livesImageHandle;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text[] _gameOverTexts;

    void Start()
    {
        foreach(Text text in _gameOverTexts)
        {
            text.gameObject.SetActive(false);
        }
    }

    public void UpdateScoreUI(int score)
    {
        _scoreTextHandle.text = "Score: " + score.ToString();
    }

    public void UpdateLivesUI(int lives)
    {
        _livesImageHandle.sprite = _liveSprites[lives];

        if (lives == 0)
        {
            foreach(Text text in _gameOverTexts)
            {
                text.gameObject.SetActive(true);
            }
        }
    }
}
