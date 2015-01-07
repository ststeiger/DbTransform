
namespace DbTransform
{


    public class IntermediateTable
    {
        public string Name;
        public System.Collections.Generic.List<IntermediateDefinition> Columns = new System.Collections.Generic.List<IntermediateDefinition>();


        public static DB.Abstraction.cDAL SetupDAL()
        {
			// string strConnectionString = "Server=127.0.0.1;Port=5432;Database=lsmail;User ID=postgres;Password=TopSecret;Pooling=False;Timeout=15; CommandTimeout=20;";
			// return DB.Abstraction.cDAL.CreateInstance("PostGreSQL", strConnectionString);

            DB.Abstraction.UniversalConnectionStringBuilder csb = null;
            // csb = SetupMs();
            csb = SetupPg();

            return DB.Abstraction.cDAL.CreateInstance(csb.EngineName, csb.ConnectionString);
        }


        public static DB.Abstraction.UniversalConnectionStringBuilder SetupMs()
        {
            DB.Abstraction.UniversalConnectionStringBuilder csb =
                DB.Abstraction.UniversalConnectionStringBuilder.CreateInstance(DB.Abstraction.cDAL.DataBaseEngine_t.MS_SQL);

            csb.IntegratedSecurity = true;
            csb.Server = System.Environment.MachineName;
            csb.DataBase = "WorldDb";
            if (!csb.IntegratedSecurity)
            {
                csb.UserName = "";
                csb.Password = "";
            }

            csb.PacketSize = 4096;
            csb.Pooling = false;
            csb.MaxPoolSize = 5;

            return csb;
        }


        public static DB.Abstraction.UniversalConnectionStringBuilder SetupPg()
        {
            DB.Abstraction.UniversalConnectionStringBuilder csb =
                DB.Abstraction.UniversalConnectionStringBuilder.CreateInstance(DB.Abstraction.cDAL.DataBaseEngine_t.PostGreSQL);

            csb.Server = "127.0.0.1";
            csb.Port = 5432;
            csb.DataBase = "lsmail";
            csb.UserName = "postgres";
            csb.Password = "TopSecret";
            csb.Pooling = false;
            csb.ConnectTimeout = 15;
            csb.CommandTimeout = 20;

            // string strConnectionString = "Server=127.0.0.1;Port=5432;Database=lsmail;User ID=postgres;Password=TopSecret;Pooling=False;Timeout=15; CommandTimeout=20;";

            return csb;
        }


        public static DB.Abstraction.cDAL DAL = SetupDAL();


        public IntermediateTable()
        { 
        }


        public IntermediateTable(string TableName)
        {
            this.Name = TableName;

			System.Data.DataTable dt = DAL.GetAllColumnInformation(TableName);
            System.Console.WriteLine(dt.Rows.Count);

            for (int i = 0; i < dt.Rows.Count; ++i)
            {
				IntermediateDefinition idl = IntermediateDefinition.MS_2_Idl(dt.Rows[i]);
                //IntermediateDefinition idl = IntermediateDefinition.PG_2_Idl(dt.Rows[i]);

				this.Columns.Add(idl);
            } // Next i

        } // End Constructor


    } // End Class IntermediateTable 


    public class IntermediateDefinition
    {


        public enum datatype_t : int
        {
			array,
            integer,
            uid,
            boolean,
            binary,
            varbinary,
            @floating,
            @decimal,
            vartext,
            fixtext,
            variant,
            datetime,
            date,
            time,
            timespan,
            timestamp,
            xml,
            unknown,
        } // End Enum datatype_t


        public string column_name;
        public int ordinal_position;
		public string native_type;
		public string udt_native_type;
        public datatype_t type;
        public bool nullable;
        public int length;
        public int precision;
        public int scale;
        public bool maxSize;
		public bool withTimezone;
		public bool isSerial;
        public bool isMoney;
		public bool isNumeric;
		public bool isRealBit;
		public bool isBpChar;
        public bool signed;
        public bool unsigned;


        public System.Text.Encoding encoding;
        public string defaultValue;


