using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizGameUI : MonoBehaviour
{
#pragma warning disable 649
    [SerializeField] private QuizManager quizManager;               //ref to the QuizManager script
    [SerializeField] private CategoryBtnScript categoryBtnPrefab;
    [SerializeField] private GameObject scrollHolder;
    [SerializeField] private Text scoreText, timerText;
    [SerializeField] private GameObject gameOverPanel, mainMenu, gamePanel, chooseDifficulty, quizPanel;
    [SerializeField] private Color correctCol, wrongCol, normalCol; //color of buttons
    [SerializeField] private Text questionInfoText;                 //text to show question
    [SerializeField] private List<Button> options;                  //options button reference
    [SerializeField] private Text progressText; // Reference to the text component of the progress button
    [SerializeField] private Text highscoreText; // Reference to the text component for highscore
    [SerializeField] private Text correctText; // Reference to the text component for correct
    [SerializeField] private Text wrongText; // Reference to the text component for wrong
    [SerializeField] private Text currentScore; // Reference to the text component for wrong
    [SerializeField] private QuizGameUI quizGameUI; // Reference to the text component for wrong
    [SerializeField] private TimerScript timerScript;
    [SerializeField] private Text questionNumberText;


    

    public void SetProgressText(int highScore, int totalCorrect, int totalWrong, int currentScore)
    {
        // Update the text values for highscore, correct, wrong, and current score
        highscoreText.text = "" + highScore.ToString() + "/10";
        correctText.text = "" + totalCorrect.ToString();
        wrongText.text = "" + totalWrong.ToString();

        // Update the progress button's text with the current score
        progressText.text = "" + currentScore.ToString() + "/10";
    }

    
    
    
    

#pragma warning restore 649

    private Question question;          //store current question data
    private bool answered = false;      //bool to keep track if answered or not
    private bool isTimerRunning = true;
    private bool explanationShown = false; // Add this field to track if the explanation is shown.

    public Text TimerText { get => timerText; }                     //getter
    public Text ScoreText { get => scoreText; }                     //getter
    public GameObject GameOverPanel { get => gameOverPanel; }                     //getter
    public Text ProgressText { get => progressText; } // Add a public property to access the progressText variable
    private bool isPaused = false;
    private GameStatus gameStatus = GameStatus.NEXT;
    

    
    private void Start()
    {
        //add the listner to all the buttons
        for (int i = 0; i < options.Count; i++)
        {
            Button localBtn = options[i];
            localBtn.onClick.AddListener(() => OnClick(localBtn));
        }

        CreateCategoryButtons();
    
    }
    /// <summary>
    /// Method which populate the question on the screen
    /// </summary>
    /// <param name="question"></param>

    public void OnNextButtonClicked()
    {
        if (gameStatus == GameStatus.PLAYING)
        {   
            isPaused = true; // Pause the game after clicking an option
        }
    }

    public GameObject explanationPanel; // Reference to the game object representing the explanation panel in the Inspector.
    public Text explanation; // Add this field to store the explanation for the question.
    public void SetQuestion(Question question)
    {
        // Set the question
        this.question = question;

        questionInfoText.text = question.questionInfo; // Set the question text

        // Shuffle the list of options
        List<string> ansOptions = ShuffleList.ShuffleListItems<string>(question.options);

        // Assign options to respective option buttons
        for (int i = 0; i < options.Count; i++)
        {
            // Set the child text
            options[i].GetComponentInChildren<Text>().text = ansOptions[i];
            options[i].name = ansOptions[i]; // Set the name of the button
            options[i].image.color = normalCol; // Set color of button to normal
        }

        answered = false;

        // Show the explanation for the current question
        explanation.text = question.explanation;    
        explanation.gameObject.SetActive(false);
    }


    /// <summary>
    /// Method assigned to the buttons
    /// </summary>
    /// <param name="btn">ref to the button object</param>
    
    void OnClick(Button btn)
    {
        if (quizManager.GameStatus == GameStatus.PLAYING && !answered && !isPaused)
        {
            answered = true;
            bool val = quizManager.Answer(btn.name);

            if (val)
            {
                StartCoroutine(BlinkImg(btn.image));
                explanation.text = question.explanation;
                explanation.gameObject.SetActive(true);
                explanationShown = true; // Set the explanationShown flag to true when showing the explanation
            

 
            }
            else
            {
                btn.image.color = wrongCol;
                explanation.text = question.explanation;
                explanation.gameObject.SetActive(true);
                explanationShown = true; // Set the explanationShown flag to true when showing the explanation
            
            }
            
            timerScript.PauseTimer(); // Pause the timer after clicking an option

        }
    }

    // Stop the timer and resume it after a specified delay
    private IEnumerator StopTimerAndResumeAfterDelay(float delay)
    {
        isTimerRunning = false;
        yield return new WaitForSeconds(delay);
        isTimerRunning = true;
    }






    /// <summary>
    /// Method to create Category Buttons dynamically
    /// </summary>
    void CreateCategoryButtons()
    {
        //we loop through all the available catgories in our QuizManager
        for (int i = 0; i < quizManager.QuizData.Count; i++)
        {
            //Create new CategoryBtn
            CategoryBtnScript categoryBtn = Instantiate(categoryBtnPrefab, scrollHolder.transform);
            //Set the button default values
            categoryBtn.SetButton(quizManager.QuizData[i].categoryName, quizManager.QuizData[i].questions.Count);
            int index = i;
            //Add listner to button which calls CategoryBtn method
            categoryBtn.Btn.onClick.AddListener(() => CategoryBtn(index, quizManager.QuizData[index].categoryName));
        }
    }

    // Method called by Category Button
    private void CategoryBtn(int index, string category)
    {
        quizManager.ResetGame(); // Reset the game before starting a new game.
        quizManager.StartGame(index, category); // Start the game
        mainMenu.SetActive(false); // Deactivate mainMenu
        gamePanel.SetActive(true); // Activate game panel

       // Get the current score from the QuizManager using the quizManager reference
        int currentScore = quizManager.GetCurrentScore();

        // Update the progress button's text with the highscore, total correct, total wrong, and current score
        int highScore = PlayerPrefs.GetInt(category + "_Highscore", 0);
        int totalCorrect = PlayerPrefs.GetInt(category + "_TotalCorrect", 0);
        int totalWrong = PlayerPrefs.GetInt(category + "_TotalWrong", 0);

        // Call SetProgressText with the correct arguments
        quizGameUI.SetProgressText(highScore, totalCorrect, totalWrong, currentScore);

    }



    //this give blink effect [if needed use or dont use]
    IEnumerator BlinkImg(Image img)
    {
        for (int i = 0; i < 2; i++)
        {
            
            img.color = correctCol;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public int GetCurrentScore()
{
    return quizManager.GetCurrentScore();
}


}