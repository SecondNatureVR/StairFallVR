using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SavedScore")]
public class SavedScore: ScriptableObject
{
    public GameEvent OnChange;
    private float _value = 0f;
    [SerializeField] public float value
    {
        get { return _value; }
        set
        {
            if (_value == value) return;
            _value = value;
            OnChange.Raise();
        }
    }
}
