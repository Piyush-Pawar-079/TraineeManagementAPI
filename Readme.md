## Project Name
Trainee Management API
 
## Technology Used
.Net, Asp.Net core, EF core
 
## How to Run
run `dotnet run` in the root of the project directory  

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

- POST   /api/reviews 
- GET    /api/reviews 
- GET    /api/reviews/{id}

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
  "assignedDate": "2026-06-15T10:08:57.407Z",
  "dueDate": "2026-06-15T10:08:57.407Z",
  "status": "Assigned",
  "remarks": ""
}

Sample PUT /api/task-assignment/{1}/status request:
{
  "status": "InProgress"
}

Sample POST /api/submissions request:
{
  "taskAssignmentId": 1,
  "submissionUrl": "https://localhost:9000",
  "notes": "Nothing",
  "submittedDate": "2026-06-15T10:14:07.178Z",
  "status": "Submitted"
}

Sample POST /api/reviews request:
{
  "submissionId": 1,
  "mentorId": 1,
  "feedback": "Good implementation",
  "score": 9/10,
  "reviewStatus": "Accepted",
  "reviewedDate": "2026-06-15T10:16:43.838Z"
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
    "taskAssignment": [],
    "createDate": "2026-06-15T09:56:05.391767",
    "updateDate": "2026-06-15T09:57:17.173395"
  }
]
 
Sample GET /api/trainees/{id} response:

[
  {
    "id": 1,
    "firstName": "Piyush",
    "lastName": "Pawar",
    "email": "piyushpawar@example.com",
    "techStack": ".net",
    "status": "Inactive",
    "taskAssignment": [],
    "createDate": "2026-06-15T09:56:05.391767",
    "updateDate": "2026-06-15T09:57:17.173395"
  }
]
 
Sample POST /api/trainees response:
{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyushpawar@example.com",
  "techStack": ".net",
  "status": "Active",
  "taskAssignment": [],
  "createDate": "2026-06-15T09:56:05.3917677Z",
  "updateDate": "2026-06-15T09:56:05.391789Z"
}
 
Sample PUT /api/trainees/{id} response:
{
  "id": 1,
  "firstName": "Piyush",
  "lastName": "Pawar",
  "email": "piyushpawar@example.com",
  "techStack": ".net",
  "status": "Inactive",
  "taskAssignment": [],
  "createDate": "2026-06-15T09:56:05.391767",
  "updateDate": "2026-06-15T09:57:17.1733952Z"
}

Sample GET /api/mentor response:
[
  {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh",
    "email": "omdeshmukh@example.com",
    "expertise": "Java Spring Boot",
    "status": "Inactive",
    "taskAssignments": [],
    "reviews": [],
    "createdDate": "2026-06-15T09:59:11.51592",
    "updatedDate": "2026-06-15T10:00:48.726323"
  }
]

Sample GET /api/mentor/{1} response:
{
  "id": 1,
  "firstName": "Om",
  "lastName": "Deshmukh",
  "email": "omdeshmukh@example.com",
  "expertise": "Java Spring Boot",
  "status": "Inactive",
  "taskAssignments": [],
  "reviews": [],
  "createdDate": "2026-06-15T09:59:11.51592",
  "updatedDate": "2026-06-15T10:00:48.726323"
}

Sample POST /api/mentor response:
{
  "id": 1,
  "firstName": "Om",
  "lastName": "Deshmukh",
  "email": "omdeshmukh@example.com",
  "expertise": "Java Spring Boot",
  "status": "Active",
  "taskAssignments": [],
  "reviews": [],
  "createdDate": "2026-06-15T09:59:11.51592Z",
  "updatedDate": "2026-06-15T09:59:11.5159939Z"
}

Sample PUT /api/mentor/{1} response:
{
  "id": 1,
  "firstName": "Om",
  "lastName": "Deshmukh",
  "email": "omdeshmukh@example.com",
  "expertise": "Java Spring Boot",
  "status": "Inactive",
  "taskAssignments": [],
  "reviews": [],
  "createdDate": "2026-06-15T09:59:11.51592",
  "updatedDate": "2026-06-15T10:00:48.7263236Z"
}

