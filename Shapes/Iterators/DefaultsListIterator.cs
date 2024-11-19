namespace Shapes.Iterators;

public sealed class DefaultsListIterator<T> where T : IEquatable<T>
{
    private readonly List<T> _items;
    private T _currentItem;

    public DefaultsListIterator(List<T> items)
    {
        if (items.Count == 0)
        {
            throw new ArgumentException("The list cannot be empty.", nameof(items));
        }
        
        _items = items;
        _currentItem = items[0];
    }
    
    public int Count => _items.Count;
    
    public List<T> ToList() => _items;
    
    public T GetCurrent() => _currentItem;
    
    public void SetCurrent(T item) => _currentItem = _items.Single(i => i.Equals(item));

    public void MoveNext()
    {
        var nextIndex = _items.IndexOf(_currentItem) + 1;
        _currentItem = nextIndex == _items.Count
            ? _items[0]
            : _items[nextIndex];
    }
}