using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{

    [SerializeField]
    private Button mainMenuButton;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
