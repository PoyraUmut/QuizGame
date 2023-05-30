using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Questions")]
    [SerializeField]TextMeshProUGUI questionText;
    [SerializeField]List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Button Colors")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite CorrectAnswerSprite;
    [SerializeField] Sprite WrongAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")]
    [SerializeField] Slider progressBar;
    

    public bool isComplete;
    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update(){
        timerImage.fillAmount = timer.fillFraction;
        if(timer.LoadNextQuestion){
            if(progressBar.value == progressBar.maxValue){
            isComplete = true;
            return;
        }
        
            hasAnsweredEarly = false;
            timer.LoadNextQuestion = false;
            GetNextQuestion();           
        }
        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion){
            DisplayAnswer(-1);
            ButtonState(false);
        }
    }

    void DisplayQuestion(){
        questionText.text = currentQuestion.GetQuestion();
        for(int i = 0; i < answerButtons.Length; i++){
        TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = currentQuestion.GetAnswer(i);}
    }
    void ButtonState(bool state){
        for(int i = 0; i<answerButtons.Length; i++){
        Button button = answerButtons[i].GetComponent<Button>();
        button.interactable = state;}
    }
    public void OnAnswerSelected(int index){
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        ButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: "+ scoreKeeper.CalculateScore() + "%";
    }
    void DisplayAnswer(int index){
         Image buttonImage;
        if(index == currentQuestion.GetCorrectAnswerIndex()){
            questionText.text ="Correct Answer!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = CorrectAnswerSprite;
            scoreKeeper.SetCorrectAnswers();
        }
        else{
            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Wrong Answer the correct answer was \n"+correctAnswer;
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = WrongAnswerSprite;
        }
    }

    void SetDefaultButtonSprites(){       
        for(int i = 0; i<answerButtons.Length; i++){
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }

    void GetNextQuestion(){
        if(questions.Count>0){
        ButtonState(true);
        SetDefaultButtonSprites();
        progressBar.value++;
        GetRandomQuestion();
        DisplayQuestion();
        scoreKeeper.SetQuestionsSeen();
    }
    void GetRandomQuestion(){
        int index = Random.Range(0,questions.Count);
        currentQuestion = questions[index];
        if(questions.Contains(currentQuestion)){
            questions.Remove(currentQuestion);}
        }
    }
}
