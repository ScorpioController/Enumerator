using System.Collections;

namespace Coroutine;

public class WaitForSeconds : IEnumerator
{
    private long targetTicks;

    public WaitForSeconds(float seconds)
    {
        this.targetTicks = DateTime.Now.AddSeconds(seconds).Ticks;
    }

    public object Current => null;

    public bool MoveNext()
    {
        return this.targetTicks > DateTime.Now.Ticks;
    }

    public void Reset()
    {
    }
}