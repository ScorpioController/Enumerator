利用C#中的迭代器机制，可以近似实现Unity中的协程Coroutine

1. 实现对IEnumrator的封装

   ```c#
   public class Coroutine
   {
       public string Name { get; private set; }
   
     	// 迭代器递归调用栈
     	// 实际调用过程中可能包含迭代器链
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
   
     	// 递归的MoveNext实现
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
         	// 当前所在这一层的迭代器执行完毕，从栈中移出
           else
           {
               Debug.Log($"{it.GetType()}", $"end");
               _enumerators.Pop();
               if (_enumerators.Count == 0) return false;
               return false || MoveNext(_enumerators.Peek());
           }
       }
   }
   ```

2. 实现驱动Coroutine的引擎层

   ```c#
   public class CoroutineEngine
   {
       List<Coroutine> coroutines = new List<Coroutine>();
           
       public CoroutineEngine()
       {
           Debug.Log("CoroutineEngine", "setup");
       }
           
       public void Update()
       {
           //Debug.Log("CoroutineEngine", "Update");
       }
   
       public void CoroutineUpdate()
       {
           for (int i = 0; i < coroutines.Count; i++)
           {
               if (coroutines[i].MoveNext())
               {
                   Debug.Log("CoroutineEngine", "CoroutineUpdate");
               }
               else
               {
                   Debug.Log("CoroutineEngine", "remove corotine : " + coroutines[i].Name);
                   coroutines.RemoveAt(i);
                   i--;
               }
           }
       }
   		// 开启协程（添加到coroutine列表中）
       public void StartCoroutine(IEnumerator coroutine)
       {
           coroutines.Add(new Coroutine(coroutine));
       }
   }
   ```

3. 实现特定的可等待对象

   ```c#
   public class WaitForSeconds : IEnumrator
   {
       // 记录ticks
       private long targetTicks;
       public WaitForSeconds(float seconds)
       {
           // 初始化ticks
           DateTime.Now.AddSeconds(seconds).Ticks;
       }
   
       public object Current => null;
       public void Reset() {}
       
       public bool MoveNext()
       {
           return targetTicks > DataTime.Now.Ticks;
       }
   
   }
   ```

4. 模拟游戏主循环

   ```c#
   // 初始化协程引擎
   var engine = new CoroutineEngine();
   engine.StartCoroutine(TestWaitForSeconds());
   
   // 模拟游戏主循环
   while (true)
   {
     	// 驱动协程更新
     	engine.CoroutineUpdate();
   		Thread.Sleep(33);
   }
   
   //...
   static IEnumerator TestWaitForSeconds()
   {
       Debug.Log("TestWaitForSeconds", "start WaitForSeconds");
       yield return new WaitForSeconds(5);
       Debug.Log("TestWaitForSeconds", "stop WaitForSeconds");
       yield return 2;
   }
   ```
   
   



