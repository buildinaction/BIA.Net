MERGE INTO [MemberRole] AS Target
USING (VALUES

-- Begin add here data
(1,'Site Admin')
-- End here data
)
AS Source ([Id], [Title])
ON Target.[Id] = Source.[Id]
--update matched rows
WHEN MATCHED THEN
UPDATE SET [Title] = Source.[Title]
--insert new rows
WHEN NOT MATCHED BY TARGET THEN
INSERT ([Id], [Title])
VALUES ([Id], [Title]);