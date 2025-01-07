using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class WebGLVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        // Make sure the VideoPlayer is set to URL source in the Inspector
        videoPlayer.source = VideoSource.Url;

        // Combine path to StreamingAssets folder
        string videoPath = Path.Combine(Application.streamingAssetsPath, "myVideo.mp4");

        // On Windows, backslashes can cause issues in a URL, so replace them
        videoPath = videoPath.Replace("\\", "/");

        // Assign final path to the videoPlayer
        videoPlayer.url = videoPath;

        // (Optional) Mute the video if you want to try auto-play
        // videoPlayer.SetDirectAudioMute(0, true);

        // Start the video
        videoPlayer.Play();
    }
}
