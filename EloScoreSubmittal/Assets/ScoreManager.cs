using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private Dictionary<string, PlayerData> players;
    private List<SinglesMatch> singlesHistory;
    public TMP_InputField winner;
    public TMP_InputField score2;
    public TMP_InputField loser;
    public GameObject confirmButton;
    public Text confirmButtonText; 



    // Start is called before the first frame update
    void Start() {
        var playerDatas = JsonUtility.FromJson<SinglesPlayers>(File.ReadAllText(Application.streamingAssetsPath + "/Singles.json")).singlesPlayers;
        this.singlesHistory = new List<SinglesMatch>(JsonUtility.FromJson<SinglesHistory>(File.ReadAllText(Application.streamingAssetsPath + "/singlesHistory.json")).singlesHistory);

        this.players = new Dictionary<string, PlayerData>();
        foreach (var pData in playerDatas) {
            players[pData.tag] = pData;
        }
    }


    public void AddSinglesMatchToHistory() {
        if (ValidInputs()) {
            var newMatch = new SinglesMatch(winner.text, "3-" + score2.text, loser.text);
            bool newWinner = !players.ContainsKey(newMatch.victor);
            bool newDefeated = !players.ContainsKey(newMatch.defeated);

            this.confirmButtonText.text = "Confirm:\n" + (newWinner ? "(NEW) " : "") + newMatch.ToString() + (newDefeated ? " (NEW)" : "");
            this.confirmButton.SetActive(true);
        }
    }


    public void ConfirmAddToSinglesMatch() {


        this.confirmButton.SetActive(false);
    }

    public bool ValidInputs() {
        return winner.text != "Winner" && score2.text != "Score2" && loser.text != "Loser";
    }

    //private void WriteData() {
    //    File.WriteAllText(this.filePath, JsonUtility.ToJson(this.dataToCreate, true));
    //}
}

[System.Serializable]
public struct PlayerData {

    public string tag;
    public int wins;
    public int totalGames;
    public int score;

    public PlayerData(string tag, int wins, int totalGames, int score) {
        this.tag = tag;
        this.wins = wins;
        this.totalGames = totalGames;
        this.score = score;
    }
}

[System.Serializable]
public struct SinglesPlayers {
    public PlayerData[] singlesPlayers;

    public SinglesPlayers(PlayerData[] singlesPlayers) {
        this.singlesPlayers = singlesPlayers;
    }

    public SinglesPlayers(List<PlayerData> singlesPlayers) {
        this.singlesPlayers = singlesPlayers.ToArray();
    }
}


[System.Serializable]
public struct SinglesMatch {

    public string victor;
    public int score1;
    public int score2;
    public string defeated;

    public SinglesMatch(string victor, string score, string defeated) {
        this.victor = victor;
        this.score1 = 3;
        this.score2 = int.Parse("" + score[2]);
        this.defeated = defeated;
    }

    public override string ToString() {
        return victor + " won " + score1 + " - " + score2 + " against " + defeated;
    }
}

[System.Serializable]
public struct SinglesHistory {
    public SinglesMatch[] singlesHistory;

    public SinglesHistory(SinglesMatch[] singlesHistory) {
        this.singlesHistory = singlesHistory;
    }

    public SinglesHistory(List<SinglesMatch> singlesHistory) {
        this.singlesHistory = singlesHistory.ToArray();
    }
}