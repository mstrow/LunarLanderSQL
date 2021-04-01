DROP DATABASE IF EXISTS LanderGame;
CREATE DATABASE LanderGame;
USE LanderGame;

DELIMITER //
DROP PROCEDURE IF EXISTS createSchema//

CREATE PROCEDURE createSchema()
BEGIN

	CREATE TABLE tblAccount (
	  Username varchar(16) NOT NULL, 
	  Password varchar(32) NOT NULL, 
	  is_Admin BOOLEAN DEFAULT FALSE NOT NULL, 
	  HighScore int DEFAULT 0 NOT NULL, 
	  LoginAttempts int DEFAULT 0 NOT NULL, 
	  Locked BOOLEAN DEFAULT FALSE NOT NULL, 
	  PRIMARY KEY (Username)
	);
	CREATE TABLE tblLander (
	  `Name` varchar(16) NOT NULL, 
	  Propulsion int NOT NULL, 
	  Weight int NOT NULL, 
	  CenterOfGravity float NOT NULL, 
	  ScoreBonus int NOT NULL, 
	  Fuel int NOT NULL, 
	  DeltaFuel int NOT NULL, 
	  Thrust int NOT NULL, 
	  PRIMARY KEY (Name)
	);
	CREATE TABLE tblTile (
	  ID int NOT NULL AUTO_INCREMENT, 
	  Map varchar(16) NOT NULL, 
	  x int NOT NULL, 
	  y int NOT NULL, 
	  Texture varchar(32) NOT NULL, 
	  PRIMARY KEY (ID)
	);
	CREATE TABLE tblMap (
	  Name varchar(16) NOT NULL, 
	  Gravity int NOT NULL, 
	  AngleLimit int NOT NULL, 
	  SpeedLimit int NOT NULL, 
	  PRIMARY KEY (Name)
	);
	CREATE TABLE tblGame (
	  ID int NOT NULL AUTO_INCREMENT, 
	  Player varchar(16) NOT NULL, 
	  Lander varchar(16) NOT NULL, 
	  `Map` varchar(16) NOT NULL, 
	  GameActive BOOLEAN DEFAULT FALSE NOT NULL, 
	  PositionX int NOT NULL, 
	  PositionY int NOT NULL, 
	  VelocityX float NOT NULL, 
	  VelocityY float NOT NULL, 
	  Rotation float NOT NULL, 
	  Rocket BOOLEAN DEFAULT FALSE NOT NULL, 
	  Score int DEFAULT 0 NOT NULL, 
	  PRIMARY KEY (ID)
	);
	CREATE TABLE tblGlobalChat (
	  ID int NOT NULL AUTO_INCREMENT, 
	  Player varchar(16) NOT NULL, 
	  `Time` DATETIME NOT NULL, 
	  Message varchar(255) NOT NULL, 
	  PRIMARY KEY (ID)
	);
	ALTER TABLE 
	  tblGame 
	ADD 
	  CONSTRAINT FK_ActiveLander FOREIGN KEY (Lander) REFERENCES tblLander (Name) ON UPDATE CASCADE;
	ALTER TABLE 
	  tblGame 
	ADD 
	  CONSTRAINT FK_ActiveMap FOREIGN KEY (Map) REFERENCES tblMap (Name) ON UPDATE CASCADE;
	ALTER TABLE 
	  tblTile 
	ADD 
	  CONSTRAINT FK_MapTile FOREIGN KEY (Map) REFERENCES tblMap (Name) ON DELETE CASCADE ON UPDATE CASCADE;
	ALTER TABLE 
	  tblGlobalChat 
	ADD 
	  CONSTRAINT FK_PlayerMessage FOREIGN KEY (Player) REFERENCES tblAccount (Username) ON DELETE CASCADE ON UPDATE CASCADE;
	ALTER TABLE 
	  tblGame 
	ADD 
	  CONSTRAINT FK_ActivePlayer FOREIGN KEY (Player) REFERENCES tblAccount (Username) ON DELETE CASCADE ON UPDATE CASCADE;
END //
DELIMITER ;

CALL createSchema();  -- creates tables

DELIMITER //
DROP PROCEDURE IF EXISTS generateMap//

CREATE PROCEDURE generateMap(mapName varchar(16), mapSize int, seed int, gravity int, angleLimit int, speedLimit int)
BEGIN
	INSERT INTO tblMap(Name, Gravity, AngleLimit, SpeedLimit) VALUES (mapName, gravity, angleLimit, speedLimit);
	SET @s = seed / 10000000;
	SET @x = 1;
	loop_label:  LOOP
		IF  @x > mapSize THEN 
			LEAVE  loop_label;
		END  IF;
		
		SET @y = (SIN(@x/5) * 5) + (COS(@x/2) * 2) + (SIN(@x) / 2 ) + (SIN(@x/10) * 10) + (SIN(@x/30) * 30);
		
		SET @y = FLOOR(@y);
		
		INSERT INTO tblTile(Map, x, y, Texture) VALUES (mapName, @x, @y, "none");
		
		SET  @x = @x + 1;
		
		END LOOP;
		
END //

DELIMITER ;

CALL generateMap('moon', 100, 482742, 3, 100, 10); -- generates map using fancy sine function and populates it with tiles

INSERT INTO tblAccount(Username, Password) VALUES ("User", "Pass"); -- creates account

INSERT INTO tblLander(Name, Propulsion, Weight, CenterOfGravity, ScoreBonus, Fuel, DeltaFuel, Thrust) VALUES ("Apollo", 3, 10, 0.43, 2, 3000, 3000, 200); -- creates a lander

INSERT INTO tblGame(Player, Lander, Map, PositionX, PositionY, VelocityX, VelocityY, Rotation) VALUES ("User", "Apollo", "moon", 0, 200, 100, 0, 90); -- creates a game

INSERT INTO tblGlobalChat(Player, Time, Message) VALUES ("User", "2017-08-15 19:30:10", "Hello"); -- creates a new chat message

UPDATE tblAccount SET Password = "pass2" WHERE Username = "User"; -- update user

UPDATE tblLander SET Weight = 15 WHERE Name = "Apollo"; -- update lander

UPDATE tblGlobalChat SET Message = "Hello again" WHERE Player = "User" AND Time = "2017-08-15 19:30:10"; -- update chat message

UPDATE tblMap SET Name = "moon2" WHERE Name = "moon"; -- update the map named moon, also updates all the tiles on the map

UPDATE tblGame SET Rotation = "120" WHERE Player = "User" AND Lander = "Apollo"; -- updates rotation on game

UPDATE tblTile SET y = 10 WHERE x = 1 AND Map = "moon2"; -- updates tile

-- prints tables

SELECT * FROM tblAccount; 
SELECT * FROM tblLander;
SELECT * FROM tblGame;
SELECT * FROM tblGlobalChat;
SELECT * FROM tblMap;
SELECT * FROM tblTile;

DELETE FROM tblAccount WHERE Username = "User"; -- deletes ACCOUNT and their GAMES and MESSAGES.

DELETE FROM tblMap WHERE Name = "moon2"; -- deletes map and all the TILES associated with it

DELETE FROM tblLander WHERE Name = "Apollo"; -- deletes lander

-- nothing left to delete. It was cascaded :)