# Hospital Project W2022
## 1. About the Project
<br>
This project is intended to build a general Content Management System(CMS) for hospitals. There are many functions included:

- Manage Department (CRUD)
- Manage patient (CRUD)
- Manage staff (CRUD)
- Manage shifts (CRUD)
- Manage Appointment (CRUD)
- Manage relationship between Staff and Shift
- Manage relationship between Patient and Appointment
- Manage relationship between Staff and Department
- Patients can add, update or delete their appointment(s). CMS Admin can view all the appointments
<br>
This project was made in collaboration with:

- [Hardi Hemantkumar Patel](https://github.com/patelhardi) - Basic CRUD for Appointment and Shift Entity, Manage one-to-many and many-to-many relationship between all entities
- [Jenny Dcruz](https://github.com/jendcruz22) - Basic CRUD for Patient and Staff Entity, Overall Project Design Layout
- [Qiyuan Liu](https://github.com/liuqiyuan628) - Basic CRUD for Department Entity, Manage Authentication and Authorization for all Entities
<br> 
<br>

## 2. Entities Relationship

![entyties relationship](https://user-images.githubusercontent.com/73659957/161692167-3d947073-62ab-4a00-838b-ddd02e8b44b2.png)
<br>
<br>
<br>
### 2.2 Authorization Permissions for Diffente Role

- Department Entity – All Functionality – Admin Role
- Patient Entity – All Functionality – Admin Role
- Staff Entity – All Functionality – Admin Role
- Schedule Entity – All Functionality – Admin Role
- Appointment Entity
  - Create, Update, Delete - Patient Role
  - List All Appointment – Admin, Patient Role
  - Find Appointment – Admin, Patient Role
<br>
<br>

## 3. How to Run This Project?

1. Clone the repository in Visual Studio
2. Open the project folder on your computer (e.g. File Explore for Windows Users)
3. Create an <App_Data> folder in the main project folder
4. Go back to Visual Studio and open Package Manager Console and run the query to build the database on your local server:
```
update-database
```
5. The project should set up


### 3.2 Before checking all the functions, manually add users data for Authorization!

1. Run the project on your browser and create at least two users using the registration function. Then close the browser and go back to Visual Studio
2. open you database in the <SQL Server Object Explorer>, you will find all the tables for this project
3. Find <dbo.AspNetRoles> table and create two roles:
  - id:1, Name:"Admin"
  - id:2, Name:"Patient"
4. Find <dbo.AspNetUsers> table and you will find the two users you registered. Copy one of the user's id.
5. Find <dbo.AspNetUserRoles> table, and paste the id you copied into the first Userid row, then set the RoleId to "1"(1 = "Admin")
6. Repeat these steps and set another user's RoleId to "2"(2 = patient)
7. Now, you've set a user with "Admin" role, and the other one with "Patient" role. You are good to go!
