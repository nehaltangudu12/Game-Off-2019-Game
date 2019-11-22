using System;
using System.Collections.Generic;
using UnityEngine;

public class PropertyEvent
{
    private readonly List<Act> Callbacks = new List<Act>();

    public void Clear()
    {
        Callbacks.ForEach(el =>
        {
            el.Mb = null;
            el.Changed = null;
        });
        Callbacks.Clear();
    }

    public void AddEvent(Action onChanged, MonoBehaviour mb, bool callEvenIfDisabled = false)
    {
        Callbacks.Add(new Act
        {
            Mb = mb,
            HasMb = mb != null,
            Changed = onChanged,
            CallEvenIfDisabled = callEvenIfDisabled
        });
    }

    public void RemoveEvent(Action onChanged)
    {
        Callbacks.RemoveAll(el => el.Changed == onChanged);
    }

    public void RemoveEvent(MonoBehaviour mb)
    {
        Callbacks.RemoveAll(el => el.Mb == mb);
    }

    public void Fire()
    {
        ChangeValue();
    }

    public void Fire(MonoBehaviour mb)
    {
        ChangeValue(mb);
    }

    private void ChangeValue(MonoBehaviour mb = null)
    {
        Callbacks.RemoveAll(el =>
        {
            try
            {
                // Here comes the magic: if monoBehaviour has been already removed we'll have null here
                if (el.HasMb && el.Mb == null)
                    return true;

                if (!el.HasMb || el.Mb.gameObject.activeInHierarchy && el.Mb.enabled || el.CallEvenIfDisabled)
                    if (mb == null || el.Mb == mb)
                        if (el.Changed != null)
                            el.Changed();
                return false;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                return false;
            }
        });
    }

    private class Act
    {
        public bool CallEvenIfDisabled;

        public Action Changed;
        public bool HasMb;
        public MonoBehaviour Mb;
    }
}

public class PropertyEvent<T>
{
    private readonly List<Act<T>> Callbacks = new List<Act<T>>();

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
            CallEvenIfDisabled = callEvenIfDisabled
        });
    }

    public void Clear()
    {
        Callbacks.Clear();
    }

    public void RemoveEvent(Action<T> onChanged)
    {
        Callbacks.RemoveAll(el => el.Changed == onChanged);
    }

    public void RemoveEvent(MonoBehaviour mb)
    {
        Callbacks.RemoveAll(el => el.Mb == mb);
    }

    public void Fire(T value, MonoBehaviour mb = null)
    {
        Callbacks.RemoveAll(el =>
        {
            try
            {
                // Here comes the magic: if monoBehaviour has been already removed we'll have null here
                if (el.HasMb && el.Mb == null)
                    return true;

                if (!el.HasMb || el.Mb.gameObject.activeInHierarchy && el.Mb.enabled || el.CallEvenIfDisabled)
                    if (mb == null || el.Mb == mb)
                        if (el.Changed != null)
                            el.Changed(value);
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
        public bool HasMb;
        public MonoBehaviour Mb;
    }
}