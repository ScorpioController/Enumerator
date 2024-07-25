using System.Runtime.CompilerServices;

namespace Enumerator;

// [CompilerGenerated] // 指示编译器生成的元素

// 实际使用的private和sealed修饰符是编译器为了避免外部访问设置的
public sealed class Iterator
{
    // 迭代次数限制
    public int _limit;
    
    private int limit;
    
    // 迭代状态
    // -2   迭代前的初始状态
    // -1   迭代状态
    // 0    迭代预备
    // 正整数    编译期间生成的状态码，和yield return操作绑定
    private int _state;
    
    // 当前的值
    private int _current;
    
    // 记录线程编号来保护不被多线程情况修改
    private int _initialThreadId;
    
    // 记录迭代次数
    private int _index;
    
    public Iterator(int state)
    {
        _state = state;
        _initialThreadId = Environment.CurrentManagedThreadId;
    }

    public int Current { get { return _current; } }

    public bool MoveNext()
    {
        if (_state != 0)
        {
            if (_state != 1) return false;

            _state = -1;
            _index++;
        }
        // 即第一次迭代，修改迭代状态和初始值
        else
        {
            _state = -1;
            _index = 1;
        }

        // 对照迭代限制
        if (_index <= limit)
        {
            _current = _index * 2;
            // 指定正整数的迭代id
            // 实际的迭代代码可能包含各种复杂结构
            // 编译器实现MoveNext时会利用一个临时编码记录一下迭代的次序、迭代的规则以及迭代的过程
            // 再配合 goto 语句和标签，便可以完美完成任何情况的跳转
            _state = 1;

            return true;
        }

        return false;
    }
    
    public Iterator GetEnumerator()
    {
        Iterator iterator;
        // 检查迭代状态和线程id，符合条件则切换为迭代预备状态
        if (_state == -2 && _initialThreadId == Environment.CurrentManagedThreadId)
        {
            _state = 0;
            iterator = this;
        }
        else { iterator = new Iterator(0); }

        iterator.limit = _limit;
        return iterator;
    }   
    
    // [IteratorStateMachine(typeof(Iterator))] // 标识完成迭代机制的方法
    // 实际使用的是private标识
    public static Iterator GenerateEvenSeriesInter(int limit)
    {
        Iterator iterator = new Iterator(-2);
        iterator._limit = limit;

        return iterator;
    }
}