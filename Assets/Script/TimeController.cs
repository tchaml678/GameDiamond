using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public Slider timeSlider;
    [SerializeField] GameObject gameOverPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        timeSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeGameOver();
    }

    void TimeGameOver()
    {
        timeSlider.value -= Time.deltaTime;
        if (timeSlider.value == 0)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }
    }
}
