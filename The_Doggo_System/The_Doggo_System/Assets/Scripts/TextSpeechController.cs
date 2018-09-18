using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSpeechController : MonoBehaviour {

    public Text outputTextBox;
    public int typeSpeed; //Lower is faster
    public string[] inputText;

    private int m_maxTypeSpeed;
    private int m_currentLine;
    private int m_currentLetter;
    private bool m_isTextDialogueComplete;
    private bool m_isWaitingForTextLine;

	// Use this for initialization
	void Start () {
        m_currentLine = 0;
        m_isTextDialogueComplete = false;
        m_maxTypeSpeed = typeSpeed;
        m_isWaitingForTextLine = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!m_isTextDialogueComplete)
        {
            if (!m_isWaitingForTextLine)
            {
                if (typeSpeed <= 0)
                {
                    typeSpeed = m_maxTypeSpeed;
                    if (m_currentLine < inputText.Length && m_currentLetter < inputText[m_currentLine].Length)
                    {
                        outputTextBox.text += inputText[m_currentLine][m_currentLetter];
                        m_currentLetter++;
                    }
                    else if (m_currentLine < inputText.Length)
                    {
                        m_currentLine++;
                        m_currentLetter = 0;
                        m_isWaitingForTextLine = true;
                    }
                    else
                    {
                        outputTextBox.text = "";
                        m_isTextDialogueComplete = true;
                    }
                }
                else
                {
                    typeSpeed--;
                }
            }
        }

	}

    public void GoToNextLine()
    {
        if(!m_isWaitingForTextLine)
        {
            outputTextBox.text = inputText[m_currentLine];
            m_currentLetter = inputText[m_currentLine].Length;
        }
        else
        {
            m_isWaitingForTextLine = false;
            outputTextBox.text = "";
        }
    }
}
