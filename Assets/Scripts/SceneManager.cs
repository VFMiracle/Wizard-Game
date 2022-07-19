using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private Texture2D clickCursor, normalCursor;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) Cursor.SetCursor(clickCursor, new Vector2(32, 32), CursorMode.ForceSoftware);
        if (Input.GetMouseButtonUp(0)) Cursor.SetCursor(normalCursor, new Vector2(32, 32), CursorMode.ForceSoftware);
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    public void OpenCredits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }

    public void OpenInstructions()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Instructions");
    }

    public void ReturnMainMenu(bool resetTimeScale)
    {
        if (resetTimeScale) Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
