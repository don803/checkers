using Checkers.Core;
using Xunit;

namespace Checkers.Tests;

public class MoveNotationTests
{
    [Fact]
    public void ParsesSingleMove()
    {
        var ok = MoveNotation.TryParsePath("b6-a5", out var path);

        Assert.True(ok);
        Assert.Equal(new[]
        {
            new Position(2, 1),
            new Position(3, 0)
        }, path);
    }

    [Fact]
    public void ParsesMultiJumpMove()
    {
        var ok = MoveNotation.TryParsePath("b6-d4-f2", out var path);

        Assert.True(ok);
        Assert.Equal(3, path.Count);
    }

    [Fact]
    public void RejectsInvalidInput()
    {
        var ok = MoveNotation.TryParsePath("z9-a1", out _);

        Assert.False(ok);
    }
}
