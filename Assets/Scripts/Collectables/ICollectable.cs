using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class ICollectable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer CollectableVisual;

    private Sequence _animSeq;
    private CollectablesSpawner _spawner;

    public void Init (CollectablesSpawner spawner)
    {
        _spawner = spawner;
        _animSeq = DOTween.Sequence();

        Animate ();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            _spawner.Collect(this);
        }
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.collider.CompareTag ("Player"))
        {
             _spawner.Collect(this);
        }
    }

    public void Collect ()
    {
        Debug.Log("Effect here");
    }

    void Animate ()
    {
        _animSeq.Append(CollectableVisual.transform.DOShakePosition (.25f, .5f, 3, 20).SetDelay(1.5f)).SetLoops(-1);

        _animSeq.Play();
    }
}