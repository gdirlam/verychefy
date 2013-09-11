


CREATE PROCEDURE [Products].[PairingsMake]  AS
 
DECLARE @thisingredient AS nvarchar (250)  = NULL  
DECLARE @matching AS nvarchar (250)  = NULL  

SET @thisingredient = 'Potato';
SET @matching = 'Sage';

DECLARE @thisIngredient_pk int;
DECLARE @matchingIngredient_pk int;
IF NOT EXISTS(SELECT Ingredient_pk FROM [Products].[Ingredients] WHERE CommonName = @thisingredient)
BEGIN

	EXECUTE [Food].[Products].[IngredientsCreate] 
	   @thisIngredient_pk OUTPUT
	  , @thisingredient


	SELECT * FROM [Products].[Ingredients] WHERE Ingredient_pk = @thisIngredient_pk
	PRINT  @thisingredient + ' not in list'
END 	
ELSE


IF NOT EXISTS(SELECT Ingredient_pk FROM [Products].[Ingredients] WHERE CommonName = @matching)
BEGIN


	EXECUTE [Food].[Products].[IngredientsCreate] 
	   @matchingIngredient_pk  OUTPUT
	  , @matching

	PRINT @matching + ' not in list'
END 	
