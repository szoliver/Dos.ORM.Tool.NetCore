# Dos.Tool.NetCore
DOS.ORM实体生成工具NetCore版

## 关于RazorEngine的问题
我已经尽最大努力希望能够成功迁移RazorEngine，更好地实现模板生成的功能。<br>
但最终无法完成，原因：<br>
      RazorEngine不支持DotNet Core，运行时报错，引用了system.web.razor
      找到RazorEngine.NetCore，开始感觉很惊喜，最后很失望，抛出异常，无解，连Demo都过不了
      下载源码，发现它还引用了System.Runtime.Remoting，查资料被DotNet Core废弃
      但微软资料上3.0版本貌似可以查得到文档，以下版本直接提示不支持
根据实践经验，模板几乎不用调整，所以干脆手写拼接。。。将模板功能彻底放弃<br>
本源码基于Dotnet Core2.2，Ubutun19.04开发。
