using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NormalTriggerDialogue : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Image portraitImage;

    [Header("Dialogue Content")]
    [SerializeField] private string[] speakers;
    [SerializeField][TextArea] private string[] lines;
    [SerializeField] private Sprite[] portraits;

    [SerializeField] private float typingSpeed = 0.02f;

    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool dialogueActive;
    private int step;

    private void Start()
    {
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(NextDialogue);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dialogueActive && collision.CompareTag("Player"))
        {
            dialogueActive = true;
            step = 0;
            ShowStep();
        }
    }

    private void NextDialogue()
    {
        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        if (step >= lines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowStep();
        }
    }

    private void ShowStep()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(true);

        if (step < speakers.Length)
            speakerText.text = speakers[step];

        if (step < portraits.Length)
            portraitImage.sprite = portraits[step];

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(lines[step]));
        step++;
    }

    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        dialogueText.text = lines[step - 1];
        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    private void EndDialogue()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        dialogueActive = false;
        step = 0;
    }
}
