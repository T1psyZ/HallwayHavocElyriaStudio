using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private Button nextButton;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;

    [SerializeField] private string[] speaker;
    [SerializeField][TextArea] private string[] dialogueWords;
    [SerializeField] private Sprite[] portrait;
    [SerializeField] private AudioClip[] dialogueAudio;

    private bool dialogueActivated;
    private int step;
    [SerializeField] private float typingSpeed = 0.02f;

    private AudioSource audioSource;
    private Coroutine typingCoroutine; // Store reference to the coroutine
    private bool isTyping; // Flag to check if typing is in progress

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        audioSource.Stop(); 
        if (isTyping)
        {
            // If text is still typing, complete it instantly
            CompleteTyping();
            return;
        }

        if (step >= speaker.Length)
        {
            dialogueCanvas.SetActive(false);
            dialogueActivated = false;
            step = 0;
        }
        else
        {
            dialogueCanvas.SetActive(true);
            speakerText.text = speaker[step];

            if (step < dialogueAudio.Length && dialogueAudio[step] != null)
            {
                audioSource.PlayOneShot(dialogueAudio[step]);
            }

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Stop any ongoing typing effect
            }
            typingCoroutine = StartCoroutine(TypeWriterEffect(dialogueWords[step]));

            if (step < portrait.Length)
            {
                portraitImage.sprite = portrait[step];
            }
            step += 1;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact") && dialogueActivated)
        {
            audioSource.Stop();
            if (isTyping)
            {
                // If text is still typing, complete it instantly
                CompleteTyping();
                return;
            }

            if (step >= speaker.Length)
            {
                dialogueCanvas.SetActive(false);
                dialogueActivated = false;
                step = 0;
            }
            else
            {
                dialogueCanvas.SetActive(true);
                speakerText.text = speaker[step];

                if (step < dialogueAudio.Length && dialogueAudio[step] != null)
                {
                    audioSource.PlayOneShot(dialogueAudio[step]);
                }

                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine); // Stop any ongoing typing effect
                }
                typingCoroutine = StartCoroutine(TypeWriterEffect(dialogueWords[step]));

                if (step < portrait.Length)
                {
                    portraitImage.sprite = portrait[step];
                }
                step += 1;
            }
        }
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        yield return new WaitForSeconds(0.5f);

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);

            if (!isTyping)
            {
                // If typing was interrupted, stop coroutine
                yield break;
            }
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (step - 1 < dialogueWords.Length)
        {
            dialogueText.text = dialogueWords[step - 1]; // Show full text instantly
        }
        isTyping = false;
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dialogueActivated = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        dialogueActivated = false;
        dialogueCanvas.SetActive(false);
    }
}
