using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text staminaText;
    public Animator HealthTextAnim;
    public Animator StaminaTextAnim;

    public GameObject respawnUi;
    public Button respawnButton;
    public Button quitButton;

    private int currentHealth;
    private int currentStamina;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerController playerController;
    private bool playerDied = false;

    public bool IsPlayerDied()
    {
        return playerDied;
    }

    void Start()
    {
        currentHealth = Stats_Manager.instance.maxHealth;
        currentStamina = 0;

        healthText.text = $"HP: {currentHealth} / {Stats_Manager.instance.maxHealth}";
        staminaText.text = $"ST: {currentStamina} / {Stats_Manager.instance.maxStamina}";

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        healthText.text = $"HP: {currentHealth} / {Stats_Manager.instance.maxHealth}";
        staminaText.text = $"ST: {currentStamina} / {Stats_Manager.instance.maxStamina}";
    }
    public void ChangeHealth(int amount, Vector2 knockbackSource)
    {
        currentHealth += amount;
        HealthTextAnim.Play("Health");
        healthText.text = $"HP: {currentHealth} / {Stats_Manager.instance.maxHealth}";

        if (currentHealth <= 0 && !playerDied)
        {
            playerDied = true;
            Die();
        }
        else
        {
            StartCoroutine(ApplyKnockback(knockbackSource));
        }
    }

    public async void GainStamina()
    {
        staminaText.text = $"ST: {currentStamina} / {Stats_Manager.instance.maxStamina}";
        await Task.Delay(1000);
        currentStamina += Stats_Manager.instance.gainStamina;
        if (currentStamina > Stats_Manager.instance.maxStamina)
        {
            currentStamina = Stats_Manager.instance.maxStamina;
        }
    }

    public async void UseStamina()
    {
        StaminaTextAnim.Play("Stamina");
        staminaText.text = $"ST: {currentStamina} / {Stats_Manager.instance.maxStamina}";
        await Task.Delay(500);
        currentStamina -= Stats_Manager.instance.useStamina;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        PlayerSetNotActive();
    }

    private void PlayerSetNotActive()
    {
        gameObject.SetActive(false);
        respawnUi.SetActive(true);
        respawnButton.onClick.AddListener(OnRespawnButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnRespawnButtonClicked()
    {
        playerDied = false;
        gameObject.SetActive(true);
        animator.SetBool("isDead", false);
        playerController.enabled = true;
        transform.position = Vector2.zero;
        respawnUi.SetActive(false);
        StartCoroutine(RestartSceneAfterDelay(0f));
    }

    private void OnQuitButtonClicked()
    {
        playerDied = false;
        gameObject.SetActive(true);
        animator.SetBool("isDead", false);
        playerController.enabled = true;
        transform.position = Vector2.zero;
        respawnUi.SetActive(false);
        StartCoroutine(QuitToMainMenu(0f));
    }

    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator QuitToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator ApplyKnockback(Vector2 knockbackSource)
    {
        rb.velocity = knockbackSource;
        playerController.enabled = false;
        yield return new WaitForSeconds(Stats_Manager.instance.knockbackDuration);
        playerController.enabled = true;
    }
}
