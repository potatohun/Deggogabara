using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyDown : MonoBehaviour
{
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSelect);
            SceneManager.LoadScene(1);
        }
    }
}
