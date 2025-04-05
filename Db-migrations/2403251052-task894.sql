-- Inserting Resources into the Resource Table
INSERT INTO [dbo].[Resource] (Name, Description, Type)
VALUES
    ('Laptop computer', 'Equipment', 'Equipment'),
    ('Hitch on my car/truck', 'Equipment', 'Equipment'),
    ('My vehicle can carry more than six people', 'Equipment', 'Equipment'),
    ('My vehicle has four-wheel drive', 'Equipment', 'Equipment'),
    ('Backhoe', 'Equipment', 'Equipment'),
    ('Forklift', 'Equipment', 'Equipment'),
    ('Shovel', 'Equipment', 'Equipment'),
    ('Rake', 'Equipment', 'Equipment'),
    ('Wheelbarrow', 'Equipment', 'Equipment'),
    ('Trailer 4-6 ft', 'Trailer', 'Trailer'),
    ('Trailer 8-14 ft', 'Trailer', 'Trailer'),
    ('Trailer 16+ ft', 'Trailer', 'Trailer');

-- Delete  Skill into the skills  Table
DELETE FROM [dbo].[Skill]
WHERE [Name] = 'Heavy Lifting';


-- Inserting Skills into the Skills Table
INSERT INTO [dbo].[Skill] ([Name], [Description])
VALUES
    ('No lifting', 'Ability to refrain from lifting'),
    ('Can Lift: 25-50 lbs', 'Ability to lift weights between 25 to 50 lbs'),
    ('Can Lift: 50-100 lbs', 'Ability to lift weights between 50 to 100 lbs'),
    ('I can drive a vehicle pulling a trailer', 'Ability to drive a vehicle with a trailer attached'),
    ('I can drive a self-contained truck', 'Ability to drive a self-contained truck');


