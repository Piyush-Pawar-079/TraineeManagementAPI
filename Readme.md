## Project Name
Trainee Management API
 
## Technology Used
.Net, Asp.Net core, EF core, MySQL, Docker, Redis, RabbitMQ
 
## How to Run
run `dotnet run` in the root of the project directory  

## Database Setup Steps

- First import required packages and make sure all the packages are of the same version so that we do not get any version mismatch error.
- Update the Program.cs file for using the MySql database instead of In-memory datase.
- In .env add entry for Connection string like this: 
    DefaultConnection=server=localhost;port=3306;database=trainee_management_db;user=root;password=root;
- Run dotnet build to make sure there are no errors.
- Run the migration command => dotnet ef migrations add InitialCreate
- Once the migration is completed run this command to make tables in the database => dotnet ef database update
- Once ran successfully, the code and the database are in sync. We can test the connection by using swagger UI, try adding one entry using POST end point and see if it is shown in the datase or not.

## EF Core migration commands

dotnet ef migrations add "MigrationName"
dotnet ef database update 

## Login Credentials for testing

username = "Admin"
password = "admin@12345"


### JWT Usage Instructions
- In the appsettings.json change the Key, Issuer and Audience values to change the JWT settings.


## API List
- GET    /api/health 

- POST   /api/auth/login 

- GET    /api/trainees?pageNumber=1&pageSize=10&search=amit&status=Active 
- GET    /api/trainees/{id} 
- POST   /api/trainees 
- PUT    /api/trainees/{id} 
- DELETE /api/trainees/{id} 

- GET    /api/mentors 
- GET    /api/mentors/{id} 
- POST   /api/mentors 
- PUT    /api/mentors/{id} 
- DELETE /api/mentors/{id} 

- GET    /api/learning-tasks 
- GET    /api/learning-tasks/{id} 
- POST   /api/learning-tasks 
- PUT    /api/learning-tasks/{id} 
- DELETE /api/learning-tasks/{id} 

- POST   /api/task-assignments 
- GET    /api/task-assignments 
- GET    /api/task-assignments/{id} 
- PUT    /api/task-assignments/{id}/status 

- POST   /api/submissions 
- GET    /api/submissions 
- GET    /api/submissions/{id}
- POST   /api/submissions/{id}/files 
- GET    /api/submissions/{id}/summary

- POST   /api/reviews 
- GET    /api/reviews 
- GET    /api/reviews/{id}

- GET    /api/submission-files/{id}
- GET    /api/submission-files/{id}/download
- DELETE /api/submission-files/{id}

- GET    /api/processing-jobs/{id}

- GET    /health/live
- GET    /health/ready

## Sample Request JSON

Sample POST /login request: 
{
  "username": "Admin",
  "password": "admin@12345"
}

Sample POST /api/trainees request:
{
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyushpawar@example.com",
  "techStack": ".net",
  "status": "Active"
}
 
Sample PUT /api/trainees/{1} request:
{  
  "status": "InActive"
}

Sample POST /api/mentor request:
{
  "firstName": "Om",
  "lastName": "Deshmukh",
  "email": "omdeshmukh@example.com",
  "expertise": "Java Spring Boot",
  "status": "Active"
}

Sample PUT /api/mentor/{1} request:
{
  "status": "Inactive"
}

Sample POST /api/learning-tasks request:
{
  "title": "Trainee Management API",
  "description": "Building a backend for trainee management",
  "expectedTechStack": ".net, asp.net core, ef core",
  "dueDate": "2026-06-15T10:03:59.092Z",
  "status": "Draft"
}

Sample PUT /api/learning-tasks request:
{
  "status": "Published"
}

Sample POST /api/task-assignment request:
{
  "traineeId": 1,
  "mentorId": 1,
  "learningTaskId": 1,
  "assignedDate": "2026-06-29T13:06:29.026Z",
  "dueDate": "2026-06-29T13:06:29.026Z",
  "status": "Assigned",
  "remarks": "string"
}

Sample PUT /api/task-assignment/{1}/status request:
{
  "status": "InProgress"
}

Sample POST /api/submissions request:
{
  "taskAssignmentId": 2,
  "submissionUrl": "http://github.com",
  "notes": "Something",
  "submittedDate": "2026-06-29T13:09:10.510Z",
  "status": "Submitted"
}

Sample POST /api/reviews request:
{
  "submissionId": 2,
  "mentorId": 1,
  "feedback": "Something",
  "score": 10,
  "reviewStatus": "Accepted",
  "reviewedDate": "2026-06-29T13:37:02.337Z"
}

Sample POST /api/submission/{id}/files
{
  "submissionId": 1,
  "File": select from the file explorer
}

Sample DELETE /api/submission-files/{id}
{
  "submissionFileId": 1
}

 
## Sample Response JSON

