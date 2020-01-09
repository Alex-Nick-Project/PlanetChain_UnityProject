using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
//[ExecuteAlways]
public class TypingAnimation : MonoBehaviour
{
    private TextMeshProUGUI textObject;

    public bool animateOnStart = true;
    public bool blinkCursor;
    public string initialString;
    public char cursor;
    public float typeTime = 0.1f;
    public float variation = 0.2f;
    public float cursorBlinkFrequency = 2f;
    public float deleteSpeed = 0.05f;

    private StringBuilder currentString;

    public void SetCursor(char newCursor)
    {
        cursor = newCursor;
    }

    private void OnEnable()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        textObject.text = "";
        currentString = new StringBuilder();
        if (animateOnStart)
            TypeString(initialString);
    }

    public void TypeString(string text)
    {
        StopBlinking();
        StartCoroutine(AnimateTyping(text, typeTime, variation));
    }

    public void ReplaceText(string newText)
    {
        StopAllCoroutines();
        StopCoroutine("AnimateTyping");
        StartCoroutine(DeleteAndReplace(newText));
    }

    public void Clear()
    {
        StopBlinking();
        currentString = new StringBuilder();
        textObject.text = "";
    }

    public void DeleteAll()
    {
        StopBlinking();
        StartCoroutine(AnimateDelete());
    }

    public void StartBlinking()
    {
        StartCoroutine(BlinkCursorForever());
    }

    public void StopBlinking()
    {
        StopAllCoroutines();
    }

    IEnumerator AnimateTyping(string str, float typeTime, float variation)
    {
        for (int currentIndex = 0; currentIndex < str.Length; ++currentIndex)
        {
            currentString.Append(str[currentIndex]);
            textObject.text = currentString.ToString() + cursor;
            yield return new WaitForSeconds(typeTime + Random.Range(0f, variation));
        }
        if (blinkCursor)
            StartCoroutine(BlinkCursorForever());
    }

    IEnumerator BlinkCursorForever()
    {
        bool cursorState = true;
        while (true)
        {
            if (cursorState)
                textObject.text = currentString.ToString() + cursor;
            else
                textObject.text = currentString.ToString() + ' ';
            cursorState = !cursorState;
            yield return new WaitForSeconds(cursorBlinkFrequency);
        }
    }

    IEnumerator BlinkCursorForSeconds(float seconds)
    {
        StringBuilder stringBuilder = new StringBuilder(textObject.text + ' ');
        bool cursorState = true;
        while (true)
        {
            stringBuilder[stringBuilder.Length - 1] = cursorState ? cursor : ' ';
            textObject.text = stringBuilder.ToString();
            cursorState = !cursorState;
            yield return new WaitForSeconds(cursorBlinkFrequency);
        }
    }

    IEnumerator AnimateDelete()
    {
        /*
        StringBuilder stringBuilder = new StringBuilder(textObject.text);
        while (stringBuilder.Length > 1)
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder[stringBuilder.Length - 1] = cursor;
            textObject.text = stringBuilder.ToString();
            yield return new WaitForSeconds(deleteSpeed);
        }
        */
        while (currentString.Length > 0)
        {
            // stringBuilder.Remove(stringBuilder.Length - 1, 1);
            // stringBuilder[stringBuilder.Length - 1] = cursor;
            currentString.Remove(currentString.Length - 1, 1);
            textObject.text = currentString.ToString() + cursor;
            yield return new WaitForSeconds(deleteSpeed);
        }
        textObject.text = "";
        if (blinkCursor)
            StartBlinking();
    }

    IEnumerator DeleteAndReplace(string str)
    {
        while (currentString.Length > 1)
        {
            currentString.Remove(currentString.Length - 1, 1);
            textObject.text = currentString.ToString() + cursor;
            yield return new WaitForSeconds(deleteSpeed);
        }
        textObject.text = "";
        StartCoroutine(AnimateTyping(str, typeTime, variation));
    }

}
