namespace HelloWorld.UnitTests.Practice;

public sealed class TestClassTests
{
    private readonly TestClass _sut = new();

    [Fact]
    public void TopWordsStartWithA_returns_most_frequent_words_up_to_count()
    {
        const string text = "Apple avocado apple apricot banana apple apricot ant";

        var result = _sut.TopWordsStartWithA(text, 3);

        Assert.Equal(["apple", "apricot", "ant"], result);
    }

    [Fact]
    public void TopWordsStartWithA_orders_words_alphabetically_when_frequencies_are_equal()
    {
        const string text = "avocado apricot apple";

        var result = _sut.TopWordsStartWithA(text, 3);

        Assert.Equal(["apple", "apricot", "avocado"], result);
    }

    [Fact]
    public void TopWordsStartWithA_is_case_insensitive_and_ignores_extra_spaces()
    {
        const string text = "  APPLE   Apple  avocado   Banana ";

        var result = _sut.TopWordsStartWithA(text, 5);

        Assert.Equal(["apple", "avocado"], result);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void TopWordsStartWithA_returns_empty_for_missing_text(string? text)
    {
        var result = _sut.TopWordsStartWithA(text, 5);

        Assert.Empty(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TopWordsStartWithA_returns_empty_for_non_positive_count(int count)
    {
        var result = _sut.TopWordsStartWithA("apple avocado", count);

        Assert.Empty(result);
    }
}
