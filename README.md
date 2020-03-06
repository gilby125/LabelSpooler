LabelSpooler
============

This program polls RAW data from a SQL Server database table and sends data to a printer.
It is useful to send RAW ZPL to Zebra printers, for example.  

Requirements
------------

- .NET Framework 4.0
- Working SQL Server Database

Installation
------------

- Grab the latest release file
- Extract the zip to a folder of your choice (for example c:\LabelSpooler\ ) 
- Create the LabelSpooler table in the database with file CreateTables.sql
- Create the file config.ini by copying the config-example.ini
- Install service from an elevated command prompt:

```
c:\LabelSpooler\LabelSpooler.exe --install
```

Uninstallation
--------------

- Uninstall service from an elevated command prompt:

```
c:\LabelSpooler\LabelSpooler.exe --uninstall
```

Spooling to the Database
------------------------

For spooling labels into database you have to populate two mandatory fields:

- *printerName:* System printer name, UNC printer path or alias defined in config.ini.
- *printData:* The RAW data. It can be ZPL code, ESC/POS, plaintext or whatever.

And you can set an optional field:

- *jobTag:* This is not mandatory, you can use it for your reference

Example
```
INSERT INTO LabelSpooler(printerName,printData,jobTag) VALUES ('PRINTER1','***LABELDATA***','USER:BOB;REFERENCE:101');
```

Database Field Explained
------------------------

| Field Name    | Data Type     | Description                                                         |
| ------------- | ------------- | ------------------------------------------------------------------- |
| jobID         | bigint        | Autoincrement identity column                                       |
| printerName   | varchar       | Printer Name                                                        |
| printData     | Text          | RAW Print data                                                      |
| jobStatus     | tinyint       | Job Status, 0: to be sent, 1: printed, 2: error                     |
| jobCreated    | datetime      | Timestamp of creation (populated automatically by SQL Server)       |
| jobRetries    | tinyint       | Number of retries                                                   |
| jobTag        | varchar       | Additional data you can set, for example for troubleshooting issues |
| jobLastSent   | datetime      | Timestamp                                                           |

Need Commercial Support?
------------------------

If you need commercial support you can send a mail to: r.bicelli@gmail.com.