Sample POST /login reponse: 
{
  "token": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbkBleGFtcGxlLmNvbSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNzgxNTE4MTEzLCJpc3MiOiJUcmFpbmVlTWFuYWdlbWVudEFwaSIsImF1ZCI6IlRyYWluZWVNYW5hZ2VtZW50Q2xpZW50In0.JKKljH2C9nOZhVVy3pmN4o0cXBqu7xkztine7XdG1Mgo6QR07K8B3WYw1NIah0VvsCvC0doOJ1TgHgMIvD42kA",
  "expiresIn": "2026-06-15T10:08:33.23547Z",
  "user": {
    "id": 1,
    "username": "Admin",
    "email": "admin@example.com",
    "role": "Admin",
    "createdDate": "2026-06-15T06:35:45.412859",
    "updatedDate": "2026-06-15T06:35:45.412884"
  }
}

Sample GET /api/health response:

{
  "status": "running",
  "application": "Trainee Management API",
  "timestamp": "2026-06-08T11:09:44.9139355+00:00"
}

Sample GET /api/trainees?pageNumber=1&pageSize=10&search=piyush&status=Inactive response:

[
  {
    "id": 1,
    "firstName": "Piyush",
    "lastName": "Pawar",
    "email": "piyushpawar@example.com",
    "techStack": ".net",
    "status": "Inactive",
    "createdDate": "0001-01-01T00:00:00",
    "updatedDate": "2026-06-29T12:59:47.708905"
  }
]
 
Sample GET /api/trainees/{id} response:

{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyushpawar@example.com",
  "techStack": ".net",
  "status": "Inactive",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "2026-06-29T12:59:47.708905"
}
 
Sample POST /api/trainees response:
{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyushpawar@example.com",
  "techStack": ".net",
  "status": "Active",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "0001-01-01T00:00:00"
}
 
Sample PUT /api/trainees/{id} response:

{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyushpawar@example.com",
  "techStack": ".net",
  "status": "Inactive",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "2026-06-29T12:59:47.7089054Z"
}

Sample GET /api/mentor response:

[
  {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh",
    "email": "omdeshmukh@example.com",
    "expertise": "Java Spring Boot",
    "status": "Active",
    "createdDate": "0001-01-01T00:00:00",
    "updatedDate": "0001-01-01T00:00:00"
  }
]

Sample GET /api/mentor/{1} response:
{
  "id": 1,
  "firstName": "Om",
  "lastName": "Deshmukh",
  "email": "omdeshmukh@example.com",
  "expertise": "Java Spring Boot",
  "status": "Active",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "0001-01-01T00:00:00"
}

Sample POST /api/mentor response:

{
  "id": 1,
  "firstName": "Om",
  "lastName": "Deshmukh",
  "email": "omdeshmukh@example.com",
  "expertise": "Java Spring Boot",
  "status": "Active",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "0001-01-01T00:00:00"
}

Sample PUT /api/mentor/{1} response:

{
  "id": 1,
  "firstName": "Om",
  "lastName": "Deshmukh",
  "email": "omdeshmukh@example.com",
  "expertise": "Java Spring Boot",
  "status": "Active",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "0001-01-01T00:00:00"
}

Sample GET /api/learning-tasks response:

[
  {
    "id": 1,
    "title": "Trainee Management API",
    "description": "Building a backend for trainee management",
    "expectedTechStack": ".net, asp.net core, ef core",
    "dueDate": "2026-06-15T10:03:59.092",
    "status": "Draft",
    "createdDate": "0001-01-01T00:00:00",
    "updatedDate": "0001-01-01T00:00:00"
  }
]

Sample GET /api/learning-tasks/{1} response:
{
  "id": 1,
  "title": "Trainee Management API",
  "description": "Building a backend for trainee management",
  "expectedTechStack": ".net, asp.net core, ef core",
  "dueDate": "2026-06-15T10:03:59.092",
  "status": "Published",
  "taskAssignment": [],
  "createdDate": "2026-06-15T10:04:56.971705",
  "updatedDate": "2026-06-15T10:07:05.999999"
}

Sample POST /api/learning-tasks response:

{
  "id": 1,
  "title": "Trainee Management API",
  "description": "Building a backend for trainee management",
  "expectedTechStack": ".net, asp.net core, ef core",
  "dueDate": "2026-06-15T10:03:59.092Z",
  "status": "Draft",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "0001-01-01T00:00:00"
}

Sample PUT /api/learning-tasks/{1} response:

{
  "id": 1,
  "title": "Trainee Management API",
  "description": "Building a backend for trainee management",
  "expectedTechStack": ".net, asp.net core, ef core",
  "dueDate": "2026-06-15T10:03:59.092",
  "status": "Published",
  "createdDate": "0001-01-01T00:00:00",
  "updatedDate": "2026-06-29T13:05:43.813875Z"
}

