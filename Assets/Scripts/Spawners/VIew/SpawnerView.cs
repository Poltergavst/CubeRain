using TMPro;
using UnityEngine;

public abstract class SpawnerView<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _textField;
    [SerializeField] private Spawner<T> _spawner;

    private int _createdObjectsCounter;
    private int _objectsTotal;
    private int _objectsActive;

    private void OnEnable()
    {
        _createdObjectsCounter = 0;
        _objectsTotal = 0;
        _objectsActive = 0;

        ShowStats();

        _spawner.InstanceCreated += AddToCounter;
        _spawner.PoolChanged += RefreshStats;
    }

    private void AddToCounter()
    {
        _createdObjectsCounter++;

        ShowStats();
    }

    private void RefreshStats(int objectsTotal, int activeObjects)
    {
        _objectsTotal = objectsTotal;
        _objectsActive = activeObjects;

        ShowStats();
    }

    protected virtual string MakeString()
    {
        return $"Всего: {_objectsTotal}\nАктивно: {_objectsActive}\nЗаспавнено за все время: {_createdObjectsCounter}";
    }

    private void ShowStats()
    {
        _textField.text = MakeString();
    }
}
