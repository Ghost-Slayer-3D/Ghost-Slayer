using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

/**
 * Handles the scrolling intro text and skipping functionality.
 */
public class IntroManager : MonoBehaviour
{
    [Header("Intro Settings")]
    [SerializeField] private TextMeshProUGUI scrollingText; // Reference to the scrolling text
    [SerializeField] private RectTransform textTransform; // Transform of the scrolling text
    [SerializeField] private float scrollSpeed = 50f; // Speed of the scrolling text
    [SerializeField] private float endDelay = 3f; // Time to wait after scrolling finishes

    [Header("Input Action")]
    [Tooltip("Input action for skipping the intro.")]
    [SerializeField] private InputAction skipAction;

    private bool isScrolling = true;

    private void OnEnable()
    {
        skipAction.Enable(); // Enable the input action
    }

    private void OnDisable()
    {
        skipAction.Disable(); // Disable the input action
    }

    private void Start()
    {
        // Lock the game by pausing time
        Time.timeScale = 0f;

        // Ensure the text starts below the screen
        if (textTransform != null)
        {
            textTransform.anchoredPosition = new Vector2(0, -Screen.height + Screen.height / 10);
        }
    }

    private void Update()
    {
        // Scroll the text if it's still active
        if (isScrolling && textTransform != null)
        {
            textTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.unscaledDeltaTime;

            // Check if the text has fully scrolled
            if (textTransform.anchoredPosition.y >= Screen.height * 2)
            {
                isScrolling = false;
                StartCoroutine(EndIntro());
            }
        }

        // Check for skip input
        if (skipAction.triggered)
        {
            SkipIntro();
        }
    }

    private void SkipIntro()
    {
        // End the intro immediately
        isScrolling = false;
        StopAllCoroutines(); // Stop any running coroutines
        EndIntroImmediately();
    }

    private IEnumerator EndIntro()
    {
        // Wait for a moment after scrolling
        yield return new WaitForSecondsRealtime(endDelay);

        EndIntroImmediately();
    }

    private void EndIntroImmediately()
    {
        // Hide the intro canvas
        gameObject.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
    }
}