Sample GET /api/learning-tasks response:
[
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
  "taskAssignment": [],
  "createdDate": "2026-06-15T10:04:56.9717059Z",
  "updatedDate": "2026-06-15T10:04:56.971727Z"
}

Sample PUT /api/learning-tasks/{1} response:
{
  "id": 1,
  "title": "Trainee Management API",
  "description": "Building a backend for trainee management",
  "expectedTechStack": ".net, asp.net core, ef core",
  "dueDate": "2026-06-15T10:03:59.092",
  "status": "Published",
  "taskAssignment": [],
  "createdDate": "2026-06-15T10:04:56.971705",
  "updatedDate": "2026-06-15T10:07:05.9999997Z"
}

Sample GET /api/task-assignment response:
[
  {
    "id": 1,
    "traineeId": 1,
    "trainee": {
      "id": 1,
      "firstName": "Piyush",
      "lastName": "Pawar",
      "email": "piyushpawar@example.com",
      "techStack": ".net",
      "status": "Inactive",
      "createdDate": "2026-06-15T09:56:05.391767",
      "updatedDate": "2026-06-15T09:57:17.173395",
      "taskAssignments": [
        {
          "id": 1,
          "traineeId": 1,
          "mentorId": 1,
          "learningTaskId": 1,
          "submission": [],
          "assignedDate": "2026-06-15T10:08:57.407",
          "dueDate": "2026-06-15T10:08:57.407",
          "status": "InProgress",
          "remarks": ""
        }
      ]
    },
    "mentorId": 1,
    "mentor": {
      "id": 1,
      "firstName": "Om",
      "lastName": "Deshmukh",
      "email": "omdeshmukh@example.com",
      "expertise": "Java Spring Boot",
      "status": "Inactive",
      "createdDate": "2026-06-15T09:59:11.51592",
      "updatedDate": "2026-06-15T10:00:48.726323",
      "taskAssignments": [
        {
          "id": 1,
          "traineeId": 1,
          "mentorId": 1,
          "learningTaskId": 1,
          "submission": [],
          "assignedDate": "2026-06-15T10:08:57.407",
          "dueDate": "2026-06-15T10:08:57.407",
          "status": "InProgress",
          "remarks": ""
        }
      ],
      "reviews": []
    },
    "learningTaskId": 1,
    "learningTask": {
      "id": 1,
      "title": "Trainee Management API",
      "description": "Building a backend for trainee management",
      "expectedTechStack": ".net, asp.net core, ef core",
      "dueDate": "2026-06-15T10:03:59.092",
      "status": "Published",
      "createdDate": "2026-06-15T10:04:56.971705",
      "updatedDate": "2026-06-15T10:07:05.999999",
      "taskAssignments": [
        {
          "id": 1,
          "traineeId": 1,
          "mentorId": 1,
          "learningTaskId": 1,
          "submission": [],
          "assignedDate": "2026-06-15T10:08:57.407",
          "dueDate": "2026-06-15T10:08:57.407",
          "status": "InProgress",
          "remarks": ""
        }
      ]
    },
    "submission": [],
    "assignedDate": "2026-06-15T10:08:57.407",
    "dueDate": "2026-06-15T10:08:57.407",
    "status": "InProgress",
    "remarks": ""
  }
]

