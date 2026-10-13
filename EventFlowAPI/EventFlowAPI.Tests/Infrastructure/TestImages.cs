namespace EventFlowAPI.Tests.Infrastructure
{
    public static class TestImages
    {
        public static readonly byte[] Png = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==");

        public static readonly byte[] Text = System.Text.Encoding.UTF8.GetBytes("not an image");
    }
}
