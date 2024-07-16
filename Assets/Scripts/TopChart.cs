using System.Collections;
using System.Collections.Generic;
using RandomNameAndCountry.Scripts;
using UnityEngine;

public class TopChart : MonoBehaviour
{
    public List<ChartItem> _items = new List<ChartItem>();

    public int minRandom, maxRandom;
    public List<int> Scores = new List<int>();
    void Start()
    {
        int playerScore = PlayerPrefs.GetInt("Viewers", 100);
        int countItems = _items.Count;
        for (int i = 0; i < Scores.Count-1; i++)
        {
            Scores[i] = Scores[i]+Random.Range(minRandom, maxRandom);
        }

        for (int i = 0; i < _items.Count; i++)
        {
            if (playerScore > Scores[i])
            {
                if (i == _items.Count - 1)
                {
                    int count = i + Random.Range(minRandom, maxRandom);
                    _items[i].numberText.text = "#" + count.ToString();
                }
                _items[i].nameText.text = "YOU";
                _items[i].scoreText.text = playerScore.ToString();
            }
            else
            {
                _items[i].nameText.text = RandomNameAndCountryPicker.Instance.GetRandomPlayerInfo().playerName;
                _items[i].scoreText.text = Scores[i].ToString();
            }
        }
    }
}
