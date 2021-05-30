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
	  Online boolean DEFAULT FALSE NOT NULL,
	  Locked BOOLEAN DEFAULT FALSE NOT NULL,
	  PRIMARY KEY (Username)
	);
	CREATE TABLE tblLander (
	  `Name` varchar(16) NOT NULL,
	  Weight int NOT NULL,
	  ScoreBonus int NOT NULL,
	  Fuel int NOT NULL,
	  Thrust int NOT NULL,
	  SpeedLimit float DEFAULT 1.0 NOT NULL,
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
	CREATE TABLE tblCollisionOffset (
	  ID int NOT NULL AUTO_INCREMENT,
	  x int NOT NULL,
	  y int NOT NULL,
	  PRIMARY KEY (ID)
	);
	CREATE TABLE tblMap (
	  Name varchar(16) NOT NULL,
	  Gravity int NOT NULL,
	  PRIMARY KEY (Name)
	);
	CREATE TABLE tblGame (
	  ID int NOT NULL AUTO_INCREMENT,
	  Player varchar(16) NOT NULL,
	  Lander varchar(16) NOT NULL,
	  `Map` varchar(16) NOT NULL,
	  GameActive BOOLEAN DEFAULT FALSE NOT NULL,
	  PositionX float NOT NULL,
	  PositionY float NOT NULL,
	  VelocityX float NOT NULL,
	  VelocityY float NOT NULL,
	  DeltaFuel int NOT NULL,
	  Rotation float NOT NULL,
	  Rocket BOOLEAN DEFAULT FALSE NOT NULL,
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

	INSERT INTO tblCollisionOffset(x,y) VALUES (-1,-1), (0,-1), (1,-1), (-1,0), (0,0), (1,0), (-1,1), (0,1), (1,1);
END //
DELIMITER ;



DELIMITER //
DROP PROCEDURE IF EXISTS generateMap//

CREATE PROCEDURE generateMap(mapName varchar(16), mapSize int, seed int, gravity int)
BEGIN
	INSERT INTO tblMap(Name, Gravity) VALUES (mapName, gravity);
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

DELIMITER //
DROP PROCEDURE IF EXISTS getChat//

CREATE PROCEDURE getChat()
BEGIN
    SELECT * FROM tblglobalchat
    ORDER BY time DESC
    LIMIT 20;
END //

DELIMITER //
DROP PROCEDURE IF EXISTS getMap//

CREATE PROCEDURE getMap(mapName varchar(16))
BEGIN
    SELECT * FROM tblTile
    WHERE Map = mapName;
END //

DELIMITER ;


DELIMITER //
DROP PROCEDURE IF EXISTS sendChat//

CREATE PROCEDURE sendChat(user varchar(16), txt varchar(255))
BEGIN
	DECLARE EXIT HANDLER FOR 1452
    BEGIN
 	    SELECT 'CONSTRAINTFAILURE' AS 'STATUS';
    END;

    INSERT INTO tblGlobalChat(Player, Time, Message)
    VALUES (user, NOW(), txt);
    SELECT 'OK' AS 'STATUS';
END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS getLanders//

CREATE PROCEDURE getLanders()
BEGIN
    SELECT * FROM tblLander
    ORDER BY Name DESC;
END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS getMaps//

CREATE PROCEDURE getMaps()
BEGIN
    SELECT * FROM tblMap
    ORDER BY Name DESC;
END //

DELIMITER ;


DELIMITER //
DROP PROCEDURE IF EXISTS  deleteChatAdmin//

CREATE PROCEDURE deleteChatAdmin(user varchar(16), pass varchar(32), messageID int)
BEGIN
    IF verifyAdmin(user, pass)
    THEN
        DELETE FROM tblglobalchat WHERE ID = messageID;
        SELECT 'OK' AS 'STATUS';
    ELSE
        SELECT 'NOACCESS' AS 'STATUS';
    END IF;
END //

DELIMITER ;

# DELIMITER //
# DROP PROCEDURE IF EXISTS  deleteMapAdmin//
#
# CREATE PROCEDURE deleteMapAdmin(user varchar(16), pass varchar(32), mapname varchar(16))
# BEGIN
#     IF verifyAdmin(user, pass)
#     THEN
#         DELETE FROM tblMap WHERE Name = mapname;
#         SELECT 'OK' AS 'STATUS';
#     ELSE
#         SELECT 'NOACCESS' AS 'STATUS';
#     END IF;
# END //
#
# DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS  deleteUserAdmin//

CREATE PROCEDURE deleteUserAdmin(user varchar(16), pass varchar(32), targetname varchar(16))
BEGIN
    IF verifyAdmin(user, pass)
    THEN
        DELETE FROM tblAccount WHERE Username = targetname;
        SELECT 'OK' AS 'STATUS';
    ELSE
        SELECT 'NOACCESS' AS 'STATUS';
    END IF;
END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS  updateUserAdmin//

CREATE PROCEDURE updateUserAdmin(user varchar(16),
	pass varchar(32),
	targetOldName varchar(16),
	targetNewName varchar(16),
	targetPass varchar(32),
	targetIs_Admin BOOLEAN,
	targetHighScore int,
	targetLoginAttempts int,
	targetLocked boolean
	)
BEGIN
    IF verifyAdmin(user, pass)
    THEN
		UPDATE tblAccount
        SET
            Username = targetNewName,
			Password = targetPass,
			is_Admin = targetIs_Admin,
			HighScore = targetHighScore,
			LoginAttempts = targetLoginAttempts,
			Locked = targetLocked
        WHERE
            Username = targetOldName;
        SELECT 'OK' AS 'STATUS';
    ELSE
        SELECT 'NOACCESS' AS 'STATUS';
    END IF;
END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS newUserAdmin//

CREATE PROCEDURE newUserAdmin(user varchar(16),
    pass varchar(32),
    targetName varchar(16),
	targetPass varchar(32),
	targetIs_Admin BOOLEAN,
	targetHighScore int,
	targetLoginAttempts int,
	targetLocked boolean
    )
BEGIN

    IF verifyAdmin(user, pass)
    THEN
        IF NOT EXISTS(SELECT * FROM tblAccount WHERE Username = targetName)
        THEN
            INSERT INTO tblAccount(Username, Password, is_Admin,HighScore,LoginAttempts,Locked)
            VALUES (targetName, targetPass,targetIs_Admin,targetHighScore,targetLoginAttempts,targetLocked);
            SELECT "OK" AS "STATUS";
        ELSE
            SELECT "EXISTS" AS "STATUS";
        END IF;
    ELSE
        SELECT "NOACCESS" AS "STATUS";
    END IF;



END //


-- DELIMITER //
-- DROP FUNCTION IF EXISTS collision//
--
-- CREATE FUNCTION collision(mapName varchar(16), posX int, posY int)
-- RETURNS BOOLEAN
-- DETERMINISTIC
-- BEGIN
--     SET @locations = JSON_ARRAY('[-1,-1]', '[0,-1]', '[1,-1]', '[-1,0]', '[0,0]', '[1,0]', '[-1, 1]', '[0, 1]', '[1, 1]');
--     SET @isColliding = FALSE;
--     SET @indx = 0;
--     locationLoop: WHILE (@indx < JSON_LENGTH(@locations))
--         DO
--
--         SET @offsetIndx = JSON_UNQUOTE(JSON_EXTRACT(@locations, CONCAT('$[', @indx, ']')));
--         SET @offsetX = JSON_EXTRACT(@offsetIndx, '$[0]');
--         SET @offsetY = JSON_EXTRACT(@offsetIndx, '$[1]');
--
--         SET @relPosX = posX-@offsetX;
--         SET @relPosY = posY-@offsetY;
--
--
--         IF EXISTS(SELECT * FROM tbltile WHERE Map = mapName AND x = @relPosX AND y = @relPosY)
--         THEN
--             SET @isColliding = TRUE;
--             LEAVE locationLoop;
--         END IF;
--
--         SET @indx = @indx + 1;
--     END WHILE;
--
--     RETURN @isColliding;
--
-- END //
--
-- DELIMITER ;

DELIMITER //
DROP FUNCTION IF EXISTS collision//

CREATE FUNCTION collision(mapName varchar(16), posX int, posY int)
RETURNS BOOLEAN
DETERMINISTIC
BEGIN
    RETURN EXISTS(
            SELECT *
            FROM tbltile
            INNER JOIN tblCollisionOffset AS TCO
                ON tblTile.x = (posx-TCO.x) AND tblTile.Y = (posY-TCO.Y) AND tblTile.Map = mapName
        );

END //

DELIMITER ;



DELIMITER //
DROP PROCEDURE IF EXISTS controlRocket//

CREATE PROCEDURE controlRocket(engine bool, angle float,gameID int)
BEGIN
	SELECT Rotation
	INTO @rotation
	FROM tblGame
	WHERE ID = gameID;

	set @rotation = @rotation + angle;

	UPDATE tblGame
	SET
		Rotation = @rotation,
	    Rocket = engine
	WHERE
		ID = gameID;
	SELECT 'OK' AS 'STATUS';

END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS updateShip//

CREATE PROCEDURE updateShip(gameID int)
BEGIN

	SELECT Map, PositionX, PositionY, VelocityX, VelocityY, DeltaFuel, Rotation, Rocket, Gravity, Thrust, Weight, ScoreBonus, SpeedLimit
    INTO @mapName, @posX, @posY, @velX, @velY, @deltaFuel, @rotation, @engine, @gravity, @thrust, @weight, @scoreBonus, @SpeedLimit
	FROM tblGame
    INNER JOIN tblmap ON tblgame.Map = tblmap.Name
    INNER JOIN tbllander on tblgame.Lander = tbllander.Name
	WHERE ID = gameID;

	SET @velY = @velY - @gravity;
	SET @posX = @posX + @velX;
	SET @posY = @posY + @velY;
	SET @scoreCalc = @deltaFuel * @scoreBonus;
	IF NOT collision(@mapName,FLOOR(@posX), FLOOR(@posY))
	THEN
        IF @engine = TRUE AND @deltaFuel > 0
        THEN
            SET @velX = @velX + (@thrust * SIN(@rotation*-1));
            SET @velY = @velY + (@thrust * COS(@rotation));
            SET @deltaFuel = @deltaFuel - (@gravity * @weight) / 100;
        END IF;

        UPDATE tblGame
        SET
            DeltaFuel = @deltaFuel,
            PositionX = @posX,
            PositionY = @posY,
            VelocityX = @velX,
            VelocityY = @velY
        WHERE
            ID = gameID;
        SELECT @posX AS 'POSX', @posY AS 'POSY', @rotation AS 'ROTATION', @engine as 'ENGINE', 'OK' AS 'STATUS';
    ELSE
        IF @velX <= @speedLimit AND NOT @engine
        THEN
        	UPDATE tblGame
        	SET
            	GameActive = FALSE
        	WHERE
            	ID = gameID;
	    	SELECT 'SUCCESS' AS 'STATUS';
	    ELSE
	    	UPDATE tblGame
        	SET
            	GameActive = FALSE
        	WHERE
            	ID = gameID;
	    	SELECT 'COLLISION' AS 'STATUS';
	    END IF;
    END IF;
END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS  endGame//

CREATE PROCEDURE endGame(gameID int)
BEGIN
    UPDATE tblGame
        SET
            GameActive = FALSE
        WHERE
            ID = gameID;
    SELECT 'OK' AS 'STATUS';
END //

DELIMITER ;


DELIMITER //
DROP FUNCTION IF EXISTS verifyAdmin//

CREATE FUNCTION verifyAdmin(user varchar(16), pass varchar(32))
RETURNS BOOLEAN
DETERMINISTIC
BEGIN

    IF EXISTS(SELECT * FROM tblAccount WHERE Username = user AND Password = pass)
    THEN
        SELECT is_Admin
        INTO @isAdmin
        FROM tblAccount
        WHERE Username = user AND Password = pass
        LIMIT 1;
    ELSE
        SET @isAdmin = FALSE;
    end if;

	RETURN @isAdmin;

END //

DELIMITER ;


DELIMITER //
DROP PROCEDURE IF EXISTS listUsersAdmin//

CREATE PROCEDURE listUsersAdmin(user varchar(16), pass varchar(32))
BEGIN
	SET @isAdmin = verifyAdmin(user, pass);
	IF @isAdmin
	THEN
		SELECT
	       tblAccount.*,
	       COALESCE(GameActive,FALSE) AS 'InGame',
	       'OK' AS 'STATUS'
        FROM tblAccount
        LEFT JOIN (SELECT * FROM tblGame WHERE GameActive = TRUE) GA
        on GA.Player = tblAccount.Username;
	ELSE
		SELECT 'NOACCESS' AS 'STATUS';
	END IF;

END //

DELIMITER ;


DELIMITER //
DROP PROCEDURE IF EXISTS listUsers//

CREATE PROCEDURE listUsers()
BEGIN
	SELECT
	       Username,
	       HighScore,
	       Online,
	       COALESCE(GameActive,FALSE) AS 'InGame',
	       'OK' AS 'STATUS'
	FROM tblAccount
	LEFT JOIN (SELECT * FROM tblGame WHERE GameActive = TRUE) GA
    on GA.Player = tblAccount.Username;
END //

DELIMITER ;



DELIMITER //
DROP PROCEDURE IF EXISTS getUserByNameAdmin//

CREATE PROCEDURE getUserByNameAdmin(user varchar(16), pass varchar(32), target varchar(16))
BEGIN
	SET @isAdmin = verifyAdmin(user, pass);
	IF @isAdmin
	THEN
		SELECT *, 'OK' AS 'STATUS' from tblAccount
	    WHERE Username = target;
	ELSE
		SELECT 'NOACCESS' AS 'STATUS';
	END IF;

END //

DELIMITER ;




DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS userLogin//

CREATE PROCEDURE userLogin(user varchar(16), pass varchar(32))
BEGIN
	IF EXISTS(SELECT * FROM tblAccount WHERE Username = user)
	    THEN
            IF NOT (SELECT Locked FROM tblAccount WHERE Username = user)
            THEN
                IF EXISTS(SELECT * FROM tblAccount WHERE Username = user AND Password = pass)
                THEN
                    UPDATE tblAccount
                    SET
                        Online = TRUE
                    WHERE
                        Username = user;
                    SELECT "OK" AS "STATUS", verifyAdmin(user,pass) AS "ADMIN";
                ELSE
                    SELECT LoginAttempts
                    INTO @attempts
                    FROM tblAccount
                    WHERE Username = user;

                    SET @attempts = @attempts + 1;

                    IF @attempts >= 5
                    THEN
                        UPDATE tblAccount
                        SET
                            Locked = true
                        WHERE Username = user;
                    ELSE
                        UPDATE tblAccount
                        SET
                            LoginAttempts = @attempts
                        WHERE Username = user;
                    END IF;

                    SELECT "BADPASS" AS "STATUS", @attempts AS "ATTEMPTS";
                END IF;
            ELSE
                SELECT "LOCKED" AS "STATUS";
            END IF;
    ELSE
	    SELECT "NOUSER" AS "STATUS";
    END IF;

END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS userLogout//

CREATE PROCEDURE userLogout(user varchar(16), pass varchar(32))
BEGIN
	IF EXISTS(SELECT * FROM tblAccount WHERE Username = user AND Password = pass)
	THEN
	    IF EXISTS(SELECT * FROM tblAccount WHERE Username = user AND Online = TRUE)
	    THEN
	        UPDATE tblAccount
	        SET
	            Online = FALSE
	        WHERE
	              Username = user;
	        SELECT "OK" AS "STATUS";
	    ELSE
	        SELECT "OFFLINE" AS "STATUS";
        END IF;
    ELSE
	    SELECT "NOUSER" AS "STATUS";
    END IF;

END //

DELIMITER ;


DELIMITER //
DROP PROCEDURE IF EXISTS newUser//

CREATE PROCEDURE newUser(user varchar(16), pass varchar(32))
BEGIN
	IF NOT EXISTS(SELECT * FROM tblAccount WHERE Username = user)
	THEN
        INSERT INTO tblAccount(Username, Password) VALUES (user, pass);
        SELECT "OK" AS "STATUS";
    ELSE
	    SELECT "EXISTS" AS "STATUS";
    END IF;

END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS newGame//

CREATE PROCEDURE newGame(landerName varchar(16), playerName varchar(16), mapName varchar(16))
BEGIN
    DECLARE EXIT HANDLER FOR 1452
    BEGIN
 	    SELECT 'CONSTRAINTFAILURE' AS 'STATUS';
    END;
    SELECT Fuel
    INTO @deltaFuel
    FROM tblLander
    WHERE Name = landerName;
    INSERT INTO tblGame(Player, Lander, Map, GameActive, PositionX, PositionY, VelocityX, VelocityY, Rotation, DeltaFuel)
    VALUES (playerName, landerName, mapName, TRUE, 0, 200, 0, 0, -90, @deltaFuel);
    SELECT LAST_INSERT_ID() AS 'GameID', 'OK' AS 'STATUS';
END //

DELIMITER ;

DELIMITER //
DROP PROCEDURE IF EXISTS insertSchema//

CREATE PROCEDURE insertSchema()
BEGIN
	CALL generateMap('moon', 100, 482742, 3); -- generates map using fancy sine function and populates it with tiles
	CALL generateMap('mars', 200, 235263, 20); -- generates map using fancy sine function and populates it with tiles
	CALL generateMap('venus', 400, 384572, 60); -- generates map using fancy sine function and populates it with tiles

	INSERT INTO tblAccount(Username, Password, is_Admin) VALUES ("Admin", "Pass", TRUE); -- creates account

	insert into tblAccount (Username, Password) values ('sothick0', '8JCNlJ');
	insert into tblAccount (Username, Password) values ('mwybourne1', 'Er5krkYiq9M');
	insert into tblAccount (Username, Password) values ('ebraidman2', 'EGUTSCzNXC');
	insert into tblAccount (Username, Password) values ('sfragino3', 'QdYZoEu');
	insert into tblAccount (Username, Password) values ('gmasi4', 'wKDZxSUDlpw4');
	insert into tblAccount (Username, Password) values ('pbirtwell5', 'PBYtsInm');
	insert into tblAccount (Username, Password) values ('hrubinsohn6', 'XsDiYQDwZL2');
	insert into tblAccount (Username, Password) values ('abravington7', 'sCE7PqQ6Tw');
	insert into tblAccount (Username, Password) values ('rdunbleton8', 'WCN7cl');
	insert into tblAccount (Username, Password) values ('ksor9', '1rMYzLFe02FX');
	insert into tblAccount (Username, Password) values ('cotya', '3H8DX7Rh7PLS');
	insert into tblAccount (Username, Password) values ('celtb', 'f2kkcH7TK');
	insert into tblAccount (Username, Password) values ('bwilliamsc', 'Jz90JM0xn');
	insert into tblAccount (Username, Password) values ('kmcalisterd', 'sMUIWcovW');
	insert into tblAccount (Username, Password) values ('cconnerlye', '5DWjJM');
	insert into tblAccount (Username, Password) values ('mbridatf', 'n1wnnOjEFAmJ');
	insert into tblAccount (Username, Password) values ('lbertomieug', 'y9fsegcPr3B');
	insert into tblAccount (Username, Password) values ('rmclenahanh', 'dCOQYikBGbk');
	insert into tblAccount (Username, Password) values ('wbrugmanni', 'QfCFZO4NBg');
	insert into tblAccount (Username, Password) values ('pburlessj', 'BHgBRy');
	insert into tblAccount (Username, Password) values ('poveringtonk', 'dSRpi08');
	insert into tblAccount (Username, Password) values ('rbeckerlegl', 'k2Z6HN6AO');
	insert into tblAccount (Username, Password) values ('coboylem', 'O4Esmq');
	insert into tblAccount (Username, Password) values ('eboothn', 'vPNuKYrp');
	insert into tblAccount (Username, Password) values ('pharrieso', 'Ax7040Fn8Pl');
	insert into tblAccount (Username, Password) values ('kgosswellp', 'dE1sD114SQc');
	insert into tblAccount (Username, Password) values ('singlesonq', 'bmOqp87rPFN');
	insert into tblAccount (Username, Password) values ('sbickstethr', 'K9lLUZSnx');
	insert into tblAccount (Username, Password) values ('dverbekes', '5FDsYg9mYB0x');
	insert into tblAccount (Username, Password) values ('cpardoet', 'biyyNeDsBVM');
	insert into tblAccount (Username, Password) values ('wnaniu', 'SDSuB9');
	insert into tblAccount (Username, Password) values ('ccockinv', 'mEq5V9O');
	insert into tblAccount (Username, Password) values ('achastanetw', 'TlQ3cl1soP');
	insert into tblAccount (Username, Password) values ('ssmalex', 'VUJzlAJe');
	insert into tblAccount (Username, Password) values ('kblewisy', 'm7rc2hB29');
	insert into tblAccount (Username, Password) values ('dgillisez', 'IAfNgA');
	insert into tblAccount (Username, Password) values ('kbegwell10', 'zJoDoWJP');
	insert into tblAccount (Username, Password) values ('vvant11', '1XFxovaj5R8c');
	insert into tblAccount (Username, Password) values ('wdukelow12', 'alGlxT');
	insert into tblAccount (Username, Password) values ('bwoodyer13', 'HvUfSFh');
	insert into tblAccount (Username, Password) values ('lshearstone14', 'aW1slu0');
	insert into tblAccount (Username, Password) values ('ekohen15', 'cSTnv2i');
	insert into tblAccount (Username, Password) values ('crubberts16', '4cciAQno0OK');
	insert into tblAccount (Username, Password) values ('arigby17', 'aMIKHmP5DAo');
	insert into tblAccount (Username, Password) values ('dderdes18', '2u4N5XRZj');
	insert into tblAccount (Username, Password) values ('kbevis19', 'NV4unXe49x');
	insert into tblAccount (Username, Password) values ('gjackways1a', '671GBZ1e');
	insert into tblAccount (Username, Password) values ('bhounsome1b', 'cwgF8C');
	insert into tblAccount (Username, Password) values ('tcumbridge1c', 'hnkWVL');
	insert into tblAccount (Username, Password) values ('ccliff1d', 'LT4shMnQ2P');
	insert into tblAccount (Username, Password) values ('jtriplow1e', 'LkbqNTuG');
	insert into tblAccount (Username, Password) values ('dsidnall1f', 'ByBZKgcLfWYa');
	insert into tblAccount (Username, Password) values ('ksproule1g', 'AdlYX0qL3');
	insert into tblAccount (Username, Password) values ('hrippon1h', 'TTYTrub66LZ');
	insert into tblAccount (Username, Password) values ('cbristo1i', 'Jg8XS9sh');
	insert into tblAccount (Username, Password) values ('bbento1j', 'zlq9haHCTic');
	insert into tblAccount (Username, Password) values ('fjurges1k', 'PzKSD77BZ');
	insert into tblAccount (Username, Password) values ('cwillett1l', 'UTtEM1J');
	insert into tblAccount (Username, Password) values ('ivallentin1m', '2jV1rYo');
	insert into tblAccount (Username, Password) values ('bgroom1n', 'ytOwGRdz');
	insert into tblAccount (Username, Password) values ('skiera1o', 'amfEFZn6Uv8');
	insert into tblAccount (Username, Password) values ('sfarny1p', 'CThE2ZysB');
	insert into tblAccount (Username, Password) values ('jlippi1q', 'Bv7TI9');
	insert into tblAccount (Username, Password) values ('kbreydin1r', 'ZQoUD4');
	insert into tblAccount (Username, Password) values ('sdumphrey1s', 'kRmfi9');
	insert into tblAccount (Username, Password) values ('rrumin1t', 'NRJ5gaJZUNvo');
	insert into tblAccount (Username, Password) values ('bborland1u', 'gjCaii');
	insert into tblAccount (Username, Password) values ('mgarmon1v', '4AjgzDfQi');
	insert into tblAccount (Username, Password) values ('lswiffen1w', 'x3z59kRjLzmE');
	insert into tblAccount (Username, Password) values ('fsitford1x', '3FRzMyF');
	insert into tblAccount (Username, Password) values ('sovitts1y', 'UbDOak3k8x');
	insert into tblAccount (Username, Password) values ('hhandaside1z', 'OYgQLRl');
	insert into tblAccount (Username, Password) values ('swindley20', 'HwmbWrf9mAP');
	insert into tblAccount (Username, Password) values ('bcarik21', 'Ukumlt');
	insert into tblAccount (Username, Password) values ('fmcnabb22', 'MMcTWybJFXO');
	insert into tblAccount (Username, Password) values ('relegood23', 'azdgBqM2lqqs');
	insert into tblAccount (Username, Password) values ('mcircuitt24', 't1wdJz');
	insert into tblAccount (Username, Password) values ('schalk25', 'xQNsQ2EM');
	insert into tblAccount (Username, Password) values ('sborsnall26', 'ZqFjdG0Xo');
	insert into tblAccount (Username, Password) values ('rmitrikhin27', 'dyYzEZ0uSb');
	insert into tblAccount (Username, Password) values ('sgrimsell28', 'XrDDCuC');
	insert into tblAccount (Username, Password) values ('jmarjanski29', 'i4vTPF');
	insert into tblAccount (Username, Password) values ('franby2a', 'y6LwmX2i');
	insert into tblAccount (Username, Password) values ('lstoodley2b', 'x25NrB0fhJv');
	insert into tblAccount (Username, Password) values ('sreeme2c', 'mL7jbretBOJ');
	insert into tblAccount (Username, Password) values ('aovens2d', 'AVubJJI4lO');
	insert into tblAccount (Username, Password) values ('gleif2e', 'nvicnYzFSw8');
	insert into tblAccount (Username, Password) values ('cbarhams2f', 'NyLyRN');
	insert into tblAccount (Username, Password) values ('skenworthey2g', 'IcnQ48etP0');
	insert into tblAccount (Username, Password) values ('gohdirscoll2h', 'hMNJVM0qHf');
	insert into tblAccount (Username, Password) values ('gfear2i', 'LAcuF7a2aF3');
	insert into tblAccount (Username, Password) values ('cbercevelo2j', 'hnXwK7DK');
	insert into tblAccount (Username, Password) values ('tworcs2k', 'Jhbxh6');
	insert into tblAccount (Username, Password) values ('cvallis2l', 'sHnFoLuzWF');
	insert into tblAccount (Username, Password) values ('bcoucher2m', '1PVoYk');
	insert into tblAccount (Username, Password) values ('mmulholland2n', 'L29y0A');
	insert into tblAccount (Username, Password) values ('ndunbabin2o', 'JAamYpS');
	insert into tblAccount (Username, Password) values ('dwingeat2p', 'yYxVxQX');
	insert into tblAccount (Username, Password) values ('cbrownlie2q', 'OC3p8J0ea');
	insert into tblAccount (Username, Password) values ('dtennison2r', 'WQl2fobVcN');


	INSERT INTO tblLander(Name, Weight, ScoreBonus, Fuel, Thrust) VALUES ("Apollo", 10, 2, 3000, 200); -- creates a lander
	INSERT INTO tblLander(Name, Weight, ScoreBonus, Fuel, Thrust) VALUES ("Mars Rover", 2, 1, 300, 120); -- creates a lander
	INSERT INTO tblLander(Name, Weight, ScoreBonus, Fuel, Thrust) VALUES ("Helios", 4, 1, 600, 160); -- creates a lander


	INSERT INTO tblGame(Player, Lander, Map, PositionX, PositionY, VelocityX, VelocityY, Rotation, DeltaFuel) VALUES ("tworcs2k", "Apollo", "moon", 0, 200, 100, 0, 90, 0); -- creates a game
	INSERT INTO tblGame(Player, Lander, Map, PositionX, PositionY, VelocityX, VelocityY, Rotation, DeltaFuel) VALUES ("cbarhams2f", "Helios", "moon", 0, 200, 100, 0, 90, 0); -- creates a game
	INSERT INTO tblGame(Player, Lander, Map, PositionX, PositionY, VelocityX, VelocityY, Rotation, DeltaFuel) VALUES ("lswiffen1w", "Mars Rover", "mars", 0, 300, 200, 0, 90, 0); -- creates a game


	INSERT INTO tblGlobalChat(Player, Time, Message) VALUES ("tworcs2k", "2017-08-15 19:30:10", "Hello"); -- creates a new chat message
	INSERT INTO tblGlobalChat(Player, Time, Message) VALUES ("cbarhams2f", '2020-12-05 04:25:36', 'School Ties'); -- creates a new chat message
	INSERT INTO tblGlobalChat(Player, Time, Message) VALUES ("lswiffen1w", '2021-01-03 19:59:27', 'Dillinger'); -- creates a new chat message
	INSERT INTO tblGlobalChat(Player, Time, Message) VALUES ("gleif2e", '2020-12-30 20:47:01', 'Paper Heart'); -- creates a new chat message-- creates a new chat message


END //
DELIMITER ;



CALL createSchema();  -- creates tables
CALL insertSchema();
