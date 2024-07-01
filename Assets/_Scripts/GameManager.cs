using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Button> buttonList = new();
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new();

    [SerializeField] Sprite frontImage;
    [SerializeField] Sprite backImage;

    [SerializeField] int singleCardMatchScore = 5;

    [Header("Timer Config")]
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject hud_label_timerHasRunOut;
    [SerializeField] TMP_Text hud_label_timerText;
    public float timeRemaining = 10;
    public bool timerIsRunning = true;

    [Header("Scoring Config")]
    [SerializeField] GameObject[] starsObects;
    [SerializeField] TMP_Text hud_label_scoreText;
    [SerializeField] TMP_Text hud_winPopUp_label_scoreText;
    int currentScore;
    int maxScoreRating;

    bool firstGuess, secondGuess;

    int countCorrectGuesses;
    int gameGuesses;

    [HideInInspector] public int firstGuessIndex, secondGuessIndex;

    string firstGuessPuzzle, secondGuessPuzzle;

    //win
    [SerializeField] GameObject display_winPopUp;
    private void Start()
    {
        GetButtons();
        AddListener();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        maxScoreRating = gamePuzzles.Count * singleCardMatchScore;
        currentScore = 0;
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                //score text
                hud_label_scoreText.SetText($"Score: {currentScore}");
            }
            else
            {
                Debug.Log("Time has run out!");
                gamePanel.SetActive(false);
                timeRemaining = 0;

                hud_label_timerHasRunOut.SetActive(true);
                display_winPopUp.SetActive(true);

                CheckScore();

                timerIsRunning = false;
            }
        }

        // FOR DEMO ONLY
        if (Input.GetKeyDown(KeyCode.K))
            CheckScore();
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        hud_label_timerText.SetText(string.Format("Timer: {0:00}:{1:00}", minutes, seconds));
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Card");

        for (int i = 0; i < objects.Length; i++)
        {
            buttonList.Add(objects[i].GetComponent<Button>());
            buttonList[i].image.sprite = backImage;
        }
    }

    void AddGamePuzzles()
    {
        int looper = buttonList.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            if (index == looper / 2)
                index = 0;

            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    void AddListener()
    {
        foreach (Button btn in buttonList)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }

    public void PickPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log($"Im a card and clicked! {name}!");

        if (!firstGuess)
        {
            firstGuess = true;
            firstGuessIndex = System.Convert.ToInt32(name);

            firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

            buttonList[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];
            ColorBlock colors = buttonList[firstGuessIndex].colors;
            colors.disabledColor = colors.normalColor;
            buttonList[firstGuessIndex].colors = colors;
            buttonList[firstGuessIndex].interactable = false;

            //SFX
            SoundManager.Instance.PlaySFX("Card Flip");
            SoundManager.Instance.PlaySFX(firstGuessPuzzle);

        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = System.Convert.ToInt32(name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            buttonList[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            if (firstGuessPuzzle == secondGuessPuzzle)
                Debug.Log($"Puzzle Match");
            else
                Debug.Log($"Puzzle doesn't Match");

            //SFX
            SoundManager.Instance.PlaySFX("Card Flip");
            SoundManager.Instance.PlaySFX(secondGuessPuzzle);

            StartCoroutine(CheckPuzzleMatch());
        }
    }

    IEnumerator CheckPuzzleMatch()
    {
        // if match
        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            // scoring
            currentScore += singleCardMatchScore * 2;

            yield return new WaitForSeconds(1.5f);

            buttonList[firstGuessIndex].interactable = false;
            buttonList[secondGuessIndex].interactable = false;


            buttonList[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            buttonList[secondGuessIndex].image.color = new Color(0, 0, 0, 0); ;

            CheckGameFinish();
        }
        else
        {
            yield return new WaitForSeconds(1f);

            buttonList[firstGuessIndex].interactable = true;

            buttonList[firstGuessIndex].image.sprite = backImage;
            buttonList[secondGuessIndex].image.sprite = backImage;
        }
        firstGuess = secondGuess = false;
    }

    void CheckGameFinish()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            CheckScore();
            timerIsRunning = false;
        }
    }

    void CheckScore()
    {
        //check star rating
        if (currentScore >= maxScoreRating)
        {
            Debug.Log($"3 STAR RATING!");
            EnableStars(3);
        }
        else if (currentScore < maxScoreRating && !(currentScore < maxScoreRating / 2))
        {
            Debug.Log($"2 STAR RATING!");
            EnableStars(2);
        }
        else if (countCorrectGuesses == 1)
        {
            Debug.Log($"1 STAR RATING!");
            EnableStars(1);
        }
        else
            Debug.Log($"No score!");

        Debug.Log($"Score of {currentScore} over {maxScoreRating}");

        //score
        hud_winPopUp_label_scoreText.SetText($"{currentScore}");

        // win pop-up
        Debug.Log($"You Win!");
        display_winPopUp.SetActive(true);

        // SFX
        SoundManager.Instance.PlaySFX("Level Clear");
    }

    void EnableStars(int starCount)
    {
        for (int i = 0; i < starCount; i++)
            starsObects[i].SetActive(true);
    }

    void Shuffle(List<Sprite> spritesList)
    {
        for (int i = 0; i < spritesList.Count; i++)
        {
            Sprite sprite = spritesList[i];
            int randomIndex = Random.Range(i, spritesList.Count);
            spritesList[i] = spritesList[randomIndex];
            spritesList[randomIndex] = sprite;
        }
    }
}