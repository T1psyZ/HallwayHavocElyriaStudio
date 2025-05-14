using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InteractDialogue : MonoBehaviour
{
    [Header("Interact Dialogue UI")]
    [SerializeField] private GameObject interactDialogueCanvas;
    [SerializeField] private GameObject interactHint;
    [SerializeField] private TMP_Text interactSpeakerText;
    [SerializeField] private Button interactNextButton;
    [SerializeField] private TMP_Text interactDialogueText;
    [SerializeField] private Image interactPortraitImage;
    public GameObject interactButton;
    public GameObject joystickControl;

    [Header("Interact Dialogue Data")]
    [SerializeField] private string[] interactSpeakers;
    [SerializeField][TextArea] private string[] interactDialogueLines;
    [SerializeField] private Sprite[] interactPortraits;
    [SerializeField] private AudioClip[] interactDialogueAudio;

    [SerializeField] private float typingSpeed = 0.02f;

    private AudioSource interactAudioSource;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool dialogueActive;
    private int step;

    public VirtualJoystick virtualJoystick;
    public GameObject player;

    void Start()
    {
        joystickControl.SetActive(true);
        interactButton.SetActive(false);
        interactAudioSource = gameObject.AddComponent<AudioSource>();
        interactNextButton.onClick.AddListener(OnNextButtonClicked);

        virtualJoystick = FindAnyObjectByType<VirtualJoystick>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            DisplayDialogueStep();
            OnMobileInteractPressed();
        }
    }

    // Removed keyboard input check since mobile UI button will handle it

    public void OnMobileInteractPressed()
    {
        interactButton.SetActive(false);
        joystickControl.SetActive(false);
        virtualJoystick.ResetAnalog();
        player.GetComponent<PlayerController>().enabled = false; // Disable player movement

        if (dialogueActive && !interactDialogueCanvas.activeSelf)
        {

            DisplayDialogueStep();
        }
        else if (dialogueActive)
        {
            OnNextButtonClicked();
        }
    }

    private void OnNextButtonClicked()
    {
       
        interactAudioSource.Stop();

        if (isTyping)
        {
            CompleteTyping();
            return;
        }

        if (step >= interactSpeakers.Length)
        {
            interactDialogueCanvas.SetActive(false);
            dialogueActive = false;
            step = 0;
            joystickControl.SetActive(true); // ? Show controls again
        }
        else
        {
            DisplayDialogueStep();
        }
    }

    private void DisplayDialogueStep()
    {
        interactDialogueCanvas.SetActive(true);
        interactSpeakerText.text = interactSpeakers[step];

        if (step < interactDialogueAudio.Length && interactDialogueAudio[step] != null)
        {
            interactAudioSource.PlayOneShot(interactDialogueAudio[step]);
        }

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeWriterEffect(interactDialogueLines[step]));

        if (step < interactPortraits.Length)
        {
            interactPortraitImage.sprite = interactPortraits[step];
        }

        step++;
    }

    private IEnumerator TypeWriterEffect(string line)
    {
        isTyping = true;
        interactDialogueText.text = "";

        yield return new WaitForSeconds(0.5f);

        foreach (char letter in line)
        {
            interactDialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);

            if (!isTyping)
                yield break;
        }

        isTyping = false;
    }

    private void CompleteTyping()
    {
        if (step - 1 < interactDialogueLines.Length)
        {
            interactDialogueText.text = interactDialogueLines[step - 1];
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
        if (collision.CompareTag("Player"))
        {
            

            dialogueActive = true;
            interactHint.SetActive(true);
            interactButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueActive = false;
            interactDialogueCanvas.SetActive(false);
            interactHint.SetActive(false);
            interactButton.SetActive(false);
        }
    }
}
