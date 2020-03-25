MERGE INTO [Version] AS Target
USING (VALUES

-- Begin add here data
(1,'AppCode','1.0.0'),
(2,'InitializedData','1.0.0.0'),
(3,'PendingInitialization','false')
-- End here data
)
AS Source ([Id], [Name], [Value])
ON Target.[Id] = Source.[Id]
--update matched rows
WHEN MATCHED THEN
UPDATE SET [Name] = Source.[Name],
           [Value] = Source.[Value]
--insert new rows
WHEN NOT MATCHED BY TARGET THEN
INSERT ([Id], [Name], [Value])
VALUES ([Id], [Name], [Value]);