
SELECT [ hint ] [ { DISTINCT | UNIQUE } | ALL ]
	{ * 
	| { query_name.*
	| [schema.] { table | view | materialized view } .* 
	| expr [[AS] c_alias]
	} 
	[, { query_name.*
		| [schema.] { table | view | materialized view } .* 
		| expr [[AS] c_alias]
		}
	]...
	}
FROM
	{ ONLY query_table_expression [flashback_clause]
	| query_table_expression [flashback_clause] [t_alias]
	| ( joined_table )
	|   { [join_type] JOIN table_reference 
		  { ON condition | USING ( column [, column]... ) }
		  | { CROSS JOIN | NATURAL [join_type] JOIN table_reference }
		}
	} [, ... ]
[ WHERE condition ]
[ hierarchical_query_clause ]
[ group_by_clause ]
[ HAVING condition ]  
[ { UNION | UNION ALL | INTERSECT | MINUS } ( subquery )] 
[ order_by_clause ]
