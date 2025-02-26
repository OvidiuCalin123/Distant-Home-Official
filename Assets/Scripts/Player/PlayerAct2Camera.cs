using System.Collections;
using UnityEngine;
using Cinemachine;

public class PlayerAct2Camera : MonoBehaviour
{
    public CinemachineVirtualCamera cinemachineCamera; // Reference to the Cinemachine Virtual Camera
    public float moveAmount = 2f; // Amount of amplified vertical movement for the camera
    public float slowOscillationAmount = 0.5f; // Amount of slow up-and-down motion
    public float slowOscillationSpeed = 1f; // Speed of the slow up-and-down motion

    private CinemachineFramingTransposer framingTransposer; // Reference to the Framing Transposer
    private float originalYOffset;
    private float nextAmplifyTime;
    private bool isAmplifying = false;

    void Start()
    {
        if (cinemachineCamera != null)
        {
            // Get the Framing Transposer component
            framingTransposer = cinemachineCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (framingTransposer != null)
            {
                originalYOffset = framingTransposer.m_TrackedObjectOffset.y;
            }
            else
            {
                Debug.LogError("Framing Transposer is not assigned or found in the Virtual Camera.");
            }
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera is not assigned in the Inspector!");
        }

        // Schedule the first random amplification
        ScheduleNextAmplify();
    }

    void Update()
    {
        if (framingTransposer != null)
        {
            // Apply continuous slow up-and-down motion
            ApplySlowOscillation();

            // Check if it's time to amplify the motion
            if (!isAmplifying && Time.time >= nextAmplifyTime)
            {
                StartCoroutine(AmplifyCameraMotion());
                ScheduleNextAmplify();
            }
        }
    }

    private void ScheduleNextAmplify()
    {
        // Random time between 3 and 10 seconds for the next amplification
        nextAmplifyTime = Time.time + Random.Range(15f, 30f);
    }

    private void ApplySlowOscillation()
    {
        if (!isAmplifying)
        {
            // Oscillate the Y offset using a sine wave for smooth motion
            float oscillation = Mathf.Sin(Time.time * slowOscillationSpeed) * slowOscillationAmount;
            framingTransposer.m_TrackedObjectOffset.y = originalYOffset + oscillation;
        }
    }

    private IEnumerator AmplifyCameraMotion()
    {
        isAmplifying = true;

        // Amplify the Y offset
        float targetYOffset = originalYOffset + moveAmount;
        float elapsedTime = 0f;
        float duration = 0.1f; // How fast the offset changes during amplification

        while (elapsedTime < duration)
        {
            float oscillation = Mathf.Sin(Time.time * slowOscillationSpeed) * slowOscillationAmount;
            framingTransposer.m_TrackedObjectOffset.y = Mathf.Lerp(originalYOffset + oscillation, targetYOffset, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hold the amplified motion for a short time
        yield return new WaitForSeconds(0.2f);

        // Smoothly return to slow oscillation
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float oscillation = Mathf.Sin(Time.time * slowOscillationSpeed) * slowOscillationAmount;
            framingTransposer.m_TrackedObjectOffset.y = Mathf.Lerp(targetYOffset, originalYOffset + oscillation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isAmplifying = false;
    }
}
