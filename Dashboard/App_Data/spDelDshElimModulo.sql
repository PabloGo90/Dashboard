CREATE PROCEDURE [dbo].[spDelDshElimModulo] @id INT AS
BEGIN
	Delete from DshModulos where id=@id
END