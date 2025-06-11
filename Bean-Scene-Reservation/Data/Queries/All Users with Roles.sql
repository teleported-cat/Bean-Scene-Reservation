USE [BeanSceneReservationDB];

--SQL query to show all users with their roles
SELECT u.Id AS UserId, u.UserName, u.Email, CONCAT(u.FirstName, ' ', u.LastName) AS FullName, STRING_AGG(r.Name, ', ') AS Roles
FROM AspNetUsers u
    LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
    LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
GROUP BY    u.Id, u.UserName, u.Email, u.FirstName, u.LastName;