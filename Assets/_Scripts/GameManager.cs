using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Button> buttonList = new();
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new();

    [SerializeField] Sprite frontImage;
    [SerializeField] Sprite backImage;

    bool firstGuess, secondGuess;

    int countGuesses;
    int countCorrectGuesses;
    int gameGuesses;

    int firstGuessIndex, secondGuessIndex;

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
            {
                index = 0;
            }

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



        }
        else if (!secondGuess)
        {
            secondGuess = true;
            secondGuessIndex = System.Convert.ToInt32(name);

            secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

            buttonList[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

            if (firstGuessPuzzle == secondGuessPuzzle)
            {
                Debug.Log($"Puzzle Match");
            }
            else
            {
                Debug.Log($"Puzzle don't Match");
            }

            StartCoroutine(CheckPuzzleMatch());
        }
    }

    IEnumerator CheckPuzzleMatch()
    {
        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(1f);
            
            buttonList[firstGuessIndex].interactable = false;
            buttonList[secondGuessIndex].interactable = false;


            buttonList[firstGuessIndex].image.color = new Color(0, 0, 0, 0);
            buttonList[secondGuessIndex].image.color = new Color(0, 0, 0, 0);;

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
            Debug.Log($"You Win!");
            Debug.Log($"It took you {countCorrectGuesses} to finish the game!");
            display_winPopUp.SetActive(true);
        }
    }

    public void Button_NextButton()
    {
        Debug.Log($"Next Level");
    }

    public void Button_RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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