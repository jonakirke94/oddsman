CREATE TABLE `gotomain_net_db2`.`Users` (
  `UserId` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  `Tag` VARCHAR(45) NOT NULL,
  `Email` VARCHAR(45) NOT NULL,
  `Password` VARCHAR(45) NOT NULL,
  `RefreshToken` VARCHAR(45) NULL,
  PRIMARY KEY (`UserId`));
