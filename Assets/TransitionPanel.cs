using System.Collections;
using UnityEngine;

public class TransitionPanel : MonoBehaviour
{
    public Transform currentPanel;
    public Transform nextPanel;
    public float transitionDuration = 1.0f;
    public float maxScale = 2.0f; // The maximum scale for zoom-in

    private void Start()
    {
        // Initialize the transition
        StartCoroutine(Transition());
    }

    public IEnumerator Transition()
    {
        // Zoom In Animation (Current Panel)
        float timer = 0;
        Vector3 originalScale = currentPanel.localScale;

        while (timer < transitionDuration)
        {
            float progress = timer / transitionDuration;
            currentPanel.localScale = Vector3.Lerp(originalScale, originalScale * maxScale, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        currentPanel.localScale = originalScale * maxScale;

        // Deactivate the current panel
        currentPanel.gameObject.SetActive(false);

        // Activate the next panel
        nextPanel.gameObject.SetActive(true);

        // Zoom Out Animation (Next Panel)
        timer = 0;

        while (timer < transitionDuration)
        {
            float progress = timer / transitionDuration;
            nextPanel.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        nextPanel.localScale = originalScale;
    }
}
