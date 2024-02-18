using Spectre.Console;
using Spectre.Console.Rendering;

public class MyAnsiConsole : IAnsiConsole
{
    /// <inheritdoc />
    public void Clear(bool home) => AnsiConsole.Console.Clear(home);

    /// <inheritdoc />
    public void Write(IRenderable renderable) => AnsiConsole.Console.Write(renderable);

    /// <inheritdoc />
    public Profile Profile => AnsiConsole.Console.Profile;

    /// <inheritdoc />
    public IAnsiConsoleCursor Cursor => AnsiConsole.Console.Cursor;

    /// <inheritdoc />
    public IAnsiConsoleInput Input => AnsiConsole.Console.Input;

    /// <inheritdoc />
    public IExclusivityMode ExclusivityMode => AnsiConsole.Console.ExclusivityMode;

    /// <inheritdoc />
    public RenderPipeline Pipeline => AnsiConsole.Console.Pipeline;
}