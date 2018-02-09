-- 2. Ile mamy rekordow w tabelach Artist i Album?

USE Chinook
GO

SELECT COUNT(*)
FROM Artist

SELECT COUNT(*)
FROM Album

--3. Polacz tabele Artist i Album – wyswietl rowniez Artystow bez albumow. Ile jest Artystow bez albumow?
USE Chinook
GO
SELECT COUNT(*) AS "ALL ARTISTS"
FROM Album AL
RIGHT JOIN Artist AR
ON AL.ArtistId = AR.ArtistId

SELECT  COUNT(*) AS "ARTISTS WITHOUT ALBUM"
FROM Album AL
RIGHT JOIN Artist AR
ON AL.ArtistId = AR.ArtistId
WHERE AL.ArtistId IS NULL

-- 4. Wyswietl wszystkich artystow ktorzy maja dokladnie 4 albumy
USE Chinook
GO
SELECT AR.Name, COUNT(AL.AlbumId)
FROM Album AL
JOIN Artist AR
ON AL.ArtistId = AR.ArtistId
GROUP BY AR.Name
HAVING COUNT(AL.AlbumId)=4

-- 5. Ilu Customerów pochodzi z Germany?

USE Chinook
GO
SELECT COUNT(*) AS 'CUSTOMERS FROM GERMANY'
FROM Customer
WHERE Country = 'Germany'

-- 6. ktorzy customerzy wydaja najwiecej pieniedzy?

USE Chinook
GO
SELECT C.LastName, C.FirstName, SUM(I.Total)
FROM Customer C
JOIN Invoice I 
ON I.CustomerId = C.CustomerId
GROUP BY C.LastName, C.FirstName
ORDER BY  SUM(I.Total) DESC

-- 7. Ktore Track sa najczesciej kupowane?

USE Chinook
GO
SELECT T.Name, T.TrackId, SUM(I.Quantity)
FROM InvoiceLine I
JOIN Track T 
ON T.TrackId = I.TrackId
GROUP BY T.Name, T.TrackId
ORDER BY SUM(I.Quantity) DESC

-- 8. (optional) jakiego Artysty utwory sa najczesciej kupowane w danym kraju?

USE Chinook
GO
SELECT AR.Name
FROM Artist AR
JOIN Album AL
ON AL.ArtistId = AR.ArtistId