Sample GET /api/task-assignment/{1} response:
{
  "id": 1,
  "traineeId": 1,
  "trainee": {
    "id": 1,
    "firstName": "Piyush",
    "lastName": "Pawar",
    "email": "piyushpawar@example.com",
    "techStack": ".net",
    "status": "Inactive",
    "createdDate": "2026-06-15T09:56:05.391767",
    "updatedDate": "2026-06-15T09:57:17.173395",
    "taskAssignments": [
      {
        "id": 1,
        "traineeId": 1,
        "mentorId": 1,
        "learningTaskId": 1,
        "submission": [],
        "assignedDate": "2026-06-15T10:08:57.407",
        "dueDate": "2026-06-15T10:08:57.407",
        "status": "InProgress",
        "remarks": ""
      }
    ]
  },
  "mentorId": 1,
  "mentor": {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh",
    "email": "omdeshmukh@example.com",
    "expertise": "Java Spring Boot",
    "status": "Inactive",
    "createdDate": "2026-06-15T09:59:11.51592",
    "updatedDate": "2026-06-15T10:00:48.726323",
    "taskAssignments": [
      {
        "id": 1,
        "traineeId": 1,
        "mentorId": 1,
        "learningTaskId": 1,
        "submission": [],
        "assignedDate": "2026-06-15T10:08:57.407",
        "dueDate": "2026-06-15T10:08:57.407",
        "status": "InProgress",
        "remarks": ""
      }
    ],
    "reviews": []
  },
  "learningTaskId": 1,
  "learningTask": {
    "id": 1,
    "title": "Trainee Management API",
    "description": "Building a backend for trainee management",
    "expectedTechStack": ".net, asp.net core, ef core",
    "dueDate": "2026-06-15T10:03:59.092",
    "status": "Published",
    "createdDate": "2026-06-15T10:04:56.971705",
    "updatedDate": "2026-06-15T10:07:05.999999",
    "taskAssignments": [
      {
        "id": 1,
        "traineeId": 1,
        "mentorId": 1,
        "learningTaskId": 1,
        "submission": [],
        "assignedDate": "2026-06-15T10:08:57.407",
        "dueDate": "2026-06-15T10:08:57.407",
        "status": "InProgress",
        "remarks": ""
      }
    ]
  },
  "submission": [],
  "assignedDate": "2026-06-15T10:08:57.407",
  "dueDate": "2026-06-15T10:08:57.407",
  "status": "InProgress",
  "remarks": ""
}

Sample POST /api/task-assignment response:
{
  "id": 1,
  "traineeId": 1,
  "trainee": null,
  "mentorId": 1,
  "mentor": null,
  "learningTaskId": 1,
  "learningTask": null,
  "submission": [],
  "assignedDate": "2026-06-15T10:08:57.407Z",
  "dueDate": "2026-06-15T10:08:57.407Z",
  "status": "Assigned",
  "remarks": ""
}

Sample PUT /api/task-assignment/{1}/status response:
{
  "id": 1,
  "traineeId": 1,
  "trainee": {
    "id": 1,
    "firstName": "Piyush",
    "lastName": "Pawar",
    "email": "piyushpawar@example.com",
    "techStack": ".net",
    "status": "Inactive",
    "createdDate": "2026-06-15T09:56:05.391767",
    "updatedDate": "2026-06-15T09:57:17.173395",
    "taskAssignments": [
      {
        "id": 1,
        "traineeId": 1,
        "mentorId": 1,
        "learningTaskId": 1,
        "submission": [],
        "assignedDate": "2026-06-15T10:08:57.407",
        "dueDate": "2026-06-15T10:08:57.407",
        "status": "InProgress",
        "remarks": ""
      }
    ]
  },
  "mentorId": 1,
  "mentor": {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh",
    "email": "omdeshmukh@example.com",
    "expertise": "Java Spring Boot",
    "status": "Inactive",
    "createdDate": "2026-06-15T09:59:11.51592",
    "updatedDate": "2026-06-15T10:00:48.726323",
    "taskAssignments": [
      {
        "id": 1,
        "traineeId": 1,
        "mentorId": 1,
        "learningTaskId": 1,
        "submission": [],
        "assignedDate": "2026-06-15T10:08:57.407",
        "dueDate": "2026-06-15T10:08:57.407",
        "status": "InProgress",
        "remarks": ""
      }
    ],
    "reviews": []
  },
  "learningTaskId": 1,
  "learningTask": {
    "id": 1,
    "title": "Trainee Management API",
    "description": "Building a backend for trainee management",
    "expectedTechStack": ".net, asp.net core, ef core",
    "dueDate": "2026-06-15T10:03:59.092",
    "status": "Published",
    "createdDate": "2026-06-15T10:04:56.971705",
    "updatedDate": "2026-06-15T10:07:05.999999",
    "taskAssignments": [
      {
        "id": 1,
        "traineeId": 1,
        "mentorId": 1,
        "learningTaskId": 1,
        "submission": [],
        "assignedDate": "2026-06-15T10:08:57.407",
        "dueDate": "2026-06-15T10:08:57.407",
        "status": "InProgress",
        "remarks": ""
      }
    ]
  },
  "submission": [],
  "assignedDate": "2026-06-15T10:08:57.407",
  "dueDate": "2026-06-15T10:08:57.407",
  "status": "InProgress",
  "remarks": ""
}

