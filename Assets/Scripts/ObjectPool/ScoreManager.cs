using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text Score;
    [SerializeField] private Text maxScore;
    public static ScoreManager scoreManagerInstance;

    void Awake(){
        scoreManagerInstance = this;
    }

    void Start(){
        maxScore.text = PlayerPrefs.GetInt("MaxScore",0).ToString();
    }
    void Update(){
        updateHighScore();
    }

    void updateHighScore(){
        int current_score = int.Parse(Score.text);
        int current_maxScore = PlayerPrefs.GetInt("MaxScore",0);

        if(current_score > current_maxScore){
            PlayerPrefs.SetInt("MaxScore",current_score);
            maxScore.text = current_score.ToString();
        }
    }

    public void updateScore(int score){
        int new_score = int.Parse(Score.text) + score;
        Score.text = new_score.ToString();
    }

    public int getCurrentScore(){
        return int.Parse(Score.text);
    }
}
