
using System;
using System.Collections.Generic;
using System.Text;


namespace DbTransform
{


    class ByteDateHandling
    {


        // lol(DateTime.Now);
        public static string lol(DateTime dt)
        {
            DateTime zero = new DateTime(1900, 1, 1);

            TimeSpan ts = dt - zero;
            TimeSpan ms = ts.Subtract(new TimeSpan(ts.Days, 0, 0, 0));


            // double d = 10.0 / 3.0;      // = 3.3333333333333335
            //string hex = "0x" + ts.Days.ToString("X8") + ((int)(ms.TotalMilliseconds * 10.0 / 3.0)).ToString("X8");
            string hex = "0x" + ts.Days.ToString("X8") + ((int)(ms.TotalMilliseconds * 3.33333333)).ToString("X8");
            return hex;
        } // End Function lol


        // http://stackoverflow.com/questions/7412944/convert-datetime-to-hex-equivalent-in-vb-net
        static string HexDateTimeToDateTimeString(string dateTimeHexString)
        {
            string datePartHexString = dateTimeHexString.Substring(0, 8);
            int datePartInt = Convert.ToInt32(datePartHexString, 16);
            DateTime dateTimeFinal = (new DateTime(1900, 1, 1)).AddDays(datePartInt);

            string timePartHexString = dateTimeHexString.Substring(8, 8);
            int timePartInt = Convert.ToInt32(timePartHexString, 16);
            double timePart = timePartInt * 10.0 / 3.0;
            dateTimeFinal = dateTimeFinal.AddMilliseconds(timePart);

            return dateTimeFinal.ToString();
        } // End Function HexDateTimeToDateTimeString


        static DateTime HexDateTimeToDateTime(string dateTimeHexString)
        {
            string datePartHexString = dateTimeHexString.Substring(0, 8);
            int datePartInt = Convert.ToInt32(datePartHexString, 16);
            DateTime dateTimeFinal = (new DateTime(1900, 1, 1)).AddDays(datePartInt);

            string timePartHexString = dateTimeHexString.Substring(8, 8);
            int timePartInt = Convert.ToInt32(timePartHexString, 16);
            double timePart = timePartInt * 10.0 / 3.0;
            dateTimeFinal = dateTimeFinal.AddMilliseconds(timePart);

            return dateTimeFinal;
        } // End Function HexDateTimeToDateTime




        /*
        static string HexDateToDateString(string dateHexString)
        {
            int days = byte.Parse(dateHexString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber)
                       | byte.Parse(dateHexString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) << 8
                       | byte.Parse(dateHexString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) << 16;
            DateTime dateFinal = new DateTime(1, 1, 1).AddDays(days);
            return dateFinal.Date.ToString();
        } // End Function HexDateToDateString
        */


        // http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
        // https://github.com/mono/mono/blob/master/mcs/class/corlib/System.Runtime.Remoting.Metadata.W3cXsd2001/SoapHexBinary.cs

        public static string ByteArrayToString(byte[] ba)
        {
            System.Text.StringBuilder hex = new System.Text.StringBuilder(ba.Length * 2);

            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);

            return hex.ToString();
        } // End Function ByteArrayToString


        public static string ByteArrayToString2(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        } // End Function ByteArrayToString2


        static string ByteToHexBitFiddleUpperCase(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            } // Next i

            return new string(c);
        } // End Function ByteToHexBitFiddleUpperCase


        static string ByteToHexBitFiddleLowerCase(byte[] bytes)
        {
            char[] c = new char[bytes.Length * 2];
            int b;
            for (int i = 0; i < bytes.Length; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(87 + b + (((b - 10) >> 31) & -39));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(87 + b + (((b - 10) >> 31) & -39));
            } // Next i

            return new string(c);
        } // End Function ByteToHexBitFiddleLowerCase


        public static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length / 2;
            byte[] bytes = new byte[NumberChars];

            using (System.IO.StringReader sr = new System.IO.StringReader(hex))
            {
                for (int i = 0; i < NumberChars; i++)
                    bytes[i] = Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
            } // End Using sr

            return bytes;
        } // End Function StringToByteArray



        public static void Test()
        {
            byte[] ba = System.IO.File.ReadAllBytes(@"D:\Stefan.Steiger\Pictures\unicorn_logo-9d56a27152325cc090d46de2b20b48e6.png");
            string strba = ByteArrayToString(ba);
            Console.WriteLine(strba);
            string strbaUpper = ByteToHexBitFiddleLowerCase(ba);
            Console.WriteLine(strbaUpper);


            DateTime dt = DateTime.Now;
            dt = HexDateTimeToDateTime("0000A3A200E6D1BC");
            string rofl = lol(dt);
            rofl = rofl.Substring(2, rofl.Length - 2);
            string datestr = HexDateTimeToDateTimeString(rofl);
        } // End Sub Test


    }


}
