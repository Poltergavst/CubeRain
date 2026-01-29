using TMPro;
using UnityEngine;

public abstract class SpawnerView<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI TextField;
    [SerializeField] private Spawner<T> _spawner;

    private int _createdObjectsCounter;
    private int _objectsTotal;
    private int _objectsActive;

    private void OnEnable()
    {
        _spawner.InstanceCreated += AddToCounter;
        _spawner.PoolChanged += RefreshStats;
    }

    private void OnDisable()
    {
        _spawner.InstanceCreated -= AddToCounter;
        _spawner.PoolChanged -= RefreshStats;
    }

    private void Start()
    {
        _createdObjectsCounter = 0;
        _objectsTotal = 0;
        _objectsActive = 0;

        ShowStats();
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
        TextField.text = MakeString();
    }
}