Sample GET /api/submissions response:
[
  {
    "id": 1,
    "taskAssignmentId": 1,
    "taskAssignment": {
      "id": 1,
      "traineeId": 1,
      "mentorId": 1,
      "learningTaskId": 1,
      "submission": [
        {
          "id": 1,
          "taskAssignmentId": 1,
          "submissionUrl": "https://localhost:9000",
          "notes": "Nothing",
          "submittedDate": "2026-06-15T10:14:07.178",
          "status": "Submitted",
          "reviews": []
        }
      ],
      "assignedDate": "2026-06-15T10:08:57.407",
      "dueDate": "2026-06-15T10:08:57.407",
      "status": "InProgress",
      "remarks": ""
    },
    "reviews": [],
    "submissionUrl": "https://localhost:9000",
    "notes": "Nothing",
    "submittedDate": "2026-06-15T10:14:07.178",
    "status": "Submitted"
  }
]


Sample GET /api/submissions/{1} response:
{
  "id": 1,
  "taskAssignmentId": 1,
  "taskAssignment": {
    "id": 1,
    "traineeId": 1,
    "mentorId": 1,
    "learningTaskId": 1,
    "submission": [
      {
        "id": 1,
        "taskAssignmentId": 1,
        "submissionUrl": "https://localhost:9000",
        "notes": "Nothing",
        "submittedDate": "2026-06-15T10:14:07.178",
        "status": "Submitted",
        "reviews": []
      }
    ],
    "assignedDate": "2026-06-15T10:08:57.407",
    "dueDate": "2026-06-15T10:08:57.407",
    "status": "InProgress",
    "remarks": ""
  },
  "reviews": [],
  "submissionUrl": "https://localhost:9000",
  "notes": "Nothing",
  "submittedDate": "2026-06-15T10:14:07.178",
  "status": "Submitted"
}

Sample POST /api/submissions response: 
{
  "id": 1,
  "taskAssignmentId": 1,
  "taskAssignment": null,
  "reviews": [],
  "submissionUrl": "https://localhost:9000",
  "notes": "Nothing",
  "submittedDate": "2026-06-15T10:14:07.178Z",
  "status": "Submitted"
}

