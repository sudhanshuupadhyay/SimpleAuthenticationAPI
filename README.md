# UserAuthApi  Database object :


--DataBase Tables for UserAuthApi


CREATE TABLE [dbo].[UserInfo](
	[Name] [nvarchar](100) NOT NULL,
	[Mobile] [int] NOT NULL,
	[Address] [nvarchar](255) NULL,
	[Skills] [nvarchar](255) NULL,
	[UserID] [int] NOT NULL,
	[Email] [nvarchar](100) NULL,
	[QRCode] [nvarchar](max) NULL
) 



CREATE TABLE [dbo].[UserRatings](
	[ResponseID] [int] NOT NULL,
	[UserMobile] [int] NULL,
	[RespondentName] [nvarchar](100) NULL,
	[RespondentMobile] [int] NULL,
	[RespondentEmail] [nvarchar](100) NULL,
	[ReviewComment] [nvarchar](max) NULL,
	[Rating] [int] NOT NULL,
	[AdditionalAttribute] [nvarchar](max) NULL
)

CREATE TABLE [dbo].[AuditLog](
	[AuditID] [numeric](18, 0) NOT NULL,
	[UserId] [nvarchar](50) NOT NULL,
	[ExecutionDate] [datetime] NOT NULL,
	[Action] [nvarchar](50) NOT NULL,
	[ErrorMsg] [nvarchar](max) NULL
)


--Stored Procs for API

CREATE PROCEDURE [dbo].[sp_InsertUserInfo] 
@Name nvarchar(100), 
@Mobile int,
@Address nvarchar(255), 
@Skills nvarchar(max),
@Email nvarchar(50), 
@QRCode nvarchar(max)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
  Declare @userID as Int 
  Set @userID = ISNull((Select Top 1 UserID from UserInfo Order by UserID desc),0) + 1
  Print @userID

 INSERT INTO UserInfo (UserID, [Name],Mobile ,[Address],Skills,Email,QRCode)
 VALUES (@userID,@Name,@Mobile,@Address,@Skills,@Email,@QRCode);

 

END


CREATE PROCEDURE [dbo].[sp_InsertUserRating] 
@Rating int, 
@Mobile int,
@RespondentEmail nvarchar(255), 
@RespondentMobile int,
@RespondentName nvarchar(50), 
@ReviewComment nvarchar(max),
@AdditionalAttribute nvarchar(max)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
  Declare @responseID as Int 
  Set @responseID = ISNull((Select Top 1 ResponseID from UserRatings Order by ResponseID desc),0) + 1
  Print @responseID

 INSERT INTO UserRatings (ResponseID, UserMobile,RespondentName ,RespondentMobile,RespondentEmail,ReviewComment,Rating,AdditionalAttribute)
 VALUES (@responseID,@Mobile,@RespondentName,@RespondentMobile,@RespondentEmail,@ReviewComment,@Rating,@AdditionalAttribute);

 

END
