基于管道思想进行集合的数据处理

案例：
对一组数据进行where和select操作

0- 一般实现

一次循环得到where后的新序列
二次循环得到select后的结果序列

1. 延迟处理

使用linq则会进行"延迟处理"

where操作符的迭代器内部驱动原始集合返回的迭代器
select操作符的迭代器内部驱动where操作符返回的迭代器

参考实现如下：

```c#
// where iterator
public bool MoveNext()
{
    switch (state)
    {
        case -2:
            enumerator = source.GetEnumerator();
            state = -1;
            goto case -1;

        case -1:
            while (enumerator.MoveNext())
            {
                current = enumerator.Current;
              	// 这里的predicate是条件筛选器
                if (predicate(current))
                {
                    state = 0;
                    return true;
                }
            }
            Dispose();
            break;

        case 0:
            state = -1;
            goto case -1;
    }

    return false;
}

// select iterator
public bool MoveNext()
{
    switch (state)
    {
        case -2:
            enumerator = source.GetEnumerator();
            state = -1;
            goto case -1;

        case -1:
            while (enumerator.MoveNext())
            {
                // 这里的selector是条件过滤器
                current = selector(enumerator.Current);
                state = 0;
                return true;
            }

            Dispose();
            break;

        case 0:
            state = -1;
            goto case -1;
    }

    return false;
}
```

```c#
var numbers = new List<int> { 1, 2, 3, 4, 5 };
var query = numbers.Where(n => n > 2).Select(n => n * 2);

foreach (var item in query)
{
    Console.WriteLine(item);
}

// 等效代码
var queryIterator = query.GetEnumerator();
while (queryIterator.MoveNext())
{
    // queryIterator => select iterator
    // select iterator -> where iterator
    // where iterator -> raw iterator
    // 每当 where iterator 取得满足 predicate 的值时才会返回true
    // 此时进入 select iterator 的 selector 逻辑
    
    var value = queryIterator.Current;
    Console.WriteLine(value);
}
```


2. 流处理

这种基于操作符的实现也满足的流式处理的编程范式，使得可以很容易的编写一段高可读性的数据处理操作。而上面所描述的延迟处理的特点也使得这种处理有较高的性能（数据处理是逐个的，可以避免每一次处理完全遍历从而减少了多余的计算开销，【？】也可以避免一次性将数据加载）







