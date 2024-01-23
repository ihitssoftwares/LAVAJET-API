using System;

namespace PPR.Lite.Infrastructure.Helpers
{
    public static class MakeSafe
    {
        #region DATETIME CONVERSION

        /// <summary>
        ///     Safely convert object to date-time.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The output date-time.</returns>
        public static DateTime ToSafeDateTime(object inputValue)
        {
            try
            {
                DateTime result;

                if (inputValue == null || inputValue == DBNull.Value ||
                    !DateTime.TryParse(inputValue.ToString(), out result))
                {
                    return DateTime.MinValue;
                }
                //IFormatProvider culture = new System.Globalization.CultureInfo("en-US", true);
                //DateTime dateVal = DateTime.ParseExact(inputValue, "yyyy-MM-dd", culture);
                return result;
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }
        }

        #endregion

        #region BOOLEAN CONVERSION

        /// <summary>
        ///     Safely convert object to boolean.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The output boolean.</returns>
        public static bool ToSafeBool(object inputValue)
        {
            if (inputValue == null || inputValue == DBNull.Value || inputValue.ToString() == "0")
            {
                return false;
            }

            if (inputValue.ToString() == "1")
            {
                return true;
            }

            bool result;

            if (bool.TryParse(inputValue.ToString(), out result))
            {
                return result;
            }

            return false;
        }

        #endregion

        public static Guid ToSafeGuid(object inputValue)
        {
            Guid result;

            if (inputValue == null || inputValue == DBNull.Value || !Guid.TryParse(inputValue.ToString(), out result))
            {
                return new Guid();
            }

            return result;
        }

        #region STRING CONVERSION

        /// <summary>
        ///     Safely convert object to string.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The output string.</returns>
        public static string ToSafeString(object inputValue)
        {
            if (inputValue == null || inputValue == DBNull.Value)
            {
                return string.Empty;
            }

            return inputValue.ToString();
        }

        /// <summary>
        ///     Safely convert object to string or null.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The output string or NULL.</returns>
        public static string ToSafeStringOrNull(object inputValue)
        {
            if (inputValue == null || inputValue == DBNull.Value)
            {
                return null;
            }

            return inputValue.ToString();
        }

        /// <summary>
        ///     Convert object to string or null.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The output string or NULL.</returns>
        public static string ToStringOrNull(string inputValue)
        {
            if (string.IsNullOrEmpty(inputValue))
            {
                return null;
            }

            return inputValue;
        }

        #endregion

        #region INTEGER CONVERSION

        /// <summary>
        ///     Safely convert object to integer-32 with default value.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The output integer-32.</returns>
        public static int ToSafeInt32(object inputValue, int defaultValue)
        {
            int result;

            if (inputValue == null || inputValue == DBNull.Value || !int.TryParse(inputValue.ToString(), out result))
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        ///     Safely convert object to integer-32.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The output integer-32.</returns>
        public static int ToSafeInt32(object inputValue)
        {
            return ToSafeInt32(inputValue, 0);
        }

        #endregion

        #region DECIMAL CONVERSION

        /// <summary>
        ///     Safely convert object to decimal.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <returns>The output decimal.</returns>
        public static decimal ToSafeDecimal(object inputValue)
        {
            return ToSafeDecimal(inputValue, new decimal(0));
        }

        /// <summary>
        ///     Safely convert object to decimal with default value.
        /// </summary>
        /// <param name="inputValue">The input value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The output decimal.</returns>
        public static decimal ToSafeDecimal(object inputValue, decimal defaultValue)
        {
            decimal result;

            if (inputValue == null || inputValue == DBNull.Value ||
                !decimal.TryParse(inputValue.ToString(), out result))
            {
                return defaultValue;
            }

            return result;
        }

        #endregion
    }
}