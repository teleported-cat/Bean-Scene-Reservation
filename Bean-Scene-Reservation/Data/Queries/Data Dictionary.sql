-- ------------------------------------------------------
-- Generate a data dictionary for the current database --
-- ------------------------------------------------------

USE [BeanSceneReservationDB];
GO

SELECT
  t.TABLE_SCHEMA AS [Schema],
  t.TABLE_NAME AS [Table Name],
  c.COLUMN_NAME AS [Column Name],
  CASE 
    WHEN c.DATA_TYPE IN ('char', 'varchar', 'text', 'nchar', 'nvarchar', 'ntext')
      THEN c.DATA_TYPE + '(' + 
        CASE 
          WHEN c.CHARACTER_MAXIMUM_LENGTH = -1
            THEN 'MAX' -- Replace -1 with MAX
          WHEN c.CHARACTER_MAXIMUM_LENGTH IS NOT NULL
            THEN CAST(c.CHARACTER_MAXIMUM_LENGTH AS VARCHAR)
          ELSE 'MAX' 
        END + ')'
    WHEN c.DATA_TYPE IN ('decimal', 'numeric')
      THEN c.DATA_TYPE + '(' + 
        CAST(c.NUMERIC_PRECISION AS VARCHAR) + ',' + 
        CAST(c.NUMERIC_SCALE AS VARCHAR) + ')'
    ELSE c.DATA_TYPE
  END AS [Data Type],
  c.IS_NULLABLE AS [Nullable],
  c.COLUMN_DEFAULT AS [Default Value],
  CASE 
    WHEN pk.COLUMN_NAME IS NOT NULL THEN 'Yes'
    ELSE 'No'
  END AS [Is Primary Key],
  CASE 
    WHEN fk.COLUMN_NAME IS NOT NULL THEN 'Yes'
    ELSE 'No'
  END AS [Is Foreign Key],
  CASE 
    WHEN fk.COLUMN_NAME IS NOT NULL THEN pk.CONSTRAINT_NAME
    ELSE ''
  END AS [Foreign Key Constraint]
  -- pk.CONSTRAINT_NAME AS [Foreign Key Constraint]
FROM 
  INFORMATION_SCHEMA.TABLES t
JOIN 
  INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME
  AND t.TABLE_SCHEMA = c.TABLE_SCHEMA
LEFT JOIN 
  INFORMATION_SCHEMA.KEY_COLUMN_USAGE pk ON c.COLUMN_NAME = pk.COLUMN_NAME
  AND c.TABLE_NAME = pk.TABLE_NAME
  AND c.TABLE_SCHEMA = pk.TABLE_SCHEMA
LEFT JOIN 
  INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc ON pk.CONSTRAINT_NAME = rc.CONSTRAINT_NAME
LEFT JOIN 
  INFORMATION_SCHEMA.KEY_COLUMN_USAGE fk ON fk.CONSTRAINT_NAME = rc.UNIQUE_CONSTRAINT_NAME
ORDER BY 
  t.TABLE_SCHEMA,
  t.TABLE_NAME,
  c.ORDINAL_POSITION;