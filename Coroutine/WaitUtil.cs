using System.Collections;

namespace Coroutine;

public class WaitUtil : IEnumerator
{
    private Func<bool> _condition;

    public object Current => null;

    public WaitUtil(Func<bool> condition)
    {
        if (null == condition) throw new ArgumentNullException();
        this._condition = condition;
    }
    
    public bool MoveNext()
    {
        // 取反以满足条件时退出
        return !_condition();
    }

    public void Reset(){}
}