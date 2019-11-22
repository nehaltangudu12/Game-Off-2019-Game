using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Property<T>
{
    private readonly List<Act<T>> Callbacks = new List<Act<T>>();

    private T currentValue;

    public Property()
    {
    }

    public Property(T defaultValue)
    {
        currentValue = defaultValue;
    }

    public virtual T Value
    {
        get { return currentValue; }
        set { ChangeValue(value); }
    }

    public void AddEvent(Action<T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        Callbacks.Add(new Act<T>
        {
            Mb = mb,
            HasMb = mb != null,
            Changed = onChanged,
            CallEvenIfDisabled = callEvenIfDisabled
        });
    }

    public void AddEvent(Action<T, T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        Callbacks.Add(new Act<T>
        {
            Mb = mb,
            HasMb = mb != null,
            ChangedWithPrev = onChanged,
            CallEvenIfDisabled = callEvenIfDisabled
        });
    }

    public void AddEventAndFire(Action<T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        AddEvent(onChanged, mb, callEvenIfDisabled);
        onChanged(currentValue);
    }

    public void AddEventAndFire(Action<T, T> onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        AddEvent(onChanged, mb, callEvenIfDisabled);
        onChanged(currentValue, currentValue);
    }

    public void RemoveEvent(Action<T> onChanged)
    {
        Callbacks.RemoveAll(el => el.Changed == onChanged);
    }

    public void RemoveEvent(Action<T, T> onChanged)
    {
        Callbacks.RemoveAll(el => el.ChangedWithPrev == onChanged);
    }

    public void RemoveEvent(MonoBehaviour mb)
    {
        Callbacks.RemoveAll(el => el.Mb == mb);
    }

    public void RemoveAllEvents()
    {
        Callbacks.Clear();
    }

    public void Fire()
    {
        ChangeValue(currentValue);
    }

    public void Fire(MonoBehaviour mb)
    {
        ChangeValue(currentValue, mb);
    }

    public void Fire(T newValue)
    {
        Value = newValue;
    }

    private void ChangeValue(T value, MonoBehaviour mb = null)
    {
        var oldValue = currentValue;
        currentValue = value;

        Callbacks.RemoveAll(el =>
        {
            try
            {
                // Here comes the magic: if monoBehaviour has been already removed we'll have null here
                if (el.HasMb && el.Mb == null)
                    return true;

                if (!el.HasMb || el.Mb.gameObject.activeInHierarchy && el.Mb.enabled || el.CallEvenIfDisabled)
                    if (mb == null || el.Mb == mb)
                    {
                        if (el.Changed != null)
                            el.Changed(currentValue);
                        if (el.ChangedWithPrev != null)
                            el.ChangedWithPrev(currentValue, oldValue);
                    }

                return false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        });
    }

    private class Act<TT>
    {
        public bool CallEvenIfDisabled;

        public Action<TT> Changed;
        public Action<TT, TT> ChangedWithPrev;
        public bool HasMb;
        public MonoBehaviour Mb;
    }
}