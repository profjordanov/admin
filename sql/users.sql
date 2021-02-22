/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) Users.[Id]
      ,[FullName]
      ,[FacultyNumber]
	  ,TopicResults.Score
  FROM [dbo].[AspNetUsers] AS Users
  LEFT JOIN [dbo].[UserTopicResults] AS TopicResults
  ON Users.Id = TopicResults.UserId
