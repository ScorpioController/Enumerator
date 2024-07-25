using System.Collections;

namespace Coroutine;

// 对迭代器的封装
public class Coroutine
{
    public string Name { get; private set; }

    Stack<IEnumerator> _enumerators = new Stack<IEnumerator>();

    public Coroutine(IEnumerator enumerator)
    {
        Name = enumerator.GetType().Name;
        _enumerators.Push(enumerator);
    }

    public bool MoveNext()
    {
        if (_enumerators.Count == 0) return false;
        return MoveNext(_enumerators.Peek());
    }

    private bool MoveNext(IEnumerator it)
    {
        if (it.MoveNext())
        {
            Debug.Log($"{it.GetType()}", $"start move next");
            // 处理嵌套的迭代器逻辑
            if (it.Current is IEnumerator)
            {
                var next = it.Current as IEnumerator;
                Debug.Log($"{it.GetType()}", $"start sub iterator {next.GetType()}");
                _enumerators.Push(next);
                MoveNext(next);
            }

            return true;
        }
        else
        {
            Debug.Log($"{it.GetType()}", $"end");
            _enumerators.Pop();
            if (_enumerators.Count == 0) return false;
            return false || MoveNext(_enumerators.Peek());
        }
    }
}