# 配置中心使用说明

## 配置中心简介
为了解决项目中配置零散、不统一，容易导致变更配置而引发”牵一发而动全身“等一系列问题，因此使用配置中心，实现系统配置信息统一管理；

## 配置中心原理
配置中心客户端远程调用配置中心服务器Api，获取配置到内存中，以及版本信息，同时下载配置到客户端，此处的文件作为备份应急使用，防止服务在链接不上远程配置中心服务器导致服务无法启动的现象发生，此处文件不能任意修改（后面会加密处理），内存中的配置以json的格式保存；配置中心客户端会保持10秒一次的心跳活动，监控地址版本变更，如果版本改变，则重新下载；

## 配置环境和版本
配置中心配置共分为三个环境，DEV(开发环境)、ALPHA(测试环境)、PROD(正式环境)，每次更新都会有一个版本号，版本号由当前时间生成，精确到秒;

## 配置中心使用说明
1、客户端，引用Zhidian.ConfigAgent项目，使用方法如下，例如：
``` cs
//定义一个名称与配置Key对应的类,例如：
 public class RedisConfig
 {
        /// <summary>
        /// ERP Redis配置
        /// </summary>
        public RedisConfigItem Erp { get; set; }
}

var redisConfig = Configuration<RedisConfig>.Instance;//配置中心获取配置
redisConfig.Erp.Hosts
```

2、项目中的配置，在web.config或者app.config中都是在<configuration>节下配置，配置信息例子如下：
``` cs
 <appSettings>
    <add key="ProjectName" value="Quote"/>
    <add key="Environment" value="Dev"/>
    <add key="ServiceUrl" value="http://192.168.199.15:8092" />
  </appSettings>
```
ProjectName表示应用名称，Environment表示环境，ServiceUrl表示配置中心连接地址；

3、Web端，进入界面，如果该项目是新项目，请使用账号：admin 密码：admin登录，进入界面创建应用，之后登录账号和密码都是应用名称；用应用名称登录后只能看到自己应用的配置；

## 部署
开发环境Web地址：http://192.168.199.15:8090/
开发环境Api地址：http://192.168.199.15:8092/

测试环境Web地址：http://192.168.100.3:8090
测试环境Api地址：http://192.168.100.3:8092

正式环境Web地址：http://192.168.1.25:8090
正式环境Api地址：http://192.168.1.25:8091

仿真环境Web地址：http://192.168.1.235:8090
仿真环境Api地址：http://192.168.1.235:8092


