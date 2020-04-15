using System;
using System.Collections.Generic;
using Dos.Tools;
using Dos.Tools.Model;

namespace Dos.ORM.Tool.Model
{
    public class ModelEntity
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string TableName { get; set; }
        public List<ColumnInfo> Columns { get; set; }
        public string NameSpace { get; set; }
        public List<ColumnInfo> PrimaryKeyColumns { get; set; }
        public ColumnInfo IdentityColumn { get; set; }
        public int i1 { get; set; }
        public int i2 { get; set; }
        public int i3 { get; set; }

        public string MakeCode()
        {
            StringPlus plus = new StringPlus();
            plus.AppendSpaceLine(0, "using System;");
            plus.AppendSpaceLine(0, "using Dos.ORM;");
            plus.AppendLine();
            plus.AppendSpaceLine(0, "namespace " + NameSpace);
            plus.AppendSpaceLine(0, "{");
            plus.AppendSpaceLine(1, "/// <summary>");
            plus.AppendSpaceLine(1, "/// 实体类"+ClassName+")。(属性说明自动提取数据库字段的描述信息)");
            plus.AppendSpaceLine(1, "/// </summary>");
            plus.AppendSpaceLine(1, "[Table(\"" + TableName + "\")]");
            plus.AppendSpaceLine(1, "[Serializable]");
            plus.AppendSpaceLine(1, "public partial class " + ClassName + " : Entity");
            plus.AppendSpaceLine(1, "{");
            plus.AppendSpaceLine(2, "#region Model");
            foreach (var item in Columns)
            {
                plus.AppendSpaceLine(2, "private " + item.TypeName + " _" + item.ColumnName + ";");
            }

            foreach (var item in Columns)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// " + item.DeText);
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "[Field(\"" + item.ColumnNameRealName + "\")]");
                plus.AppendSpaceLine(2, "public " + item.TypeName + " " + item.ColumnName);
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "get{ return _"+item.ColumnName+"; }");
                plus.AppendSpaceLine(3, "set");
                plus.AppendSpaceLine(3, "{");
                plus.AppendSpaceLine(4, "this.OnPropertyValueChange(\"" + item.ColumnNameRealName + "\");");
                plus.AppendSpaceLine(4, "this._" + item.ColumnName + " = value;");
                plus.AppendSpaceLine(3, "}");
                plus.AppendSpaceLine(3, "}");
            }
            plus.AppendSpaceLine(2, "#endregion");
            plus.AppendLine();
            plus.AppendSpaceLine(2, "#region Method");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取实体中的主键列");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public override Field[] GetPrimaryKeyFields()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return new Field[] {");
            foreach (var item in PrimaryKeyColumns)
            {
                plus.AppendSpaceLine(4, "_." + item.ColumnName + ",");
            }
            plus.AppendSpaceLine(4, "};");
            plus.AppendSpaceLine(2, "}");
            if (IdentityColumn != null)
            {
                plus.AppendSpaceLine(2, "/// <summary>");
                plus.AppendSpaceLine(2, "/// 获取实体中的标识列");
                plus.AppendSpaceLine(2, "/// </summary>");
                plus.AppendSpaceLine(2, "public override Field GetIdentityField()");
                plus.AppendSpaceLine(2, "{");
                plus.AppendSpaceLine(3, "return _." + IdentityColumn.ColumnName + ";");
                plus.AppendSpaceLine(2, "}");
            }
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取列信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public override Field[] GetFields()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return new Field[] {");
            foreach (var item in Columns)
            {
                plus.AppendSpaceLine(4, "_." + item.ColumnName + ",");
            }
            plus.AppendSpaceLine(4, "};");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 获取值信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public override object[] GetValues()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return new object[] {");
            foreach (var item in Columns)
            {
                plus.AppendSpaceLine(4, "this._" + item.ColumnName + ",");
            }
            plus.AppendSpaceLine(4, "};");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 是否是v1.10.5.6及以上版本实体。");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "/// <returns></returns>");
            plus.AppendSpaceLine(2, "public override bool V1_10_5_6_Plus()");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "return true;");
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "#endregion");
            plus.AppendLine();
            plus.AppendSpaceLine(2, "#region _Field");
            plus.AppendSpaceLine(2, "/// <summary>");
            plus.AppendSpaceLine(2, "/// 字段信息");
            plus.AppendSpaceLine(2, "/// </summary>");
            plus.AppendSpaceLine(2, "public class _");
            plus.AppendSpaceLine(2, "{");
            plus.AppendSpaceLine(3, "/// <summary>");
            plus.AppendSpaceLine(3, "/// * ");
            plus.AppendSpaceLine(3, "/// </summary>");
            plus.AppendSpaceLine(3, "public readonly static Field All = new Field(\"*\", \"" + TableName + "\");");
            foreach (var item in Columns)
            {
                plus.AppendSpaceLine(3, "/// <summary>");
                plus.AppendSpaceLine(3, "/// " + item.DeText);
                plus.AppendSpaceLine(3, "/// </summary>");
                plus.AppendSpaceLine(3, "public readonly static Field " + item.ColumnName + " = new Field(\"" + item.ColumnNameRealName + "\", \"" + TableName + "\", \"" + item.DeText + "\");");
            }
            plus.AppendSpaceLine(2, "}");
            plus.AppendSpaceLine(2, "#endregion");
            plus.AppendSpaceLine(1, "}");
            plus.AppendSpaceLine(0, "}");
            return plus.ToString();
        }
    }
}
