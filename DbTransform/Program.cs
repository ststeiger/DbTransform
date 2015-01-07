
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace DbTransform
{


    static class Program
    {


        //Dezimal Stunden in hh:mm Format umwandeln
        public static string FormatTime(double dblMinutes)
        {
            string str = "";

            try
            {
                if (dblMinutes == 0.0)
                {
                    str = "";
                }
                else
                {
                    string str2 = Microsoft.VisualBasic.Strings.FormatNumber((dblMinutes), 2, Microsoft.VisualBasic.TriState.True, Microsoft.VisualBasic.TriState.UseDefault, Microsoft.VisualBasic.TriState.True);
                    Console.WriteLine(str2);
                    dblMinutes = System.Convert.ToDouble(str2);
                    System.TimeSpan ts = new System.TimeSpan(0, System.Convert.ToInt32(dblMinutes), 0);
                    if (ts.Seconds > 0)
                    {
                        ts = ts.Add(new TimeSpan(0, 1, 0));
                    }
                    ts = ts.Subtract(new TimeSpan(0, 0, ts.Seconds));
                    str = string.Format("{0}:{1}", (ts.Days * 24 + ts.Hours).ToString().PadLeft(2, '0'), ts.Minutes.ToString().PadLeft(2, '0'));
                }
            }
            catch (System.Exception ex)
            {
                str = ex.Message;
            }

            if (str == "00:00")
            {
                str = "";
            }

            return str;
        } // End Function FormatTime


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool bShowForms = false;

            if (bShowForms)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            } // End if bShowForms

            // FormatTime(6.38221);
            // EmbeddedBrowserHelper.FixBrowserVersion();



            System.Globalization.IdnMapping idn = new System.Globalization.IdnMapping();
            string strPunyCode = idn.GetAscii("www.altstätten.ch");
            string strUnicode = idn.GetUnicode(strPunyCode);
            Console.WriteLine(strPunyCode);
            Console.WriteLine(strUnicode);


            IntermediateTable idt = new IntermediateTable("Profiles"); //"__types", "Profiles"
            //IntermediateTable idt = new IntermediateTable("__datatypemappings");
            Console.WriteLine("test");

            for (int i = 0; i < idt.Columns.Count; ++i)
            {
                string str = DbTransform.IntermediateDefinition.idl_2_pg(idt.Columns[i]);
                //string str = DbTransform.IntermediateDefinition.idl_2_ms(idt.Columns[i]);
                Console.WriteLine(str);
            } // Next i

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            Console.ReadKey();
        } // End Sub Main


    } // End Class Program


} // End Namespace DbTransform
