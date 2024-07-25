using System.Collections;

namespace Coroutine;

class Program
{
    static void Main(string[] args)
    {
        var engine = new CoroutineEngine();
        engine.StartCoroutine(TestWaitForSeconds());
        // 模拟游戏主循环
        while (true)
        {
            engine.CoroutineUpdate();
            Thread.Sleep(33);   
        }
    }
    
    static IEnumerator TestWaitForSeconds()
    {
        Debug.Log("TestWaitForSeconds", "start WaitForSeconds");
        yield return new WaitForSeconds(5);
        Debug.Log("TestWaitForSeconds", "stop WaitForSeconds");
        yield return 2;
    }
}