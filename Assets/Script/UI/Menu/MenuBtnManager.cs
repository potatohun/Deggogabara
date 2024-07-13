using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Menu
{
    Play = 0,
    Option = 1,
    Exit = 2
}
public class MenuBtnManager : MonoBehaviour
{

    public Menu menuState;

    [SerializeField]
    Color baseColor = new Color(204, 204, 204, 1);
    [SerializeField]
    Color pointColor = Color.white;

    public List<TextMeshProUGUI> textBtn;

    private void Awake()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            textBtn.Add(GetComponentsInChildren<TextMeshProUGUI>()[i]);
        }
    }

    private void Start()
    {
        textBtn[0].color = pointColor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
            switch (menuState)
            {
                case Menu.Play:
                    menuState = Menu.Option;
                    return;
                case Menu.Option:
                    menuState = Menu.Exit;
                    return;
                case Menu.Exit:
                    menuState = Menu.Play;
                    return;
            }
        }
        if( Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
            switch (menuState)
            {
                case Menu.Play:
                    menuState = Menu.Exit;
                    return;
                case Menu.Option:
                    menuState = Menu.Play;
                    return;
                case Menu.Exit:
                    menuState = Menu.Option;
                    return;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSelect);
            switch (menuState)
            {
                case Menu.Play:
                    SceneManager.LoadScene(2);
                    return;
                case Menu.Option:
                    
                    return;
                case Menu.Exit:
                    Application.Quit();
                    return;
            }
        }


        switch (menuState) 
        {
            case Menu.Play:
                textColorReset(textBtn[0]);
                return;
            case Menu.Option:
                textColorReset(textBtn[1]);
                return; 
            case Menu.Exit:
                textColorReset(textBtn[2]);
                return;
        }

        
    }

    public void textColorReset(TextMeshProUGUI changeText)
    {
        foreach(var t in textBtn)
        {
            if(changeText == t)
                changeText.color = pointColor;
            else
                t.color = baseColor;
        }
        
    }
}
