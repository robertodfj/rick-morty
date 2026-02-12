DELETE Users WHERE username = 'testuser'; 
INSERT INTO Users (username, [Password], Email, Role, Money, LastWorked, TimesWorked) VALUES ('testuser', '$2a$12$RYO1H1dbpHBRfu/QwXlizeNnyqK4XZT7lbOyV8qia8qbwa.LpR982', 'test@email.com', 'Admin', 1000000, GETDATE(), 10000);
SELECT * FROM Users WHERE username = 'testuser'; 