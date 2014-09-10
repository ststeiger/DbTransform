
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace DbTransform
{


    static class Program
    {


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
            }


			//IntermediateTable idt = new IntermediateTable("__types");
			IntermediateTable idt = new IntermediateTable("__datatypemappings");
            Console.WriteLine("test");

            for (int i = 0; i < idt.Columns.Count; ++i)
            {
				//string str = DbTransform.IntermediateDefinition.idl_2_pg(idt.Columns[i]);
				string str = DbTransform.IntermediateDefinition.idl_2_ms(idt.Columns[i]);
                Console.WriteLine(str);
            }

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            Console.ReadKey();
        }


    }


}
