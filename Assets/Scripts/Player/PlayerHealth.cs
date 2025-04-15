    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour //tiete
{
    [Header("Health")] 
    public int currentHealth;
    public int maxHealth;
    public Animator HealthTextAnim;
    public TMP_Text healthText;
    [Header("Stamina")]
    public int currentStamina;
    public int gainStamina;
    public int useStamina;
    public int maxStamina;
    public Animator StaminaTextAnim;
    public TMP_Text staminaText;
    [Header("Knockback")]
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    [Header("Dead")]
    public GameObject respawnUi;
    public Button respawnButton;
    public Button quitButton;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerController playerController;
    private bool isKnockedBack = false;
    private bool playerDied = false;

    public bool IsPlayerDied()
    {
        return playerDied;
    }
    public void Start()
    {
        currentHealth = maxHealth;
        currentStamina = 0;
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;
        staminaText.text = "ST: " + currentStamina + " / " + maxStamina;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
    }
    public void ChangeHealth(int amount, Vector2 knockbackSource)
    {
        currentHealth += amount ;
        HealthTextAnim.Play("Health");
        healthText.text = "HP: " + currentHealth + " / " + maxHealth;

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
        staminaText.text = "ST: " + currentStamina + " / " + maxStamina;
        await Task.Delay(1000);
        currentStamina += gainStamina;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public async void UseStamina()
    {
        StaminaTextAnim.Play("Stamina");
        staminaText.text = "ST: " + currentStamina + " / " + maxStamina;
        await Task.Delay(500);
        currentStamina -= useStamina;
        if (currentStamina < 0)
        {
            currentStamina = 0;
        }
    }
    private void Die()
    {
        animator.SetBool("isDead", true);
    }
    private void OnRespawnButtonClicked()
    {
        playerDied = false;
        gameObject.SetActive(true);
        animator.SetBool("isDead", false);
        playerController.enabled = true;
        gameObject.transform.position = new Vector2(0, 0); // Reset position    
        respawnUi.SetActive(false); // Hide respawn UI

        StartCoroutine(RestartSceneAfterDelay(1f));
    }
    private void OnQuitButtonClicked()
    {
        playerDied = false;
        gameObject.SetActive(true);
        animator.SetBool("isDead", false);
        playerController.enabled = true;
        gameObject.transform.position = new Vector2(0, 0); // Reset position    
        respawnUi.SetActive(false); // Hide respawn UI
        StartCoroutine(QuitToMainMenu(0f));
    }
    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart the scene
    }
    private IEnumerator QuitToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the delay
        SceneManager.LoadScene("MainMenu"); // Restart the scene
    }
    private void PlayerSetNotActive()
    {
        gameObject.SetActive(false);
        respawnUi.SetActive(true);
        respawnButton.onClick.AddListener(OnRespawnButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
    }
    private IEnumerator ApplyKnockback(Vector2 knockbackForce)
    {
        rb.velocity = knockbackForce; // Apply knockback force
        playerController.enabled = false; // Disable movement temporarily

        yield return new WaitForSeconds(0.2f); // Knockback duration

        playerController.enabled = true; // Re-enable movement
    }
}
