using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button startGameButton, quitGameButton;

    // Start is called before the first frame update
    void Start()
    {
        startGameButton.onClick.AddListener(OnClickStartGameButton);
        quitGameButton.onClick.AddListener(OnClickQuitGameButton);
    }

    private void OnClickStartGameButton ()
    {
        SceneManager.LoadScene("Game");
    }

    private void OnClickQuitGameButton ()
    {
        Application.Quit();
    }
}
