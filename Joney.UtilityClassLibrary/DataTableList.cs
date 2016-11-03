using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Reflection;

namespace Joney.UtilityClassLibrary
{
    /// <summary>
    /// by:JONEY
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class DataTableList<T> where T : new()
    {
        /// <summary>
        /// 将List数据转化成DataTable数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable(IList list)
        {
            DataTable result = new DataTable();
            if (list.Count > 0)
            {
                try
                {
                    PropertyInfo[] propertys = list[0].GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        result.Columns.Add(pi.Name);//, pi.PropertyType
                    }
                    for (int i = 0; i < list.Count; i++)
                    {
                        ArrayList tempList = new ArrayList();
                        foreach (PropertyInfo pi in propertys)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        object[] array = tempList.ToArray();
                        result.LoadDataRow(array, true);
                    }
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                //BY:JONEY
                //Type type=typeof(T);
                //List<PropertyInfo> plist=new List<PropertyInfo>();
                //Array.ForEach<PropertyInfo>(type.GetProperties(), p => { plist.Add(p); result.Columns.Add(p.Name, p.PropertyType); });
                //foreach (var item in list)
                //{
                //    DataRow row = result.NewRow();
                //    plist.ForEach(p=>row[p.Name]=p.GetValue(item,null));
                //    result.Rows.Add(row);
                //}
            }
            return result;
        }

        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        public static IList<T> DataTableToDataList(DataTable dt)
        {
            IList<T> ts = new List<T>();
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType, CultureInfo.CurrentCulture), null);
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        public static IList<T> TableToList(DataTable dt)
        {
            IList<T> ts = new List<T>();
            if (dt==null||dt.Rows.Count==0)
            {
                return ts;
            }
            Type type = typeof(T);
            foreach (DataRow dr in dt.Rows)
            {
                object obj = System.Activator.CreateInstance(type);
                foreach (PropertyInfo pi in type.GetProperties())
                {
                    if (pi.PropertyType.IsPublic && pi.CanWrite && dt.Columns.Contains(pi.Name))
                    {
                        Type oType = Type.GetType(pi.PropertyType.FullName);
                        if (dr[pi.Name]!=string.Empty && dr[pi.Name]!=null && dr[pi.Name]!=DBNull.Value)
                        {
                            object value = Convert.ChangeType(dr[pi.Name],oType);
                            type.GetProperty(pi.Name).SetValue(obj,value,null);
                        }
                    }
                }
                ts.Add((T)obj);
            }
            return ts;
        }

        public static IList<T> ConverToModel(DataTable dt)
        {
            IList<T> ts = new List<T>();
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        if (!pi.CanWrite)
                        {
                            continue;
                        }
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                ts.Add(t);
            }
            return ts;
        }
    }
}
