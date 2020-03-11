/* Simple catch and return of error code via output parm */
DECLARE @OutputMessage NVARCHAR(MAX) --OUTPUT
BEGIN TRY
    SELECT 'do stuff...'
END TRY
BEGIN CATCH
    SET @OutputMessage = ERROR_MESSAGE()
END CATCH

/*	Adding linebreaks 
	char(13)+char(10) for CRLF style
	CHAR(13) for CR style
*/
SELECT 'This is line 1.' + CHAR(13) + CHAR(10) + 'This is line 2.'