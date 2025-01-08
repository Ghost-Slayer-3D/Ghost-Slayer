using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI; // Required for RawImage
using System.IO;

public class WebGLVideoPlayer : MonoBehaviour
{
    [Header("Video Player Settings")]
    [SerializeField] private VideoPlayer videoPlayer;        // Reference to the Video Player
    [SerializeField] private RenderTexture renderTexture;   // Reference to the Render Texture
    [SerializeField] private RawImage rawImage;             // Reference to the RawImage displaying the video

    void Start()
    {
        // Make sure the VideoPlayer is set to URL source in the Inspector
        videoPlayer.source = VideoSource.Url;

        // Combine path to StreamingAssets folder
        string videoPath = Path.Combine(Application.streamingAssetsPath, "Ghost_Slayer_Intro.mp4");

        // On Windows, backslashes can cause issues in a URL, so replace them
        videoPath = videoPath.Replace("\\", "/");

        // Assign final path to the videoPlayer
        videoPlayer.url = videoPath;

        // Attach an event for when the video ends
        videoPlayer.loopPointReached += OnVideoEnd;

        // Start the video
        videoPlayer.Play();
    }

    void Update()
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

    // Function to stop and hide the video completely
    private void EndVideo()
    {
        // Stop the video player
        videoPlayer.Stop();

        // Clear the Render Texture to remove the last frame
        if (renderTexture != null)
        {
            renderTexture.Release(); // Clears the texture completely
        }

        // Disable the RawImage component to hide the video display
        if (rawImage != null)
        {
            rawImage.gameObject.SetActive(false); // Hides the RawImage
        }

        // Disable the Video Player object
        videoPlayer.gameObject.SetActive(false);

        // Destroy the Video Player object
        Destroy(gameObject);
    }
}
