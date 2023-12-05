CREATE PROCEDURE [dbo].[spSelDshIDModulo] @nombreMenu VARCHAR(100) AS
BEGIN
	SELECT top 1 id
	FROM DshModulos
	WHERE nombreMenu= @nombreMenu;
END