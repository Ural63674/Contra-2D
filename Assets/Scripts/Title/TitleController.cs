using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private AudioSource startButtonAudioSource;
    private bool isTitleSoundEnd;
    private bool isStartButtonPressed = false;

    private void Update()
    {
        if (startButtonAudioSource.isPlaying)
            isTitleSoundEnd = false;
        else
            isTitleSoundEnd = true;

        if(isStartButtonPressed == true && isTitleSoundEnd == true)
        {
            StartFirstScene();
        }
    }
    public void OnStartButtonClick()
    {
        if (isTitleSoundEnd)
            StartFirstScene();
        else
            isStartButtonPressed = true;
    }

    private void StartFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}
