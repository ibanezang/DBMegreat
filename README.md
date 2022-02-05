This is a new explanation.

# DB Megreat
DB Megreat designed to be a very simplistic database migration tools with a simple json configuration file. DB megreat is created as a CLI command so you can integrate DB Megreat to your build pipeline to make sure your that database always up-to-date.

## How to use?
You just need to create a configuration file with any name you like. For example create a file `db-megreat.json` with this content:

```
{
    "sql_files_directories": [
        "/your/directory/contains/sql",
        "../another/directory/contains/sql"
    ],
    "db_connection": {
        "type": "mysql",
        "connection_string": "Server=HOST_NAME;Database=DB_NAME;Uid=USER_ID;Pwd=PASSWORD"
    },
    "log_output_directory": "../directory/output"
}
```

And then we pass it to the command line argument of DB Megreat:
```
$ megreat db-megreat.json
```

DB Megreat will use `db_connection` to connect to the database and scan all directories in the `sql_files_directories` for `*.sql` files and execute them once. If you try execute again, it will only excute the newly created file. 

### Note:
* Mandatory configuration fields are: `sql_files_directories`, `db_connection`, `db_connection.type`, `db_connection.connection_string`. 
* If `log_output` provided, DB Megreat will write logs to a file. Otherwise, it will only print the output in the CLI.
* DB Megreat executes the sql scripts based on the file naming order. The best practice for a consistent ordering, you can give a number for your file for example `00001_your_file_name.sql`, `00002_another_file_name.sql`, etc.
* Make sure the database user being used has the privileges to manipulate your database. Otherwise, you might encounter some errors when executing your scripts.

## Supported Database Connection Type

| Type | Description |
|--|--|
|mysql| MySQL database|
|sqlserver| MS SQL Server database|

## Technical Detail
DB Megreat will create a new table called `db_megreat_tracks` in your database. It keeps track all executed `.sql` scripts and execution time. The key used to identify a unique scripts is the `<filename>.sql`. The log file will tell the about the success or failure execution. If DB Megreat fails to execute a file, it will stop the entire operation and only make the records of the success executions in the `db_megreat_tracks` table. 
