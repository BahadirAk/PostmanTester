namespace PostmanTester.Application.Extensions
{
    public static class StringExtensions
    {
        public static byte[] HexStringToByteArray(this string hex)
        {
            if (hex.StartsWith("0x"))
            {
                hex = hex.Substring(2);
            }

            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have an even length");
            }

            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
