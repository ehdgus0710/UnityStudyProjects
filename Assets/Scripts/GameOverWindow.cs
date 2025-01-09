using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverWindow : GenericWindow
{

    [SerializeField] private TextMeshProUGUI[] statText;
    [SerializeField] private TextMeshProUGUI[] scoreText;

    [SerializeField] private TextMeshProUGUI totalScoreText;

    [SerializeField] private string statName = "STAT";

    [SerializeField] private int randomRangeScore;
    [SerializeField] private int randomRangeTotalScore;

    [SerializeField] private float drawWaitTime = 1f;
    [SerializeField] private float totalScoreUpTime = 5f;

    [SerializeField] private int rowCount;

    public override void Open()
    {
        for (int i = 0; i < statText.Length; ++i)
        {
            statText[i].text = "";
            scoreText[i].text = "";
        }

        base.Open();
        StartCoroutine(CoDrawStatInfo());
    }

    public override void Close()
    {
        base.Close();
    }

    public void OnClickNext()
    {
        windowManager.Open(0);
    }

    private IEnumerator CoDrawStatInfo()
    {
        int count = statText.Length * rowCount;

        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(drawWaitTime);

            int currentIndex = i / rowCount;
            statText[currentIndex].text += $"{statName}{i}\n";
            scoreText[currentIndex].text += string.Format($"{{0:D4}}\n", Random.Range(0, randomRangeScore));
        }

        yield return new WaitForSeconds(drawWaitTime);
        StartCoroutine(CoTotalScoreUp());
    }

    private IEnumerator CoTotalScoreUp()
    {
        int totalScore = Random.Range(0, randomRangeTotalScore);
        int currentScore = 0;
        totalScoreText.text = string.Format($"{{0:D8}}", currentScore);
        float currentTime = 0f;
        while (currentScore < totalScore)
        {
            currentTime += Time.deltaTime;
            currentScore = (int)Mathf.Lerp(0, totalScore, currentTime / totalScoreUpTime);
            totalScoreText.text = string.Format($"{{0:D8}}", currentScore);
            yield return new WaitForEndOfFrame();
        }
    }
}
