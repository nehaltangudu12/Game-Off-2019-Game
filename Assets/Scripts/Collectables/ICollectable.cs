using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class ICollectable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer CollectableVisual;

    protected Sequence _animSeq;
    protected CollectablesSpawner _spawner;
    protected SpriteRenderer _visual => CollectableVisual;

    public void Init (CollectablesSpawner spawner)
    {
        _spawner = spawner;
        _animSeq = DOTween.Sequence ();

        Animate ();
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            Collect (other.gameObject);
        }
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.collider.CompareTag ("Player"))
        {
            Collect (other.gameObject);
        }
    }

    public virtual void Collect (GameObject target)
    {
        _spawner.Collect (this);
        Debug.Log ("Base Collect");
    }

    public virtual void Animate ()
    {
        _animSeq.Append (CollectableVisual.transform.DOScale (CollectableVisual.transform.localScale + new Vector3 (.2f, .2f, .2f),
            .2f).SetDelay (.7f)).SetLoops (-1, LoopType.Yoyo);

        _animSeq.Play ();
    }
}