Sample GET /api/reviews response:
[
  {
    "id": 1,
    "submissionId": 1,
    "submission": {
      "id": 1,
      "taskAssignmentId": 1,
      "submissionUrl": "https://localhost:9000",
      "notes": "Nothing",
      "submittedDate": "2026-06-15T10:14:07.178",
      "status": "Submitted",
      "reviews": [
        {
          "id": 1,
          "submissionId": 1,
          "mentorId": 1,
          "feedback": "Good implementation",
          "score": 9,
          "reviewStatus": "Accepted",
          "reviewedDate": "2026-06-15T10:16:43.838"
        }
      ]
    },
    "mentorId": 1,
    "mentor": {
      "id": 1,
      "firstName": "Om",
      "lastName": "Deshmukh",
      "email": "omdeshmukh@example.com",
      "expertise": "Java Spring Boot",
      "status": "Inactive",
      "createdDate": "2026-06-15T09:59:11.51592",
      "updatedDate": "2026-06-15T10:00:48.726323",
      "taskAssignments": [
        {
          "id": 1,
          "traineeId": 1,
          "mentorId": 1,
          "learningTaskId": 1,
          "submission": [
            {
              "id": 1,
              "taskAssignmentId": 1,
              "submissionUrl": "https://localhost:9000",
              "notes": "Nothing",
              "submittedDate": "2026-06-15T10:14:07.178",
              "status": "Submitted",
              "reviews": [
                {
                  "id": 1,
                  "submissionId": 1,
                  "mentorId": 1,
                  "feedback": "Good implementation",
                  "score": 9,
                  "reviewStatus": "Accepted",
                  "reviewedDate": "2026-06-15T10:16:43.838"
                }
              ]
            }
          ],
          "assignedDate": "2026-06-15T10:08:57.407",
          "dueDate": "2026-06-15T10:08:57.407",
          "status": "InProgress",
          "remarks": ""
        }
      ],
      "reviews": [
        {
          "id": 1,
          "submissionId": 1,
          "mentorId": 1,
          "feedback": "Good implementation",
          "score": 9,
          "reviewStatus": "Accepted",
          "reviewedDate": "2026-06-15T10:16:43.838"
        }
      ]
    },
    "feedback": "Good implementation",
    "score": 9,
    "reviewStatus": "Accepted",
    "reviewedDate": "2026-06-15T10:16:43.838"
  }
]

Sample GET /api/reviews/{1} response:
{
  "id": 1,
  "submissionId": 1,
  "submission": {
    "id": 1,
    "taskAssignmentId": 1,
    "submissionUrl": "https://localhost:9000",
    "notes": "Nothing",
    "submittedDate": "2026-06-15T10:14:07.178",
    "status": "Submitted",
    "reviews": [
      {
        "id": 1,
        "submissionId": 1,
        "mentorId": 1,
        "feedback": "Good implementation",
        "score": 9,
        "reviewStatus": "Accepted",
        "reviewedDate": "2026-06-15T10:16:43.838"
      }
    ]
  },
  "mentorId": 1,
  "mentor": {
    "id": 1,
    "firstName": "Om",
    "lastName": "Deshmukh",
    "email": "omdeshmukh@example.com",
    "expertise": "Java Spring Boot",
    "status": "Inactive",
    "createdDate": "2026-06-15T09:59:11.51592",
    "updatedDate": "2026-06-15T10:00:48.726323",
    "taskAssignments": [
      {
        "id": 1,
        "traineeId": 1,
        "mentorId": 1,
        "learningTaskId": 1,
        "submission": [
          {
            "id": 1,
            "taskAssignmentId": 1,
            "submissionUrl": "https://localhost:9000",
            "notes": "Nothing",
            "submittedDate": "2026-06-15T10:14:07.178",
            "status": "Submitted",
            "reviews": [
              {
                "id": 1,
                "submissionId": 1,
                "mentorId": 1,
                "feedback": "Good implementation",
                "score": 9,
                "reviewStatus": "Accepted",
                "reviewedDate": "2026-06-15T10:16:43.838"
              }
            ]
          }
        ],
        "assignedDate": "2026-06-15T10:08:57.407",
        "dueDate": "2026-06-15T10:08:57.407",
        "status": "InProgress",
        "remarks": ""
      }
    ],
    "reviews": [
      {
        "id": 1,
        "submissionId": 1,
        "mentorId": 1,
        "feedback": "Good implementation",
        "score": 9,
        "reviewStatus": "Accepted",
        "reviewedDate": "2026-06-15T10:16:43.838"
      }
    ]
  },
  "feedback": "Good implementation",
  "score": 9,
  "reviewStatus": "Accepted",
  "reviewedDate": "2026-06-15T10:16:43.838"
}

Sample POST /api/reviews response:
{
  "id": 1,
  "submissionId": 1,
  "submission": null,
  "mentorId": 1,
  "mentor": null,
  "feedback": "Good implementation",
  "score": 9,
  "reviewStatus": "Accepted",
  "reviewedDate": "2026-06-15T10:16:43.838Z"
}


## Known Limitations
- Scalability 
- Better architecture or schema design

## Security Checklist


## Next Improvement areas
- Improve the scalability of the api's
