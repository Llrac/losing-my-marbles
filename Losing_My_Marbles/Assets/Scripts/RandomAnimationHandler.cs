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
        GetComponent<RandomAnimationHandler>().StopRandomizeIdleAnimation();
        StartCoroutine(GetComponent<RandomAnimationHandler>().RandomizeIdleAnimation(character));
    }

    public IEnumerator RandomizeIdleAnimation(GameObject character)
    {
        int randomWaitTime = Random.Range(5, 15);
        yield return new WaitForSeconds(randomWaitTime);
        Movement m = character.GetComponent<Movement>();
        m.UpdateSkeleton();
        if (m.usingFrontSkeleton)
        {
            m.frontSkeleton.timeScale = 1;
            if (character.CompareTag("Player"))
            {
                int randomizedAnimation = Random.Range(1, 4); // 3 unique animations
                m.nextIdleAnimation = randomizedAnimation switch
                {
                    1 => m.pFrontIdle2,
                    2 => m.pFrontIdle3,
                    _ => m.frontIdle,
                };
            }
            else if (character.CompareTag("Enemy"))
            {
                int randomizedAnimation = Random.Range(1, 6); // 5 unique animations
                m.nextIdleAnimation = randomizedAnimation switch
                {
                    1 => m.rFrontIdle2,
                    2 => m.rFrontIdle3,
                    3 => m.rFrontIdle4,
                    4 => m.rFrontIdle5,
                    _ => m.frontIdle,
                };
            }
            
            m.frontSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false);
            yield return new WaitForSeconds(m.frontSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false).AnimationEnd);
            m.nextIdleAnimation = m.frontIdle;
            m.frontSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, true);
        }
        else
        {
            m.backSkeleton.timeScale = 1;
            if (character.CompareTag("Player"))
            {
                int randomizedAnimation = Random.Range(1, 4); // 3 unique animations
                m.nextIdleAnimation = randomizedAnimation switch
                {
                    2 => m.pBackIdle2,
                    3 => m.pBackIdle3,
                    _ => m.backIdle,
                };
            }
            else if (character.CompareTag("Enemy"))
            {
                int randomizedAnimation = Random.Range(1, 6); // 5 unique animations
                m.nextIdleAnimation = randomizedAnimation switch
                {
                    1 => m.rBackIdle2,
                    2 => m.rBackIdle3,
                    3 => m.rBackIdle4,
                    4 => m.rBackIdle5,
                    _ => m.backIdle,
                };
            }

            m.backSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false);
            yield return new WaitForSeconds(m.backSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, false).AnimationEnd);
            m.nextIdleAnimation = m.backIdle;
            m.backSkeleton.AnimationState.SetAnimation(0, m.nextIdleAnimation, true);
        }
        GetComponent<Movement>().RepeatIdleRandomizer(character);
    }
}
