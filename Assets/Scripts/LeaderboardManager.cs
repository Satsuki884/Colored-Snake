using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Supabase Config")]
    [SerializeField] private string supabaseUrl = "https://rvlgqolotmwidqbyrujs.supabase.co";
    [SerializeField] private string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJ2bGdxb2xvdG13aWRxYnlydWpzIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTIzMzMxOTQsImV4cCI6MjA2NzkwOTE5NH0.1SBKuhSGyDC3-_Dl-I0pFzVqtkN5RBLw7RhMmm7E9HA";

    public IEnumerator SubmitScore(string username, int score)
    {
        username = username.ToUpper();
        string json = JsonUtility.ToJson(new ScoreData(username, score));
        string endpoint = $"{supabaseUrl}/rest/v1/scores";

        var request = new UnityWebRequest(endpoint, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        // request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);
        request.SetRequestHeader("Prefer", "resolution=merge-duplicates");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Score submitted or updated!");
        else
            Debug.LogError("SubmitScore error: " + request.error);
    }

    public IEnumerator UpdateScore(int score)
    {
        var username = PlayerPrefs.GetString("player_name", "Unknown");
        username = username.ToUpper();
        string json = JsonUtility.ToJson(new ScoreData(username, score));
        string endpoint = $"{supabaseUrl}/rest/v1/scores?username=eq.{username}";

        var request = new UnityWebRequest(endpoint, "PATCH");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Prefer", "return=representation");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Score updated!");
        else
            Debug.LogError("SubmitScore error: " + request.error);
    }



    public IEnumerator GetTopScores(int topCount, TMP_Text targetText)
    {
        string url = $"{supabaseUrl}/rest/v1/scores?select=*&order=score.desc&limit={topCount}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            ScoreData[] scores = JsonHelper.FromJson<ScoreData>(json);

            string display = "";
            for (int i = 0; i < scores.Length; i++)
            {
                display += $"{i + 1}. {scores[i].username} — {scores[i].score}\n";
            }

            targetText.text = display;
        }
        else
        {
            Debug.LogError("GetTopScores error: " + request.error);
            targetText.text = "Error loading records.";
        }
    }

    public IEnumerator GetPlayerRank(string username, int score, TMP_Text targetText)
    {
        username = username.ToUpper();
        targetText.text = "Loading...";
        string url = $"{supabaseUrl}/rest/v1/scores?select=username,score&order=score.desc";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            ScoreData[] scores = JsonHelper.FromJson<ScoreData>(json);

            int place = -1;
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i].username == username && scores[i].score == score)
                {
                    place = i + 1;
                    break;
                }
            }

            if (place > 0)
                targetText.text = $"You're in {place} position: {username} - {score} points.";
            else
                targetText.text = $"Your result is not in the top, but it is saved: {username} - {score}.";

        }
        else
        {
            Debug.LogError("GetPlayerRank error: " + request.error);
            targetText.text = "Error in determining the position in the ranking.";
        }
    }

    public IEnumerator GetCurrentPlayerRank(string username, TMP_Text targetText)
    {
        username = username.ToUpper();
        targetText.text = "Loading...";
        string url = $"{supabaseUrl}/rest/v1/scores?select=username,score&order=score.desc";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            ScoreData[] scores = JsonHelper.FromJson<ScoreData>(json);

            int place = -1;
            int foundScore = 0;

            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i].username == username)
                {
                    place = i + 1;
                    foundScore = scores[i].score;
                    break;
                }
            }

            if (place > 0)
                targetText.text = $"{place}. {username} — {foundScore}";
            else
                targetText.text = "-. - -";
        }
    }

    public IEnumerator CheckUsernameExists(string username, System.Action<bool> callback)
    {
        username = username.ToUpper();
        string url = $"{supabaseUrl}/rest/v1/scores?select=username&username=eq.{username}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("apikey", supabaseKey);
        request.SetRequestHeader("Authorization", "Bearer " + supabaseKey);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;

            if (!string.IsNullOrEmpty(json) && json != "[]")
            {
                callback(true);
            }
            else
            {
                callback(false);
            }
        }
        else
        {
            Debug.LogError("CheckUsernameExists error: " + request.error);
            callback(true); // Лучше заблокировать регистрацию при ошибке
        }
    }


}

[System.Serializable]
public class ScoreData
{
    public string username;
    public int score;

    public ScoreData(string username, int score)
    {
        this.username = username;
        this.score = score;
    }
}


