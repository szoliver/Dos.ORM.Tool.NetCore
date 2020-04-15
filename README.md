# Dos.Tool.NetCore
DOS.ORM实体生成工具NetCore版

## 关于RazorEngine的问题 
现在我已经全线转到Linux平台下进行.Net开发，使用的是.NetCore<br>
DOS.ORM是一个很好用的工具，但是实体生成工具还得到Windows下处理，于是有了迁移到NetCore的想法<br>
保持原有配置文件，只处理配置的第一项参数，例如服务器选择，导出生成所有表和视图<br>
如果谁有时间，可以增加参数处理，让工具更方便，因为我常用的就是那个固定的模板并且只用Mysql，所以代码没有增加配置选项。<br>
### 基本满足“Mysql以最新模板导出数据库所有实体定义文件”，配置好数据库后直接执行即可
```
dotnet /path/yourapp.dll
```
我已经尽最大努力希望能够成功迁移RazorEngine，更好地实现模板生成的功能。<br>
但最终无法完成，原因：<br>
 + RazorEngine不支持DotNet Core，运行时报错，引用了system.web.razor
 + 找到RazorEngine.NetCore，开始感觉很惊喜，最后很失望，抛出异常，无解，连Demo都过不了
 + 下载源码，发现它还引用了System.Runtime.Remoting，查资料被DotNet Core废弃
 + 但微软资料上3.0版本貌似可以查得到文档，以下版本直接提示不支持
 + 根据实践经验，模板几乎不用调整，所以干脆手写拼接。。。将模板功能彻底放弃<br>
本源码基于Dotnet Core2.2，Ubutun19.10开发。
