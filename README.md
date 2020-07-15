# DB Megreat
DB Megreat designed to be a very simplistic database migration tools with a simple json configuration file.

## configuration file
You just need to create a configuration file with any name we like. For example create a file `db-megreat.json` with this content:

```
{
    "sql_files_directories": [
        "/your/directory/contains/sql",
        "../another/directory/contains/sql",
    ],
    "db_connection": {
        "type": "mysql",
        "connection_string": "Server=HOST_NAME;Database=DB_NAME;Uid=USER_ID;Pwd=PASSWORD"
    },
    "log_output": "../directory/output",
}
```

And then we pass it to the command line argument of DB Megreat:
```
$ megreat db-megreat.json
```

DB Megreat will scan all directories in the `sql_files_directories` for `*.sql` files and execute them once. 

## Technical Detail
DB Megreat will create a new table called `db_megreat_tracks` in your database. It keeps track all executed `.sql` scripts and when. The key used to identify a unique scripts is the `directory+filename`. 