Sample GET /api/task-assignment response:
[
  {
    "id": 1,
    "trainee": {
      "id": 1,
      "firstName": "Piyush",
      "lastName": "Pawar"
    },
    "mentor": {
      "id": 1,
      "firstName": "Om",
      "lastName": "Deshmukh"
    },
    "learningTask": {
      "id": 1,
      "title": "Trainee Management API",
      "description": "Building a backend for trainee management"
    },
    "assignedDate": "2026-06-29T13:06:29.026",
    "dueDate": "2026-06-29T13:06:29.026",
    "status": "Assigned",
    "remarks": "string"
  }
]

Sample GET /api/task-assignment/{1} response:
{
  "id": 1,
  "trainee": {
    "id": 1,
    "firstName": "Piyush",
    "lastName": "Pawar"
  },
  "mentor": {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh"
  },
  "learningTask": {
    "id": 1,
    "title": "Trainee Management API",
    "description": "Building a backend for trainee management"
  },
  "assignedDate": "2026-06-29T13:06:29.026",
  "dueDate": "2026-06-29T13:06:29.026",
  "status": "Assigned",
  "remarks": "string"
}

Sample POST /api/task-assignment response:
{
  "id": 1,
  "trainee": null,
  "mentor": null,
  "learningTask": null,
  "assignedDate": "2026-06-29T13:06:29.026Z",
  "dueDate": "2026-06-29T13:06:29.026Z",
  "status": "Assigned",
  "remarks": "string"
}

Sample PUT /api/task-assignment/{1}/status response:
{
  "id": 1,
  "trainee": {
    "id": 1,
    "firstName": "Piyush",
    "lastName": "Pawar"
  },
  "mentor": {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh"
  },
  "learningTask": {
    "id": 1,
    "title": "Trainee Management API",
    "description": "Building a backend for trainee management"
  },
  "assignedDate": "2026-06-29T13:06:29.026",
  "dueDate": "2026-06-29T13:06:29.026",
  "status": "InProgress",
  "remarks": "string"
}

Sample GET /api/submissions response:
[
  {
    "id": 2,
    "taskAssignmentId": 2,
    "taskAssignment": {
      "id": 2,
      "traineeName": null,
      "mentorName": null,
      "learningTaskTitle": null,
      "dueDate": "2026-06-29T13:06:29.026",
      "status": "Assigned"
    },
    "submissionUrl": "http://github.com",
    "notes": "Something",
    "submittedDate": "2026-06-29T13:09:10.51",
    "status": "Submitted"
  }
]


Sample GET /api/submissions/{1} response:
{
  "id": 2,
  "taskAssignmentId": 2,
  "taskAssignment": {
    "id": 2,
    "traineeName": null,
    "mentorName": null,
    "learningTaskTitle": null,
    "dueDate": "2026-06-29T13:06:29.026",
    "status": "Assigned"
  },
  "submissionUrl": "http://github.com",
  "notes": "Something",
  "submittedDate": "2026-06-29T13:09:10.51",
  "status": "Submitted"
}

Sample POST /api/submissions response: 
{
  "id": 2,
  "taskAssignmentId": 2,
  "taskAssignment": null,
  "submissionUrl": "http://github.com",
  "notes": "Something",
  "submittedDate": "2026-06-29T13:09:10.51Z",
  "status": "Submitted"
}

Sample GET /api/reviews response:
[
  {
    "id": 1,
    "submission": {
      "id": 2,
      "status": "Submitted"
    },
    "mentor": {
      "id": 1,
      "firstName": "Om",
      "lastName": "Deshmukh"
    },
    "feedback": "Something",
    "score": 10,
    "reviewStatus": "Accepted",
    "reviewedDate": "2026-06-29T13:37:02.337"
  }
]

Sample GET /api/reviews/{1} response:
{
  "id": 1,
  "submission": {
    "id": 2,
    "status": "Submitted"
  },
  "mentor": {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh"
  },
  "feedback": "Something",
  "score": 10,
  "reviewStatus": "Accepted",
  "reviewedDate": "2026-06-29T13:37:02.337"
}

Sample POST /api/reviews response:
{
  "id": 1,
  "submission": null,
  "mentor": null,
  "feedback": "Something",
  "score": 10,
  "reviewStatus": "Accepted",
  "reviewedDate": "2026-06-29T13:37:02.337Z"
}

Sample POST /api/submission/{id}/files response:
{
  "id": 1,
  "originalFileName": "Day_01.txt",
  "contentType": "text/plain",
  "correlationId": "17fc5349-4a22-40ab-bdd4-42333c827b27",
  "size": 222,
  "ownerName": "Piyush",
  "submissionId": 2,
  "createdAt": "2026-06-29T13:38:46.0811208Z"
}

## Docker compose start-up
- Run the following command

docker compose up -d 


## Known Limitations
- Scalability 
- Better architecture or schema design

## Security Checklist


## Next Improvement areas
- Improve the scalability of the api's
