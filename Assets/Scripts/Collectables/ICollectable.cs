using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

public class ICollectable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer CollectableVisual;
    [SerializeField] private AudioClip CollectableSound;

    protected Sequence _animSeq;
    private AudioController _audioControl;
    protected CollectablesSpawner _spawner;

    protected SpriteRenderer _visual => CollectableVisual;

    public void Init (CollectablesSpawner spawner)
    {
        _spawner = spawner;
        _animSeq = DOTween.Sequence ();
        _audioControl = AudioController.Instance;

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

            _audioControl.PlaySfx (CollectableSound, 1);
            Debug.Log ("Base Collect");
    }

    public virtual void Animate ()
    {
        _animSeq.Append (CollectableVisual.transform.DOScale (CollectableVisual.transform.localScale + new Vector3 (.2f, .2f, .2f),
            .2f).SetDelay (.7f)).SetLoops (-1, LoopType.Yoyo);

        _animSeq.Play ();
    }
}