		public static string idl_2_pg(IntermediateDefinition idl)
		{
			System.Collections.Generic.Dictionary<datatype_t, string> dict
			= new System.Collections.Generic.Dictionary<datatype_t, string>()
			;

			dict.Add(datatype_t.datetime, "timestamp without time zone");
			dict.Add(datatype_t.date, "date");
			//dict.Add(datatype_t.time, "time with time zone");
			dict.Add(datatype_t.time, "time without time zone");
			dict.Add(datatype_t.timespan, "tinterval");
			dict.Add(datatype_t.timestamp, "timestamp without timezone");


			dict.Add(datatype_t.fixtext, "national character");
			dict.Add(datatype_t.vartext, "national character varying");

			dict.Add(datatype_t.uid, "uuid");
			dict.Add(datatype_t.integer, "integer");

			dict.Add(datatype_t.floating, "double precision");
			dict.Add(datatype_t.@decimal, "decimal");

			dict.Add(datatype_t.boolean, "boolean");
			dict.Add(datatype_t.variant, "Aaaaaaaaaaaaaaaargh! Variant not supported");
			dict.Add(datatype_t.xml, "xml");

			dict.Add(datatype_t.binary, "bit");
			dict.Add(datatype_t.varbinary, "bytea");


			dict.Add(datatype_t.array, "array");
			dict.Add(datatype_t.unknown, "Aaaaaaaaaaaaaaaargh! IDL-type unknown");


			string datatype = null;

			if(dict.ContainsKey(idl.type))
				datatype = dict[idl.type];
			else
			{
				string.Format("The type \"{0}\" is not defined.", idl.type.ToString());
			}


			if (idl.type == datatype_t.integer)
			{

				if (idl.isSerial)
				{
					if (idl.precision > 32)
						datatype = "bigserial";
					else
						datatype = "serial";
				}
				else
				{
					if (idl.precision > 32 && idl.precision < 65)
						datatype = "bigint";

					if (idl.precision < 17)
						datatype = "smallint";
				}

			}
			else if (idl.type == datatype_t.floating)
			{ 
				if (idl.precision == 24)
					datatype = "real";
			}
			else if (idl.type == datatype_t.@decimal)
			{
				if (idl.isNumeric)
					datatype = "numeric";

				datatype = string.Format("{0}({1},{2})", datatype, idl.precision, idl.scale);
			}
			else if (idl.type == datatype_t.vartext)
			{
				if (idl.maxSize)
				{
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = "national text";
					else
						datatype = "text";
				}
				else
				{
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = string.Format("{0}({1})", datatype, idl.length);
					else
						datatype = string.Format("{0}({1})", "character varying", idl.length);
				}

			}
			else if (idl.type == datatype_t.fixtext)
			{
				if(idl.length > 0)
				{
					// todo length > max for db ?
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = string.Format("{0}({1})", datatype, idl.length);
					else
						datatype = string.Format("{0}({1})", "character", idl.length);
				}
				else
				{
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = string.Format("{0}", datatype);
					else
						datatype = string.Format("{0}", "character");
				}
			}
			else if (idl.type == datatype_t.boolean)
			{
				if(idl.isRealBit)
					datatype = "bit";
			}
			else if (idl.type == datatype_t.time)
			{
				if(idl.withTimezone)
					datatype = "time with time zone";
			}
			else if (idl.type == datatype_t.timestamp)
			{
				if(idl.withTimezone)
					datatype = "timestamp with time zone";
			}

			if (idl.nullable)
				datatype = string.Format("{0} {1}", datatype, "NULL");
			else
				datatype = string.Format("{0} {1}", datatype, "NOT NULL");

			if(idl.ordinal_position == 1)
				datatype = string.Format("{0} {1}", idl.column_name, datatype);
			else
				datatype = string.Format(",{0} {1}", idl.column_name, datatype);


			if(idl.type == datatype_t.variant || idl.type == datatype_t.unknown)
				datatype = "-- " + datatype;

			return datatype;
		} // End Function idl_2_pg


