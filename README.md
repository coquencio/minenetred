# Minenetred

App which allows logging time entries in multiple projects and dates into redmine.

## Before running

Run the database migrations in package manager console.
```bash
update-database
```
Add user secret key in your secrets.json file which can be opened by right clicking "Minenetred.web" project and then selecting the "Manage user secrets" option.
Feel free to write whatever string you want in "EncriptionKey" value.
```bash
{
  "EncryptionKey": "Any string you want"
}
```
