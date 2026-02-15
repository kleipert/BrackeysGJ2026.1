using TMPro;
using UnityEngine;

public class AudioTyping: MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip typingLoop;

    private int lastVisible = -1;
    private bool isTyping;

    void OnEnable() => Canvas.willRenderCanvases += OnWillRenderCanvases;
    void OnDisable() => Canvas.willRenderCanvases -= OnWillRenderCanvases;

    void OnWillRenderCanvases()
    {
        if (text == null || audioSource == null || typingLoop == null) return;

        text.ForceMeshUpdate();

        int total = text.textInfo.characterCount;
        int visible = text.maxVisibleCharacters;

        if (total <= 0)
        {
            StopLoop();
            isTyping = false;
            lastVisible = visible;
            return;
        }

        bool allVisible = (visible == int.MaxValue) || (visible >= total);
        bool increased = visible > lastVisible;
        
        if (!allVisible && increased)
        {
            isTyping = true;
            StartLoop();
        }
        
        if (isTyping && allVisible)
        {
            isTyping = false;
            StopLoop();
        }
        
        if (allVisible && !increased)
        {
            StopLoop();
            isTyping = false;
        }

        lastVisible = visible;
    }

    void StartLoop()
    {
        audioSource.clip = typingLoop;
        audioSource.loop = true;
        if (!audioSource.isPlaying) audioSource.Play();
    }

    void StopLoop()
    {
        if (audioSource.isPlaying) audioSource.Stop();
    }
}