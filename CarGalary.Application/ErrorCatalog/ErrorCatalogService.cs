using System.Reflection;
using System.Text.Json;

namespace CarGalary.Application.ErrorCatalog
{
    public class ErrorCatalogService : IErrorCatalogService
    {
        private readonly Dictionary<string, ErrorCatalogEntry> _byCode;
        private readonly Dictionary<string, string> _codeByMessage;

        public ErrorCatalogService()
        {
            var assembly = typeof(ErrorCatalogService).Assembly;
            var resourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith("ErrorCatalog.error-codes.json", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                _byCode = new Dictionary<string, ErrorCatalogEntry>(StringComparer.OrdinalIgnoreCase);
                _codeByMessage = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return;
            }

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                _byCode = new Dictionary<string, ErrorCatalogEntry>(StringComparer.OrdinalIgnoreCase);
                _codeByMessage = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                return;
            }

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var entries = JsonSerializer.Deserialize<List<ErrorCatalogEntry>>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<ErrorCatalogEntry>();

            _byCode = entries
                .Where(x => !string.IsNullOrWhiteSpace(x.ErrorCode))
                .GroupBy(x => x.ErrorCode, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Last(), StringComparer.OrdinalIgnoreCase);

            _codeByMessage = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in _byCode.Values)
            {
                RegisterMessage(entry.MessageEn, entry.ErrorCode);
                RegisterMessage(entry.MessageAr, entry.ErrorCode);
                foreach (var alias in entry.Aliases ?? new List<string>())
                {
                    RegisterMessage(alias, entry.ErrorCode);
                }
            }
        }

        public ErrorCatalogEntry? GetByCode(string errorCode)
        {
            if (string.IsNullOrWhiteSpace(errorCode))
            {
                return null;
            }

            return _byCode.TryGetValue(errorCode.Trim(), out var entry) ? entry : null;
        }

        public string? GetCodeByMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return null;
            }

            return _codeByMessage.TryGetValue(Normalize(message), out var code) ? code : null;
        }

        private void RegisterMessage(string? message, string code)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            _codeByMessage[Normalize(message)] = code;
        }

        private static string Normalize(string message)
        {
            return string.Join(" ", message.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }
    }
}
