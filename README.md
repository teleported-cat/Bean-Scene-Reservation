# Bean Scene Reservation ASP.NET Web Application

[📱Link to Bean Scene's Ordering System Repository](https://github.com/teleported-cat/BeanSceneOrderingFrontend)

[🎥Video Demonstration of this System](https://www.youtube.com/watch?v=oietcdXqwZU)

A restaurant reservation applications which allow staff and customers to reserve tables for scheduled sittings.

This system was developed for a fictional client which is a restaurant called "Bean Scene", and follows the requirements and business rules they layed out.

This project was developed over course of ~20 weeks from Feburary 2025 to June 2025, with a majority of the progress made in the latter half.

## Workflows

There are 4 different workflows & depending on who the user is authorised as:

As a **guest** (when a user is not logged in), you can only place reservations in valid scheduled sittings.

As a **member**, you can place reservations in sittings, with personal information such as name, phone number, & email automatically filled in, as well as viewing all reservations placed under your account in reservation history.

As **staff**, you can capture reservations into the system from phone, email, or in-person reservation requests, track all reservations within the system, and update a reservation's status (pending, confirmed, in-progress, completed, or cancelled).

As a **manager**, you have access to managing the areas & tables recorded within the system, schedule sittings in bulk over a period of time, track all reservations, manage all user accounts, view reports visualising reservation data, and have access to all staff functionality.

## How Reservations Work

Firstly, the timeslots within the system are seeded to be 30-minute intervals within the hour (e.g. 10:00am, 10:30am, etc.) Each reservation can only start/end on these times; this is a part of the business rules outlined by Bean Scene.

The areas and the tables within each area are seeded into the database and can be manually updated by a manager if the restaurant's layout is updated. A table is always set to seat 4 guests.

For sittings, there are 3 types: Breakfast, Lunch, & Dinner, but the database can be updated to include special occasions.

A sitting is set for a date & type (Breakfast, Lunch, Dinner), and includes the start/end times, its capacity, and whever or not it's open for reservations.

Sittings can be individually created, but this isn't recommended as the manager can generate them in bulk using sitting schedules. A sitting schedule allows the manager to set all the information about a specific sitting (Breakfast, Lunch, Dinner) on a day of the week over set period. The client requested this feature to allow them to create sittings on a quarterly basis.

When a customer places or a staff member captures a reservation, they must select a date, sitting type, and when they would like the reservation to start & end.

The restaurant area and number of guests must be noted as well. The system automatically chooses available tables within the selected area for the reservation, and if the number of guests exceeds the current capacity for that area during that sitting, it will prevent it and warn the user.

The reservation then requests a first & last name of who placed it, as well as the customer's email & phone number so the restaurant can confirm the reservation a week ahead. Name is required, but only either an email or a phone number is required. This information is automatically filled in if the user is a member.

Finally, the reservation can include a note with additional information if the reserver so chooses.

If the reserver was a member, the reservation keeps track of that and allows them to view it later in their reservation history.

## Technology/Tools Used

The entire system is built in ASP.NET Core MVC using .NET 8.0.

The front-end are CSHTML pages which uses C# to pre-process HTML pages that are styled with Bootstrap classes & hand-written CSS, and client-side functionality is written with vanilla JavaScript.

The back-end uses C# for server-side functionality & communicating between controllers and views. The database uses Microsoft SQL Server mapped from the models defined using Entity Framework, Microsoft's Object Relational Mapper.

The system was hosted on Microsoft Azure to test its live functionality, but is currently deactivated to stop unnecessary costs.

The system also uses libraries such as:

- Bootstrap Icons for vector icons
- Chart.js for report graphs
- Dragula for drag & drop functionality

## UI

The user interface was built to be mobile responsive to allow both customers to place reservations quickly & easily on their mobiles devices, and managers to access their dashboard from anywhere.

## Unit Testing

This project uses XUnit for unit testing, primarly for conntroller functionality to ensure they always produce the expected result/correct functionality.
