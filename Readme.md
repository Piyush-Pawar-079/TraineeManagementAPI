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