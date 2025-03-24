DELETE FROM [dbo].[UserResource]
WHERE [ResourceId] IN (SELECT [ResourceId] FROM [Resource] WHERE [Type] IS NULL OR [Type] = '');


DELETE FROM [Resource]
WHERE [Type] IS NULL OR [Type] = '';
