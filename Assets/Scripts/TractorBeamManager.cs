using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TractorBeamManager : MonoBehaviour
{
    Animator animator;
    private bool isAnimationDone = false;
    Transform currentLookAtTransform;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (currentLookAtTransform != null)
        {
            
        }
    }
    public void ProgressFinished()
    {
        isAnimationDone = true;
        Debug.Log("tractor progress finished!");

    }
    public IEnumerator PlaceAndScaleTo(Vector3 destination, float startScale, float endScale, Vector3 lookAtPosition)
    {
        Debug.Log("tractor started!");

        // Start the animation from the beginning
        transform.position = destination;
        transform.LookAt(lookAtPosition - new Vector3(0,0.2f,0));
        animator.Play("TractorBeamEaseToOne", 0, 0f);
        // Wait one frame so Animator updates
        yield return null;
        float scale;
        // Keep updating position while the animation is running
        while (!isAnimationDone)
        {

            float progress = animator.GetFloat("progress");
            scale = Mathf.Lerp(startScale, endScale, progress);

            transform.localScale = new Vector3(scale, scale, scale);


            yield return null; // Wait for next frame
        }

        // Ensure final position is exact
        // transform.position = endPosition;

        // Reset the flag so it's ready for next time
        isAnimationDone = false;

    }
}
