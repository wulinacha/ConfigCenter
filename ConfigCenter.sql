-- --------------------------------------------------------
-- 主机:                           119.147.171.113
-- 服务器版本:                        5.7.9-log - MySQL Community Server (GPL)
-- 服务器操作系统:                      linux-glibc2.5
-- HeidiSQL 版本:                  9.5.0.5291
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- 导出 ConfigCenter 的数据库结构
CREATE DATABASE IF NOT EXISTS `ConfigCenter` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ConfigCenter`;

-- 导出  表 ConfigCenter.Account 结构
CREATE TABLE IF NOT EXISTS `Account` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) DEFAULT NULL,
  `Password` varchar(64) DEFAULT NULL,
  `RoleId` int(11) DEFAULT NULL,
  `Salt` varchar(64) DEFAULT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。
-- 导出  表 ConfigCenter.App 结构
CREATE TABLE IF NOT EXISTS `App` (
  `id` int(11) unsigned NOT NULL AUTO_INCREMENT,
  `AppId` varchar(50) DEFAULT NULL,
  `Version` varchar(50) DEFAULT NULL,
  `Environment` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=66 DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。
-- 导出  表 ConfigCenter.AppSetting 结构
CREATE TABLE IF NOT EXISTS `AppSetting` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AppId` int(11) DEFAULT NULL,
  `ConfigKey` varchar(50) DEFAULT NULL,
  `ConfigValue` text,
  `ConfigType` int(11) NOT NULL,
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=2992256 DEFAULT CHARSET=utf8;

-- 数据导出被取消选择。
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
