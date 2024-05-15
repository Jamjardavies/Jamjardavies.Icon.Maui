using System.Text;

namespace Jamjardavies.Icons.Generator;

public class AutoStringBuilder
{
    private readonly StringBuilder sb = new();
    private int indent;

    public Indenter Indent() => new(() => this.indent++, () => this.indent--);

    public void WriteLine(string line = "")
    {
        if (this.indent > 0)
        {
            this.sb.Append(new string('\t', this.indent));
        }

        this.sb.AppendLine(line);
    }

    public override string ToString() => this.sb.ToString();
}

public class Indenter : IDisposable
{
    private Action decrease;

    public Indenter(Action increase, Action decrease)
    {
        this.decrease = decrease;

        increase.Invoke();
    }

    public void Dispose()
    {
        this.decrease.Invoke();
    }
}