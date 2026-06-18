using System.Globalization;
using System.Text;

namespace CarGalary.Application.Utilities
{
    public static class ZatcaQrCodeBuilder
    {
        public static string BuildPhaseOnePayload(
            string sellerName,
            string vatRegistrationNumber,
            DateTime invoiceTimestamp,
            decimal invoiceTotalWithVat,
            decimal vatTotal)
        {
            var buffer = new List<byte>();

            AppendTlv(buffer, 1, sellerName);
            AppendTlv(buffer, 2, vatRegistrationNumber);
            AppendTlv(buffer, 3, FormatTimestamp(invoiceTimestamp));
            AppendTlv(buffer, 4, FormatAmount(invoiceTotalWithVat));
            AppendTlv(buffer, 5, FormatAmount(vatTotal));

            return Convert.ToBase64String(buffer.ToArray());
        }

        private static void AppendTlv(List<byte> target, byte tag, string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            if (valueBytes.Length > byte.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "TLV field exceeds the 255-byte QR limitation.");
            }

            target.Add(tag);
            target.Add((byte)valueBytes.Length);
            target.AddRange(valueBytes);
        }

        private static string FormatTimestamp(DateTime value)
        {
            var normalized = value.Kind switch
            {
                DateTimeKind.Utc => value,
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => DateTime.SpecifyKind(value, DateTimeKind.Local).ToUniversalTime()
            };

            return normalized.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
        }

        private static string FormatAmount(decimal value)
        {
            return value.ToString("0.00", CultureInfo.InvariantCulture);
        }
    }
}
