It is necessary to implement a web application for an information system that enables the management of fitness centers. 
The application is used by three groups (roles) of users: Visitor, Trainer, and Owner. 
The application manages the following entities:

User

  - Username (unique)
  - Password
  - First name
  - Last name
  - Gender
  - Email
  - Date of birth (store in format dd/MM/yyyy)
  - Role (Visitor, Trainer, or Owner)
  - List of group trainings the user is enrolled in (if the user has the role of Visitor)
  - List of group trainings the user is engaged in as a trainer (if the user has the role of Trainer)
  - Fitness center where the user is engaged (if the user has the role of Trainer)
  - Fitness centers owned by the user (if the user has the role of Owner)

Fitness Center

  - Name
  - Address in the format: street and number, city/town, postal code
  - Year of opening of the fitness center
  - Owner (relation to the User class)
  - Monthly membership fee
  - Annual membership fee
  - Price of a single training session
  - Price of a single group training session
  - Price of a personal training session

Group Training

  - Name
  - Type of training (yoga, Les Mills Tone, Body Pump, etc.)
  - Fitness center where the training takes place (relation to the Fitness Center class)
  - Duration of training (expressed in minutes)
  - Date and time of training (store in format dd/MM/yyyy HH:mm)
  - Maximum number of visitors
  - List of visitors (list of Users with the role of Visitor who have enrolled to attend the training)

Comment

  - Visitor who left the comment
  - Fitness center the comment refers to
  - Comment text
  - Rating (on a scale from 1 to 5)

Note: The entities listed above are mandatory and must contain the specified attributes. 
It is allowed to add additional entities as well as to extend existing entities with more attributes if needed.

Functionalities to Implement

Unregistered User

  - On the home page, they can view all Fitness Centers (sorted by name in ascending order) that exist in the system in a table format.
  - They can search fitness centers by name, address, and year of opening (for year of opening, allow defining a minimum and maximum range for the search).
  - Enable combined search so that an unregistered user can enter multiple search parameters and see results that meet all entered criteria.
  - They can sort fitness centers by name (ascending and descending), address (ascending and descending), and year of opening (ascending and descending).
  - By clicking the Details button, the user is taken to a page showing all information about the selected fitness center (detailed view).
  - Below the detailed view of the selected fitness center, there should be a list (table view) of upcoming group trainings (those that will take place in the future).
    For each group training, all relevant information must be displayed, including date and time, maximum number of visitors, and the total number of registered visitors at that time.
  - Below the list of all group trainings, the user can see all comments left by Visitors for that fitness center.
  - Registration – registers in the application by filling in the required fields, after which they become a Visitor.
  - Login – logs into the system by entering username and password. If the login is successful, the user can perform activities defined for their role.

Registered User

  - After logging in, the user sees the same content on the home page (list of fitness centers) as an unregistered user and can search and sort them in the same way.
  - Depending on the role of the registered user, they can navigate from this page to other pages corresponding to their role.
  - Every registered user can view and edit their profile.
  - A registered user can perform all the same actions as an unregistered user.

List of Functionalities Depending on Role:

Visitor

  - Can register to attend a training session.
    By selecting one of the fitness centers from the home page, they can view a list of all upcoming group trainings at that fitness center.
    By clicking the button next to a specific group training, they can register to participate in the selected training session.
    A user can register for a specific training session only once (cannot participate multiple times in the same time slot).
    If the maximum number of participants for a group training is reached, they cannot register for that training session.
  - Can view a list (table view) of all past group trainings they have attended.
  - Can search the list of all past group trainings they attended by training name, training type, and fitness center name.
  - Enable combined search so that the Visitor can enter multiple search parameters and see results that meet all entered criteria.
  - Can sort the list of all past group trainings they attended by name (ascending and descending), training type (ascending and descending), and date and time of the training (ascending and descending).
  - Can leave a comment (with rating and text) for a fitness center they have previously visited (this implies they were registered for a group training that took place at that fitness center, i.e., the group training occurred with the Visitor in attendance).
    This comment becomes visible to all users if approved by the Owner of that fitness center.

Trainer

  - Can create, modify, view, and delete their own group trainings.
    - Deletion of a group training is logical (soft delete).
    - Deletion of a group training is not allowed if any Visitor has registered to participate in that training.
    - Can modify and delete only group trainings that have not yet taken place, where they are engaged as the trainer, and at the fitness center where they are employed.
    - For every group training they create, they are automatically assigned as the trainer for that session, and this information cannot be changed.
    - A group training cannot be created in the past; it can only be scheduled for the future, at least 3 days in advance.
  - Can view all group trainings they have conducted as a trainer in the past.
  - When viewing their group trainings (upcoming and completed), they can choose to see a list of all Visitors who have registered to participate in a specific training session.
  - Can search all group trainings they have conducted in the past by training name, training type, and date and time of the training (for date and time, allow defining minimum and maximum limits for the search).
  - Enable combined search so that the Trainer can enter multiple search parameters and see results that meet all entered criteria.
  - Can sort the list of all past group trainings they have conducted by name (ascending and descending), training type (ascending and descending), and date and time of the training (ascending and descending).

Owner

  - Are loaded programmatically from a text file and cannot be added later.
  - Registers new trainers in their fitness center (if the Owner has multiple fitness centers, they can add a trainer as an employee to one of them, i.e., a trainer can work in exactly one fitness center).
  - Can create, modify, view, and delete their own fitness centers.
    - Deletion of a fitness center is logical (soft delete).
    - Deletion of a fitness center is not allowed if there are upcoming group trainings scheduled.
    - If a fitness center is deleted, all trainers employed at that fitness center lose the ability to log into the application.
  - Can block a trainer, after which the trainer can no longer log into the application.
  - When a Visitor leaves a comment, the Owner can approve or reject it.
    - An approved comment becomes visible to everyone.
    - A rejected comment is visible only to that Owner.  
    - Only comments on the fitness center created by that Owner can be approved or rejected.
