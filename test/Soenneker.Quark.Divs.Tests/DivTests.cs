using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Quark.Divs.Tests;

[Collection("Collection")]
public sealed class DivTests : FixturedUnitTest
{
    public DivTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
    }

    [Fact]
    public void Default()
    {

    }
}
