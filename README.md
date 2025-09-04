# Student-Performance-Tracker

This project is an ASP.NET Core application for tracking student performance.

## Local Development Setup

This project uses the .NET User Secrets feature to store the database connection string for local development. This keeps our credentials out of the git repository.

After cloning the repository, each team member must run the following command in their terminal from the project's root directory.

**Note:** You will need to get the actual Supabase connection string from your team lead and paste it in place of the `<Your-Supabase-Connection-String>` placeholder.

```sh
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<Your-Supabase-Connection-String>"
```

After running this command with the correct connection string, you will be able to run the project successfully.
