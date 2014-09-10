
SELECT 
	-- table_catalog
	--,table_schema
	 table_name
	,column_name
	,ordinal_position
	,column_default
	,is_nullable
	,data_type
	,character_maximum_length
	,character_octet_length
	,numeric_precision
	,numeric_precision_radix
	,numeric_scale
	,datetime_precision
	,character_set_catalog
	,character_set_schema
	,character_set_name
	,collation_catalog
	,collation_schema
	,collation_name
	,domain_catalog
	,domain_schema
	,domain_name
FROM information_schema.columns
WHERE (1=1) 
AND TABLE_NAME = '__types'
--AND data_type IN ('numeric', 'decimal') 
/*
-- Basic-types
AND  data_type not in 
(
  'char', 'varchar', 'text', 'nchar', 'nvarchar', 'ntext'
, 'tinyint', 'smallint', 'int', 'bigint'
, 'uniqueidentifier'
, 'date', 'datetime'
, 'bit'
, 'float', 'real', 'decimal', 'numeric'
, 'varbinary'
, 'image'
, 'sql_variant'
, 'xml' -- reportserver
)
*/ 

ORDER BY ordinal_position 