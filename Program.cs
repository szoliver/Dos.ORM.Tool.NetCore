using System.Linq;
using System;
using System.Text;
using System.IO;
using Dos.DbObjects;
using Dos.Common;
using Dos.Tools;
using Dos.Tools.Model;
using System.Data;
using System.Collections.Generic;

namespace Dos.ORM.Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            //关于RazorEngine的问题
            //我已经尽最大努力希望能够成功迁移RazorEngine，更好地实现模板生成的功能。
            //但最终无法完成，原因：
            //      RazorEngine不支持DotNet Core，运行时报错，引用了system.web.razor
            //      找到RazorEngine.NetCore，开始感觉很惊喜，最后很失望，抛出异常，无解，连Demo都过不了
            //      下载源码，发现它还引用了System.Runtime.Remoting，查资料被DotNet Core废弃
            //      但微软资料上3.0版本貌似可以查得到文档，以下版本直接提示不支持
            //根据实践经验，模板几乎不用调整，所以干脆手写拼接。。。将模板功能彻底放弃
            //本源码基于Dotnet Core2.2，Ubutun19.04开发。
            //string TplContent = ""; //模板内容
            string databaseName = ""; //数据库名
            Connection connectionModel = null;
            Dictionary<string, bool> tableview = new Dictionary<string, bool>(); //所有表和视图
            string txtTableStar = "";
            //读取目录中所有模板文件列表
            //FileInfo[] tpls = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "Template")).GetFiles("*.tplx", SearchOption.AllDirectories);
            //选择第一个模板
            //var tpl = tpls[0].FullName;
            //TplContent = FileHelper.Read(tpl);

            #region 获取服务器列表，仅使用第一个
            List<Connection> list = Utils.GetConnectionList();
            connectionModel = list.ElementAt(0);
            databaseName = connectionModel.Database;
            Console.WriteLine("服务器信息：");
            Console.WriteLine(" 主机：" + connectionModel.Name);
            Console.WriteLine(" 数据库：" + connectionModel.Database);
            #endregion

            IDbObject dbObject = null;
            try
            {
                dbObject = new Dos.DbObjects.MySQL.DbObject(connectionModel.ConnectionString);
                Sysconfig sysconfigModel;

                sysconfigModel = Utils.GetSysconfigModel();
                string txtNamaspace = sysconfigModel.Namespace;
                DataTable tablesDT = dbObject.GetTables(databaseName);
                DataRow[] drs = tablesDT.Select("", "name asc");
                int tcnt = 0;
                if (null != drs && drs.Length > 0)
                {
                    tcnt = drs.Length;
                    foreach (DataRow dr in drs)
                    {
                        tableview.Add(dr[0].ToString(), false);
                    }
                }
                tablesDT = dbObject.GetVIEWs(databaseName);
                int vcnt = 0;
                drs = tablesDT.Select("", "name asc");
                if (null != drs && drs.Length > 0)
                {
                    vcnt = drs.Length;
                    foreach (DataRow dr in drs)
                    {
                        tableview.Add(dr[0].ToString(), true);
                    }
                }
                Console.WriteLine("将导出此库所有表和视图的实体定义，表：" + tcnt + "、视图：" + vcnt);
                #region 开始导出所有表和视图模型
                EntityBuilder builder;
                Console.WriteLine("********************************");
                foreach (var dr in tableview)
                {
                    string o = dr.Key;
                    var ro = !string.IsNullOrWhiteSpace(txtTableStar.Trim())
                        ? o.Trim().Replace(' ', '_').Replace(txtTableStar.Trim(), "")
                        : o.Trim().Replace(' ', '_');
                    builder = new EntityBuilder(o, txtNamaspace,
                        ro,
                        Utils.GetColumnInfos(dbObject.GetColumnInfoList(databaseName, o)),
                        dr.Value,
                        false, //首字母大写
                        connectionModel.DbType);
                    var path = AppContext.BaseDirectory + "DBModels/";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    using (StreamWriter sw = new StreamWriter(Path.Combine(path, ro + ".cs"), false, Encoding.UTF8))
                    {
                        Console.WriteLine("导出" + ro + "......OK!");
                        sw.Write(builder.Builder());
                        sw.Close();
                    }
                    System.Threading.Thread.Sleep(1);
                }
                Console.WriteLine("********************************");
                #endregion
                Console.WriteLine("实体文件生成完毕！\n按任意健退出");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("异常：" + ex.Message);
            }
        }
    }
}
