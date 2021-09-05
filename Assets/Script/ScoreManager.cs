using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] Text scoreText;
    public int myScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + myScore;
        
    }

    public void AddScore(int score)
    {
        myScore += score;
    }
    public int Score()
    {
        return myScore;
    }
}
