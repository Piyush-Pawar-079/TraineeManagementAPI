## Project Name
Trainee Management API
 
## Technology Used
.Net, Asp.Net core, EF core
 
## How to Run
run `dotnet run` in the root of the project directory
 
## API List
- GET /api/health

- GET /api/trainees
- GET /api/trainees/{id}
- POST /api/trainees
- PUT /api/trainees/{id}
- DELETE /api/trainees/{id}
 
## Sample Request JSON

Sample POST /api/trainees request:
{
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyush.pawar@training.com",
  "techStack": "MERN",
  "status": "Active"
}
 
Sample PUT /api/trainees/1 request:
{  
  "status": "InActive"
}
 
## Sample Response JSON

Sample GET /api/health response:

{
  "status": "running",
  "application": "Trainee Management API",
  "timestamp": "2026-06-08T11:09:44.9139355+00:00"
}

Sample GET /api/trainees response:
[
  {
    "id": 1,
    "firstName": "Piyush",
    "lastName": "Pawar",
    "email": "piyush.pawar@training.com",
    "techStack": "MERN",
    "status": "Active"
    "createdDate": "2026-06-08T10:55:05.7288647+00:00",
    "updatedDate": "2026-06-08T10:55:05.7294876+00:00"
  }
]
 
Sample POST /api/trainees response:
{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyush.pawar@example.com",
  "techStack": "MERN",
  "status": "Active",
  "createDate": "2026-06-08T11:05:54.2587553+00:00",
  "updateDate": "2026-06-08T11:05:54.2587851+00:00"
}
 
Sample GET /api/trainees/{id} response:
{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyush.pawar@example.com",
  "techStack": "MERN",
  "status": "Active",
  "createDate": "2026-06-08T11:05:54.2587553+00:00",
  "updateDate": "2026-06-08T11:05:54.2587851+00:00"
}
 
Sample PUT /api/trainees/{id} response:
{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyush.pawar@example.com",
  "techStack": "MERN",
  "status": "Inactive",
  "createDate": "2026-06-08T11:05:54.2587553+00:00",
  "updateDate": "2026-06-08T11:06:48.8217489+00:00"
}


## Known Limitations
- No Authentication implemented.
- No real database used in the project, still using in-memory database.

## Database Setup Steps

- First import required packages and make sure all the packages are of the same version so that we do not get any version mismatch error.
- Update the Program.cs file for using the MySql database instead of In-memory datase.
- In appsettings.json add another entry for Connection string like this: 
  "ConnectionStrings": {
    "DefaultConnection": "server=localhost;port=3306;database=trainee_management_db;user=root;password=root;"
  },
- Run dotnet build to make sure there are no errors.
- Run the migration command => dotnet ef migrations add InitialCreate
- Once the migration is completed run this command to make tables in the database => dotnet ef database update
- Once ran successfully, the code and the database are in sync. We can test the connection by using swagger UI, try adding one entry using POST end point and see if it is shown in the datase or not.