-- Aggregate Functions

count(*)
count([all | distinct] expr)
max(expr)
median(expr)
min(expr)
stddev(expr)
sum(expr)
variance(expr)

-- Character Functions

chr(n)
concat(char1, char2)
initcap(char)
lower(char)
lpad(expr1, n, expr2)
ltrim(char, set)
nls_initcap(char, 'NLS_SORT=lang')
nls_lower(char, 'NLS_SORT=lang')
nls_upper(char, 'NLS_SORT=lang')
nlssort(char, 'NLS_SORT=lang')
rpad(expr1, n, expr2)
rtrim(char, set)
soundex(char)
substr(char, position, substring_length)
translate(expr, from_string, to_string)
trim([leading|trailing|both] trim_char)
upper(char)

-- Conversion Functions

to_char(nchar | clob | nclob)
to_char(datetime, fmt, nlsparam)
TO_CHAR(systimestamp, 'DD-MON-YYYY HH24:MI:SS TZH:TZM')
to_char(n, fmt, nlsparam)
to_clob(lob_column | char)
to_date(datetime, fmt, nlsparam)
to_timestamp(char, ftm, nlsparam)
nvl(value, defualt)

-- Date/Time Functions

add_months(date, integer)
current_date
current_timestamp
sysdate
systimestamp
to_char(datetime, fmt, nlsparam)
to_char(sysdate, 'DD-MON-YYYY HH24:MI:SS')
to_timestamp(char, fmt, nlsparam)
trunc(date, fmt)
months_between(date, date)

-- Numeric Functions

abs(n)
ceil(n)
exp(n)
floor(n)
log(n2, n1)
mod(n2, n1)
power(n2, n1)
remainder(n2, n1)
round(n, integer)
trunc(n1, n2)