		public static string idl_2_ms(IntermediateDefinition idl)
		{
			System.Collections.Generic.Dictionary<datatype_t, string> dict
			= new System.Collections.Generic.Dictionary<datatype_t, string>()
			;

			dict.Add(datatype_t.datetime, "datetime2");
			dict.Add(datatype_t.date, "date");
			//dict.Add(datatype_t.time, "time with time zone");
			dict.Add(datatype_t.time, "time");
			dict.Add(datatype_t.timespan, "datetimeoffset");
			dict.Add(datatype_t.timestamp, "timestamp");


			dict.Add(datatype_t.fixtext, "national character");
			dict.Add(datatype_t.vartext, "national character varying");

			dict.Add(datatype_t.uid, "uniqueidentifier");
			dict.Add(datatype_t.integer, "integer");

			dict.Add(datatype_t.floating, "float");
			dict.Add(datatype_t.@decimal, "decimal");

			dict.Add(datatype_t.boolean, "bit");
			dict.Add(datatype_t.variant, "sql_variant");
			dict.Add(datatype_t.xml, "xml");

			dict.Add(datatype_t.binary, "bit");
			dict.Add(datatype_t.varbinary, "varbinary");


			dict.Add(datatype_t.array, "Aaaaaaaaaaaaaaaargh! Arrays not supported");
			dict.Add(datatype_t.unknown, "Aaaaaaaaaaaaaaaargh! IDL-type unknown");


			string datatype = null;

			if(dict.ContainsKey(idl.type))
				datatype = dict[idl.type];
			else
			{
				datatype = string.Format("The type \"{0}\" is not defined.", idl.type.ToString());
			}


			if (idl.type == datatype_t.integer)
			{

				if (idl.isSerial)
				{
					if (idl.precision > 32)
						datatype = "bigint";
					else
						datatype = "integer";
				}
				else
				{
					if (idl.precision > 32 && idl.precision < 65)
						datatype = "bigint";

					if (idl.precision < 17)
						datatype = "smallint";

					if (idl.precision < 9)
						datatype = "tinyint";
				}

			}
			else if (idl.type == datatype_t.floating)
			{ 
				if (idl.precision == 24)
					datatype = "real";
			}
			else if (idl.type == datatype_t.@decimal)
			{
				if (idl.isNumeric)
					datatype = "numeric";

				datatype = string.Format("{0}({1},{2})", datatype, idl.precision, idl.scale);
			}
			else if (idl.type == datatype_t.vartext)
			{
				if (idl.maxSize)
				{
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = "national text";
					else
						datatype = "text";
				}
				else
				{
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = string.Format("{0}({1})", datatype, idl.length);
					else
						datatype = string.Format("{0}({1})", "character varying", idl.length);
				}

			}
			else if (idl.type == datatype_t.fixtext)
			{
				if(idl.length > 0)
				{
					// todo length > max for db ?
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = string.Format("{0}({1})", datatype, idl.length);
					else
						datatype = string.Format("{0}({1})", "character", idl.length);
				}
				else
				{
					if (idl.encoding == System.Text.Encoding.UTF8)
						datatype = string.Format("{0}", datatype);
					else
						datatype = string.Format("{0}", "character");
				}
			}
			else if (idl.type == datatype_t.boolean)
			{
				if(idl.isRealBit)
					datatype = "bit";
			}
			else if (idl.type == datatype_t.time)
			{
				//if(idl.withTimezone) datatype = "time with time zone";
			}
			else if (idl.type == datatype_t.timestamp)
			{
				//if(idl.withTimezone) datatype = "timestamp with time zone";
			}

			if (idl.nullable)
				datatype = string.Format("{0} {1}", datatype, "NULL");
			else
				datatype = string.Format("{0} {1}", datatype, "NOT NULL");

			if(idl.ordinal_position == 1)
				datatype = string.Format("{0} {1}", idl.column_name, datatype);
			else
				datatype = string.Format(",{0} {1}", idl.column_name, datatype);


			if(idl.type == datatype_t.array || idl.type == datatype_t.unknown)
				datatype = " -- " + datatype;
			
			return datatype;
		} // End Function idl_2_ms


