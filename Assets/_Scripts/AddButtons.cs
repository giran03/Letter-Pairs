using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddButtons : MonoBehaviour
{
    [Header("Configs")]
    
    [Tooltip ("Must be an even number")]
    [SerializeField] int cardCount; 
    [SerializeField] Transform puzzleField;
    [SerializeField] GameObject button;

    private void Awake()
    {
        for (int i = 0; i < cardCount; i++)
        {
            GameObject spawned_button = Instantiate(button);
            spawned_button.name = "" + i;
            spawned_button.transform.SetParent(puzzleField, false);
        }
    }
}
