using System.Reflection;
using System.Text.Json;

namespace CarGalary.Application.ErrorCatalog
{
    public class ErrorCatalogService : IErrorCatalogService
    {
        private readonly Dictionary<string, ErrorCatalogEntry> _byCode;

        public ErrorCatalogService()
        {
            var assembly = typeof(ErrorCatalogService).Assembly;
            var resourceName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(x => x.EndsWith("ErrorCatalog.error-codes.json", StringComparison.OrdinalIgnoreCase));

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                _byCode = new Dictionary<string, ErrorCatalogEntry>(StringComparer.OrdinalIgnoreCase);
                return;
            }

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                _byCode = new Dictionary<string, ErrorCatalogEntry>(StringComparer.OrdinalIgnoreCase);
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
        }

        public ErrorCatalogEntry? GetByCode(string errorCode)
        {
            if (string.IsNullOrWhiteSpace(errorCode))
            {
                return null;
            }

            return _byCode.TryGetValue(errorCode.Trim(), out var entry) ? entry : null;
        }
    }
}