		public static IntermediateDefinition PG_2_Idl(System.Data.DataRow dr)
		{
			IntermediateDefinition idl = new IntermediateDefinition();
			idl.column_name = (string)dr["column_name"];
			idl.ordinal_position = (int)dr["ordinal_position"];
			idl.native_type = (string)dr["data_type"];

			string strNullable = System.Convert.ToString(dr["is_nullable"]);

            idl.nullable = System.StringComparer.InvariantCultureIgnoreCase.Equals(strNullable, "yes");
			idl.signed = true;
			idl.unsigned = false;
			idl.defaultValue = System.Convert.ToString(dr["column_default"]);

			idl.udt_native_type = "";


			if (dr["character_maximum_length"] != System.DBNull.Value)
				idl.length = System.Convert.ToInt32(dr["character_maximum_length"]);

			if (dr["numeric_precision"] != System.DBNull.Value)
				idl.precision = System.Convert.ToInt32(dr["numeric_precision"]);

			if (dr["numeric_scale"] != System.DBNull.Value)
				idl.scale = System.Convert.ToInt32(dr["numeric_scale"]);



			System.Collections.Generic.Dictionary<string, datatype_t> dict
				= new System.Collections.Generic.Dictionary<string, datatype_t>
                      (System.StringComparer.InvariantCultureIgnoreCase)
			;


			dict.Add("timestamp", datatype_t.timestamp);
			dict.Add("timestamp without time zone", datatype_t.datetime);
			dict.Add("timestamp with timezone", datatype_t.datetime);
			dict.Add("date", datatype_t.date);
			dict.Add("time with time zone", datatype_t.time);
			dict.Add("time without time zone", datatype_t.time);
			dict.Add("tinterval", datatype_t.timespan);


			dict.Add("double precision", datatype_t.floating); // numeric_precision: 53
			dict.Add("real", datatype_t.floating); // numeric_precision: 24
			dict.Add("decimal", datatype_t.@decimal); // standard's datatype 
			dict.Add("numeric", datatype_t.@decimal); // legacy datatype from Sybase
			// In the SQL-92/SQL2003 standard as quoted at stackoverflow.com/a/759606/14731. 
			// decimal is at least as precise as declared, whereas numeric is exactly as precise as declared
			// In SQL Server both are exactly as precise as declared

			// dict.Add("smallmoney", datatype_t.@decimal);
			dict.Add("money", datatype_t.@decimal);

			dict.Add("boolean", datatype_t.boolean);
			dict.Add("bit", datatype_t.boolean); // uuuuh

			//dict.Add("sql_variant", datatype_t.variant);
			dict.Add("xml", datatype_t.xml);

			dict.Add("uuid", datatype_t.uid);

			dict.Add("tinyint", datatype_t.integer);
			dict.Add("smallint", datatype_t.integer);
			dict.Add("int", datatype_t.integer);
			dict.Add("integer", datatype_t.integer);
			dict.Add("bigint", datatype_t.integer);



			dict.Add("char", datatype_t.fixtext);
			dict.Add("character", datatype_t.fixtext);
			//dict.Add("nchar", datatype_t.fixtext);
			dict.Add("\"char\"", datatype_t.fixtext);

			dict.Add("varchar", datatype_t.vartext);
			dict.Add("character varying", datatype_t.vartext);

			//dict.Add("nvarchar", datatype_t.vartext);

			dict.Add("text", datatype_t.vartext);
			dict.Add("ntext", datatype_t.vartext);


			dict.Add("binary", datatype_t.binary);
			dict.Add("bytea", datatype_t.varbinary);
			//dict.Add("image", datatype_t.varbinary);

			dict.Add("array", datatype_t.array);


			if (dict.ContainsKey(idl.native_type))
				idl.type = dict[idl.native_type];
			else
				idl.type = datatype_t.unknown;


			if (idl.type == datatype_t.vartext || idl.type == datatype_t.fixtext)
			{
				idl.encoding = System.Text.Encoding.UTF8;

				if (idl.length > 4000 || idl.length < 0)
					idl.maxSize = true;
			}
			if (idl.type == datatype_t.@decimal)
			{
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(idl.native_type, "numeric"))
					idl.isNumeric = true;
			}
			if (idl.type == datatype_t.boolean)
			{
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(idl.native_type, "bit"))
					idl.isRealBit = true;
			}
			if (idl.type == datatype_t.time)
			{
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(idl.native_type, "time with time zone"))
					idl.withTimezone = true;
			}
			if (idl.type == datatype_t.timestamp)
			{
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(idl.native_type, "timestamp with time zone"))
					idl.withTimezone = true;
			}
			if (idl.type == datatype_t.array)
			{

				if(dr.Table.Columns.Contains("udt_name"))
				{
					idl.udt_native_type = System.Convert.ToString(dr["udt_name"]);
				}

			}

			return idl;
		} // End Function PG_2_Idl


        public static IntermediateDefinition MS_2_Idl(System.Data.DataRow dr)
        {
            IntermediateDefinition idl = new IntermediateDefinition();
            idl.column_name = (string)dr["column_name"];
            idl.ordinal_position = (int)dr["ordinal_position"];
            idl.native_type = (string)dr["data_type"];

            string strNullable = System.Convert.ToString(dr["is_nullable"]);

            idl.nullable = System.StringComparer.InvariantCultureIgnoreCase.Equals(strNullable, "yes");
            idl.signed = true;
            idl.unsigned = false;
            idl.defaultValue = System.Convert.ToString(dr["column_default"]);

            if (dr["character_maximum_length"] != System.DBNull.Value)
                idl.length = System.Convert.ToInt32(dr["character_maximum_length"]);

            if (dr["numeric_precision"] != System.DBNull.Value)
                idl.precision = System.Convert.ToInt32(dr["numeric_precision"]);

            if (dr["numeric_scale"] != System.DBNull.Value)
                idl.scale = System.Convert.ToInt32(dr["numeric_scale"]);
                


            System.Collections.Generic.Dictionary<string, datatype_t> dict
            = new System.Collections.Generic.Dictionary<string, datatype_t>
            (System.StringComparer.InvariantCultureIgnoreCase);

            dict.Add("smalldatetime", datatype_t.datetime);
            dict.Add("datetime", datatype_t.datetime);
            dict.Add("datetime2", datatype_t.datetime);
            dict.Add("date", datatype_t.date);
            dict.Add("time", datatype_t.time);
            dict.Add("datetimeoffset", datatype_t.timespan);
            dict.Add("timestamp", datatype_t.timestamp);


            dict.Add("float", datatype_t.floating); // numeric_precision: 53
            dict.Add("real", datatype_t.floating); // numeric_precision: 24
            dict.Add("decimal", datatype_t.@decimal); // standard's datatype 
            dict.Add("numeric", datatype_t.@decimal); // legacy datatype from Sybase
            // In the SQL-92/SQL2003 standard as quoted at stackoverflow.com/a/759606/14731. 
            // decimal is at least as precise as declared, whereas numeric is exactly as precise as declared
            // In SQL Server both are exactly as precise as declared

            dict.Add("smallmoney", datatype_t.@decimal);
            dict.Add("money", datatype_t.@decimal);

            dict.Add("bit", datatype_t.boolean);
            dict.Add("sql_variant", datatype_t.variant);
            dict.Add("xml", datatype_t.xml);

            dict.Add("uniqueidentifier", datatype_t.uid);

            dict.Add("tinyint", datatype_t.integer);
            dict.Add("smallint", datatype_t.integer);
            dict.Add("int", datatype_t.integer);
            dict.Add("integer", datatype_t.integer);
            dict.Add("bigint", datatype_t.integer);



            dict.Add("char", datatype_t.fixtext);
            dict.Add("nchar", datatype_t.fixtext);

            dict.Add("varchar", datatype_t.vartext);
            dict.Add("nvarchar", datatype_t.vartext);

            dict.Add("text", datatype_t.vartext);
            dict.Add("ntext", datatype_t.vartext);


            dict.Add("binary", datatype_t.binary);
            dict.Add("varbinary", datatype_t.varbinary);
            dict.Add("image", datatype_t.varbinary);



            if (dict.ContainsKey(idl.native_type))
                idl.type = dict[idl.native_type];
            else
                idl.type = datatype_t.unknown;


            if (idl.type == datatype_t.vartext || idl.type == datatype_t.fixtext)
            {
                // nvarchar, ntext
                if (idl.native_type.StartsWith("n", System.StringComparison.InvariantCultureIgnoreCase))
                {
                    idl.encoding = System.Text.Encoding.UTF8;
                    if (idl.length > 4000 || idl.length < 0)
                        idl.maxSize = true;
                }
                else // varchar, text
                {
                    idl.encoding = System.Text.Encoding.ASCII;
                    if (idl.length > 8000 || idl.length < 0)
                        idl.maxSize = true;
                }

            }
            if (idl.type == datatype_t.@decimal)
            {
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(idl.native_type, "numeric"))
                    idl.isNumeric = true;
            }
            if (idl.type == datatype_t.integer)
            {
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(idl.native_type, "int"))
                    idl.precision = 32;

                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(idl.native_type, "bigint"))
                    idl.precision = 64;
            }
            
            return idl;
        } // End Function MS_2_Idl


    } // End  Class IntermediateDefinition 


} // End namespace DbTransform 
