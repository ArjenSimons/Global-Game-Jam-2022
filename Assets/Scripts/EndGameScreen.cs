using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private Button menuButton;

    private void Start()
    {
        menuButton.onClick.AddListener(OnClickMenuButton);
    }

    private void OnClickMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
