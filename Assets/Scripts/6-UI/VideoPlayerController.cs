using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoPlayerController : MonoBehaviour
{
    [Header("Video Player Settings")]
    [SerializeField] private VideoPlayer videoPlayer; // Reference to the Video Player
    [SerializeField] private Canvas[] canvasObjects; // References to all Canvas objects

    private void Start()
    {
        // Hide all Canvas objects at the start
        ToggleCanvas(false);

        // Attach an event for when the video ends
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Update()
    {
        // Skip the video if Space is pressed
        if (UnityEngine.InputSystem.Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            EndVideo();
        }
    }

    // Event when the video ends
    private void OnVideoEnd(VideoPlayer vp)
    {
        EndVideo();
    }

    // Function to end video and restore Canvas visibility
    private void EndVideo()
    {
        // Show all Canvas objects
        ToggleCanvas(true);

        // Optionally, disable the Video Player object
        gameObject.SetActive(false);
    }

    // Toggles visibility of all Canvas objects
    private void ToggleCanvas(bool isVisible)
    {
        foreach (Canvas canvas in canvasObjects)
        {
            if (canvas != null)
            {
                canvas.gameObject.SetActive(isVisible);
            }
        }
    }
}
