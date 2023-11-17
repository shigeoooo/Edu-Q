using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuizManager : MonoBehaviour
{
#pragma warning disable 649
    //ref to the QuizGameUI script
    [SerializeField] private QuizGameUI quizGameUI;
    //ref to the scriptableobject file
    [SerializeField] private List<QuizDataScriptable> quizDataList;
    [SerializeField] private float timeInSeconds;
    
#pragma warning restore 649

    private string currentCategory = "";
    private int correctAnswerCount = 0;
    //questions data
    private List<Question> questions;
    //current question data
    private Question selectedQuetion = new Question();
    private int gameScore;
    private float currentTime;
    private QuizDataScriptable dataScriptable;
    private bool isAnswerSelected = false;
    private GameStatus gameStatus = GameStatus.NEXT;
    private bool isPaused = false;
    public GameStatus GameStatus { get { return gameStatus; } }
    [SerializeField] private Text questionNumberText;
    public List<QuizDataScriptable> QuizData { get => quizDataList; }
    private int totalQuestionsForCategory;
    public void StartGame(int categoryIndex, string category)
    {
        currentCategory = category;
        correctAnswerCount = 0;
        gameScore = 0;
        currentTime = timeInSeconds;

        // Set the reference to the QuizGameUI component
        quizGameUI = FindObjectOfType<QuizGameUI>();

        // Set the current question number text to 1 out of total questions count
    
        //set the questions data
        questions = new List<Question>();
        dataScriptable = quizDataList[categoryIndex];
        questions.AddRange(dataScriptable.questions);

        //select the question
        SelectQuestion();
        gameStatus = GameStatus.PLAYING;
        quizGameUI.GameOverPanel.SetActive(false);
        
    
    }

    /// <summary>
    /// Method used to randomly select the question form questions data
    /// </summary>
    private void SelectQuestion()
    {
        int currentQuestionNumber = dataScriptable.questions.Count - questions.Count + 1;
        int totalQuestions = dataScriptable.questions.Count;


        //get the random number
        int val = UnityEngine.Random.Range(0, questions.Count);
        //set the selectedQuetion
        selectedQuetion = questions[val];
        //send the question to quizGameUI
        quizGameUI.SetQuestion(selectedQuetion);
        questionNumberText.text = "Question " + currentQuestionNumber + " / " + totalQuestions;


        questions.RemoveAt(val);
        
    }

    private void Update()
    {
        if (gameStatus == GameStatus.PLAYING && !isPaused)
        {
            currentTime -= Time.deltaTime;
            SetTime(currentTime);
        }
    }

    void SetTime(float value)
    {
        TimeSpan time = TimeSpan.FromSeconds(currentTime);                       //set the time value
        quizGameUI.TimerText.text = time.ToString("mm':'ss");   //convert time to Time format

        if (currentTime <= 0)
        {
            //Game Over
            GameEnd();
        }
    }

    /// <summary>
    /// Method called to check the answer is correct or not
    /// </summary>
    /// <param name="selectedOption">answer string</param>
    /// <returns></returns>
    
    public void NextQuestion()
    {
        if (gameStatus == GameStatus.PLAYING)
        {
            if (questions.Count > 0)
            {
                SelectQuestion();
                isPaused = false; // Resume the game
            }
            else
            {
                GameEnd();
            }
        }
    }
    
    public bool Answer(string selectedOption)
    {
        //set default to false
        bool correct = false;
        //if selected answer is similar to the correctAns
        if (selectedQuetion.correctAns == selectedOption)
        {
            //Yes, Ans is correct
            correctAnswerCount++;
            correct = true;
            gameScore += 1;
            quizGameUI.ScoreText.text = "Score: " + gameScore + "/10";
        }
        //return the value of correct bool
        return correct;
        GameEnd();
    }

    private void GameEnd()
{
    gameStatus = GameStatus.NEXT;
    quizGameUI.GameOverPanel.SetActive(true);

    // Calculate the current score from the correctAnswerCount
    int currentScore = correctAnswerCount;

    // Update the highscore if the current score is higher
    string currentCategoryKey = currentCategory + "_Highscore";
    int currentHighScore = PlayerPrefs.GetInt(currentCategoryKey, 0);
    if (currentScore > currentHighScore)
    {
        PlayerPrefs.SetInt(currentCategoryKey, currentScore);
    }

    // Calculate the total wrong answered questions for the current category
    int totalQuestionsForCategory = dataScriptable.questions.Count;
    int totalWrongForCategory = totalQuestionsForCategory - currentScore;

    // Save the total correct and total wrong for the category in PlayerPrefs
    string totalCorrectKey = currentCategory + "_TotalCorrect";
    PlayerPrefs.SetInt(totalCorrectKey, currentScore);

    string totalWrongKey = currentCategory + "_TotalWrong";
    PlayerPrefs.SetInt(totalWrongKey, totalWrongForCategory);

    // Get the updated values from PlayerPrefs
    int updatedHighScore = PlayerPrefs.GetInt(currentCategoryKey, 0);
    int updatedTotalCorrect = PlayerPrefs.GetInt(totalCorrectKey, 0);
    int updatedTotalWrong = PlayerPrefs.GetInt(totalWrongKey, 0);

    // Call the SetProgressText method of QuizGameUI to update the text values
    quizGameUI.SetProgressText(updatedHighScore, updatedTotalCorrect, updatedTotalWrong, currentScore);

    // Other code...
}




    public void ResetGame()
    {
        gameStatus = GameStatus.NEXT;
        quizGameUI.GameOverPanel.SetActive(false); // Hide the game over panel.
        quizGameUI.ScoreText.text = "Score: 0"; // Reset the score text to zero.
        quizGameUI.TimerText.text = "00:00"; // Reset the timer text to zero.
    }

public int GetCurrentScore()
{
    return correctAnswerCount;
}
    
}

//Datastructure for storing the questions data
[System.Serializable]
public class Question
{
    public string questionInfo;
    public List<string> options;
    public string correctAns;
    public string explanation; // Use 'string' type for the explanation field.
    public TimerScript timerScript;
}


[System.Serializable]
public enum QuestionType
{
    TEXT,
}

[SerializeField]
public enum GameStatus
{
    PLAYING,
    NEXT,
}