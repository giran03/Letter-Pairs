using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowAllCards : MonoBehaviour
{
    public Sprite backImage;
    public List<Button> buttonList = new();
    public Sprite[] puzzles;
    public List<Sprite> gamePuzzles = new();

    GameManager gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();

        GetButtons();
        AddListener();
        AddCards();
    }

    void GetButtons()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Card");

        for (int i = 0; i < objects.Length; i++)
        {
            buttonList.Add(objects[i].GetComponent<Button>());
            buttonList[i].image.sprite = puzzles[i];
            // buttonList[i].image.sprite = backImage;
        }
    }

    void AddListener()
    {
        foreach (Button btn in buttonList)
        {
            btn.onClick.AddListener(() => PickPuzzle());
        }
    }

    void AddCards()
    {
        int looper = buttonList.Count;
        int index = 0;

        for (int i = 0; i < looper; i++)
        {
            gamePuzzles.Add(puzzles[index]);
            index++;
        }
    }

    public void PickPuzzle()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log($"Im a card and clicked! {name}!");

        int cardTapped = System.Convert.ToInt32(name);

        string cardName = gamePuzzles[cardTapped].name;

        //SFX
        SoundManager.Instance.PlaySFX(cardName);
    }
}
