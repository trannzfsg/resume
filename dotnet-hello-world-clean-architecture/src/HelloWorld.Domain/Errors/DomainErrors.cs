using HelloWorld.Domain.Common;

namespace HelloWorld.Domain.Errors;

public static class DomainErrors
{
    public static class RecipientName
    {
        public static readonly Error Empty = new(
            "RecipientName.Empty",
            "A recipient name is required.");

        public static readonly Error TooLong = new(
            "RecipientName.TooLong",
            "A recipient name must be 80 characters or fewer.");

        public static readonly Error ContainsControlCharacters = new(
            "RecipientName.ContainsControlCharacters",
            "A recipient name cannot contain control characters.");
    }

    public static class GreetingMessage
    {
        public static readonly Error Empty = new(
            "GreetingMessage.Empty",
            "A greeting message is required.");

        public static readonly Error TooLong = new(
            "GreetingMessage.TooLong",
            "A greeting message must be 200 characters or fewer.");
    }

    public static class Greeting
    {
        public static readonly Error EmptyId = new(
            "Greeting.EmptyId",
            "A greeting must have a non-empty id.");

        public static readonly Error NonUtcTimestamp = new(
            "Greeting.NonUtcTimestamp",
            "A greeting timestamp must be expressed in UTC.");
    }
}
