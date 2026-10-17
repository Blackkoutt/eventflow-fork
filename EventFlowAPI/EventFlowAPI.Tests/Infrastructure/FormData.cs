namespace EventFlowAPI.Tests.Infrastructure
{
    public class FormData
    {
        private readonly List<KeyValuePair<string, string>> _fields = new();
        private readonly List<(string Name, string FileName, byte[] Bytes, string ContentType)> _files = new();

        public FormData Add(string name, object? value)
        {
            if (value != null)
            {
                _fields.Add(new KeyValuePair<string, string>(name, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)!));
            }
            return this;
        }

        public FormData AddFile(string name, string fileName, byte[] bytes, string contentType)
        {
            _files.Add((name, fileName, bytes, contentType));
            return this;
        }

        public MultipartFormDataContent ToContent()
        {
            var content = new MultipartFormDataContent();
            foreach (var field in _fields)
            {
                content.Add(new StringContent(field.Value), field.Key);
            }
            foreach (var file in _files)
            {
                var fileContent = new ByteArrayContent(file.Bytes);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, file.Name, file.FileName);
            }
            return content;
        }
    }
}
