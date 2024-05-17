using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;

namespace DotNetCore_CDCD_Charging_Model.SugarEntity
{
    /// <summary>
    /// {{ModelDescription}}
    /// </summary>
    [SugarTable("{{ModelClassName}}")]
    public partial class {{ModelClassName}}_Sugar
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public {{ModelClassName}}_Sugar()
        {
        }
		{% for field in ModelFields %}
        /// <summary>
        /// {{field.ColumnDescription}}
        /// </summary>
        [Display(Name = "{{field.ColumnDescription}}")]
		{% if field.IsIdentity == true and field.IsPrimarykey == true %}
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        {% elsif  field.IsIdentity == true and field.IsPrimarykey == false %}
        [SugarColumn(IsIdentity = true)]
        {% elsif  field.IsIdentity == false and field.IsPrimarykey == true %}
        [SugarColumn(IsPrimaryKey = true)]
        {% else %}{% endif %}
        {% if field.IsNullable == false %}[Required(ErrorMessage = "请输入{0}")]{% endif %}
        {% if field.IsNullable == true %}[SugarColumn(IsNullable = true)]{% endif %}
        {% if field.DataType == 'nvarchar' and field.Length > 0 %}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超过{1}字")]{% endif %}
        {% if field.DataType == 'varchar' and field.Length > 0 %}[StringLength(maximumLength:{{field.Length}},ErrorMessage = "{0}不能超过{1}字")]{% endif %}
        {% if field.DataType == 'nvarchar' or field.DataType == 'varchar'  or field.DataType == 'text' %}
        public string {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'int' and field.IsNullable == false  %}
        public int {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'int' and field.IsNullable == true %}
        public int? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bigint' and field.IsNullable == false  %}
        public long {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bigint' and field.IsNullable == true %}
        public long? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'float' and field.IsNullable == false  %}
        public float {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'float' and field.IsNullable == true %}
        public float? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bit' and field.IsNullable == false %}
        public bool {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'bit' and field.IsNullable == true %}
        public bool? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'datetime' and field.IsNullable == false %}
        public DateTime {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'datetime' and field.IsNullable == true %}
        public DateTime? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'date' and field.IsNullable == false %}
        public DateTime {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'date' and field.IsNullable == true %}
        public DateTime? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'uniqueidentifier' and field.IsNullable == false %}
        public Guid {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'uniqueidentifier' and field.IsNullable == true %}
        public Guid? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'decimal' and field.IsNullable == false %}
        public decimal {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'decimal' and field.IsNullable == true %}
        public decimal? {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'numeric' and field.IsNullable == false %}
        public decimal {{field.DbColumnName}}  { get; set; }
        {% elsif  field.DataType == 'numeric' and field.IsNullable == true %}
        public decimal? {{field.DbColumnName}}  { get; set; }
        {% else %}
        {% endif %}
		{% endfor %}
    }
}
