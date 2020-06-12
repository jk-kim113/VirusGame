using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUICanvasController : MonoBehaviour
{
    public void NewGameButton()
    {
        IngameManager.Instance.StartNewGame();
    }
}
