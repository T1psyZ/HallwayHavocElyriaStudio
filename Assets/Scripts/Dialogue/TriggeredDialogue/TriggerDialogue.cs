using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TriggerDialogue : MonoBehaviour
{
    [Header("Trigger Dialogue UI")]
    [SerializeField] private GameObject triggerDialogueCanvas;
    [SerializeField] private TMP_Text triggerSpeakerText;
    [SerializeField] private Button triggerNextButton;
    [SerializeField] private TMP_Text triggerDialogueText;
    [SerializeField] private Image triggerPortraitImage;

    [Header("Trigger Dialogue Data")]
    [SerializeField] private string[] triggerSpeakers;
    [SerializeField][TextArea] private string[] triggerDialogueLines;
    [SerializeField] private Sprite[] triggerPortraits;
    [SerializeField] private AudioClip[] triggerDialogueAudio;

    [SerializeField] private float typingSpeed = 0.02f;

    private AudioSource triggerAudioSource;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool dialogueActive;
    private int step;

    void Start()
    {
        triggerAudioSource = gameObject.AddComponent<AudioSource>();
        triggerNextButton.onClick.AddListener(OnNextButtonClicked);
    }

    private void OnNextButtonClicked()
    {
        triggerAudioSource.Stop();

        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        if (step >= triggerSpeakers.Length)
        {
            EndDialogue();
        }
        else
        {
            DisplayDialogueStep();
        }
    }

    public void DisplayDialogueStep()
    {
        triggerDialogueCanvas.SetActive(true);
        triggerSpeakerText.text = triggerSpeakers[step];

        if (step < triggerDialogueAudio.Length && triggerDialogueAudio[step] != null)
        {
            triggerAudioSource.PlayOneShot(triggerDialogueAudio[step]);
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeWriterEffect(triggerDialogueLines[step]));

        if (step < triggerPortraits.Length)
        {
            triggerPortraitImage.sprite = triggerPortraits[step];
        }

        step++;
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        isTyping = true;
        triggerDialogueText.text = "";

        yield return new WaitForSeconds(0.5f);

        foreach (char letter in line)
        {
            triggerDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);

            if (!isTyping)
                yield break;
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (step - 1 < triggerDialogueLines.Length)
        {
            triggerDialogueText.text = triggerDialogueLines[step - 1];
        }

        isTyping = false;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (dialogueActive) return;

    //    if (collision.CompareTag("Player"))
    //    {
    //        dialogueActive = true;
    //        step = 0;
    //        DisplayDialogueStep();
    //    }
    //}

    private void EndDialogue()
    {
        triggerDialogueCanvas.SetActive(false);
        dialogueActive = false;
        step = 0;
        //Destroy(gameObject); // Optional: remove this if you want the object to stay
    }
}
