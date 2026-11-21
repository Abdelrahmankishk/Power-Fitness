/*
    IMPORTANT: Run this script exactly once, after taking a full backup of the production database.

    Existing Session.StartDate and Session.EndDate values were persisted from datetime-local
    form fields as Egypt local wall-clock times. The application will now store these values in UTC.

    Do not run this script again. Re-running it would shift the dates a second time.
*/

IF NOT EXISTS (SELECT 1 FROM sys.time_zone_info WHERE name = N'Egypt Standard Time')
    THROW 50001, 'The Windows time zone Egypt Standard Time is not available on this SQL Server instance.', 1;

IF OBJECT_ID(N'dbo.Sessions', N'U') IS NULL
    THROW 50002, 'The dbo.Sessions table was not found. Verify the table name before running this script.', 1;

BEGIN TRANSACTION;

UPDATE dbo.Sessions
SET
    StartDate = CAST(StartDate AT TIME ZONE 'Egypt Standard Time' AT TIME ZONE 'UTC' AS datetime2(7)),
    EndDate = CAST(EndDate AT TIME ZONE 'Egypt Standard Time' AT TIME ZONE 'UTC' AS datetime2(7));

COMMIT TRANSACTION;
