/* Simple catch and return of error code via output parm */
DECLARE @OutputMessage NVARCHAR(MAX) --OUTPUT
BEGIN TRY
	--....
END TRY
BEGIN CATCH
    SET @OutputMessage = ERROR_MESSAGE()
END CATCH