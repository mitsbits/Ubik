using System;

namespace Ubik.Web.Components
{
    internal class Guard
    {
        public static void ForNullOrEmpty(string value, string parameterName)
        {
            if (String.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new NullReferenceException(parameterName);
            }
        }

        public static void ForPrecedesDate(DateTime value, DateTime dateToPrecede, string parameterName)
        {
            if (value >= dateToPrecede)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForLatitudeValue(double latitude, string parameterName)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForLongtitudeValue(double longtitude, string parameterName)
        {
            if (longtitude < -180 || longtitude > 180)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }
        }

        public static void ForSizeValues(int width, int heigth)
        {
            if (width == default(int) && heigth == default(int)) return;

            if (width <= 0) throw new ArgumentOutOfRangeException("width");
            if (heigth <= 0) throw new ArgumentOutOfRangeException("heigth");
        }
    }
}