using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationHandler : MonoBehaviour
{
    private void Start()
    {
        if (CompareTag("Player"))
        {
            StopRandomizeIdleAnimation();
            StartCoroutine(RandomizeIdleAnimation(gameObject));
        }
    }

    public void StopRandomizeIdleAnimation()
    {
        StopAllCoroutines();
    }

    public void StartRandomizeIdleAnimation(GameObject character)
    {
        if (CompareTag("Player"))
        {
            GetComponent<RandomAnimationHandler>().StopRandomizeIdleAnimation();
            StartCoroutine(GetComponent<RandomAnimationHandler>().RandomizeIdleAnimation(character));
        }
    }

    public IEnumerator RandomizeIdleAnimation(GameObject character)
    {
        int randomWaitTime = Random.Range(5, 15);
        yield return new WaitForSeconds(randomWaitTime);
        int randomizedAnimation = Random.Range(1, 4);
        Movement m = character.GetComponent<Movement>();
        m.UpdateSkeleton();
        if (m.usingFrontSkeleton)
        {
            m.frontSkeleton.timeScale = 1;
            m.nextIdleAnimation = randomizedAnimation switch
            {
                1 => m.frontIdle2,
                2 => m.frontIdle3,
                _ => m.frontIdle,
            };
            m.frontSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false);
            yield return new WaitForSeconds(m.frontSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false).AnimationEnd);
            m.nextIdleAnimation = m.frontIdle;
            m.frontSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, true);
        }
        else
        {
            m.backSkeleton.timeScale = 1;
            m.nextIdleAnimation = randomizedAnimation switch
            {
                2 => m.backIdle2,
                3 => m.backIdle3,
                _ => m.backIdle,
            };
            m.backSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false);
            yield return new WaitForSeconds(m.backSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false).AnimationEnd);
            m.nextIdleAnimation = m.backIdle;
            m.backSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, true);
        }
        GetComponent<Movement>().RepeatIdleRandomizer(character);
    }
}
