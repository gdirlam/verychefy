using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.Mvc;
using System.Linq;
using System.Configuration;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using App_a_matic.Helper;
using App_a_matic.Orm;
using System.Web.Caching;

/*
ToDo:
 * Decoupling....
 * Log and Caching can not be injected. Because of static nature of 
 * extension methods. They must be mocked, or set to off.
 * ? Is there anyway Query could be overloaded for Func, in lieu of Touple?
*/

namespace App_a_matic.Helper {

    static public class DataExtensions {

        static private string Connstring() {
            return ConfigurationManager.ConnectionStrings["App_a_matic"].ToString();
        }

        #region "Class AutoMappers"

        static public void AutoMapFromDictionary(this IOrmModel me, Dictionary<string, object> row) {
            string _message = string.Format("Deserialize of object {0} Failed", me.ToString());
            try {
                foreach (KeyValuePair<string, object> Field in row) {
                    _message = string.Format("Deserialize/Cast of Object '{0}' Failed at Property '{1}' with Value '{2}'", me.ToString(), Field.Key, Field.Value);
                    System.Object[] intVal = { Field.Value };

                    me.GetType().InvokeMember(Field.Key, System.Reflection.BindingFlags.SetProperty, Type.DefaultBinder, me, intVal);
                }
            }
            catch (Exception ex) {
                //err.WriteError(_message, ProcessHelper.GetProcessName(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType), ex);
            }
        }

        static public Dictionary<string, object> ToDataRow(this System.Data.SqlClient.SqlDataReader me) {
            string _message = string.Format("ToDictionary Call Failed");
            try {
                return Enumerable.Range(0, me.FieldCount).ToDictionary(field => me.GetName(field).ToString(), field => me.GetValue(field));
            }
            catch (Exception ex) {
                // err.WriteError(_message, ProcessHelper.GetProcessName(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType), ex);
            }
            return new Dictionary<string, object>();
        }

        static public Dictionary<string, object> ToDataRow(this System.Data.Common.DbDataRecord me) {
            string _message = string.Format("ToDictionary Call Failed");
            try {
                return Enumerable.Range(0, me.FieldCount).ToDictionary(field => me.GetName(field).ToString(), field => me.GetValue(field));
            }
            catch (Exception ex) {
                // err.WriteError(_message, ProcessHelper.GetProcessName(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType), ex);
            }
            return new Dictionary<string, object>();
        }

        #endregion

        #region "CRUD and Query"

        public static T Get<T> (this IOrmModel me, int id) where T: IOrmModel, new() {
            KeyValuePair<string, int?> _PrimaryKey = ReflectionHelper.PrimaryKey(me);
            string ProcessName = ReflectionHelper.GetProcessName(me);
            string _message = string.Format("Process {0} failed;", ProcessName);
            _message = string.Format("Process {0} failed; {1}={2}", ProcessName, _PrimaryKey.Key, _PrimaryKey.Value);
            string _cacheName = string.Format("{0}/{1}", ProcessName, _PrimaryKey.Value);

            if (!me.CacheStatus(_cacheName)) {
                try {
                    using (SqlConnection cn = new SqlConnection(Connstring())) {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand(me.OrmContext.Read, cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue(_PrimaryKey.Key, id);
                        SqlDataReader r = cmd.ExecuteReader();
                        while (r.Read()) {
                            Dictionary<string, object> row = r.ToDataRow();
                            IOrmModel item = (IOrmModel) new T();
                            item.AutoMapFromDictionary(row);
                            OrmCaching.SetCache(_cacheName, item);
                            return (T) item;                              
                        }
                        while (r.NextResult()) ;//get Error... Foobar..
                        cn.Close();
                    }
                }
                catch (SqlException ex) {
                    // err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                    throw;
                }
                catch (Exception ex) {
                    // err.WriteError(_message, GetProcessName(me), ex);
                }
                return (T) me; 
            }
            else {
                return (T)OrmCaching.GetCache(_cacheName);
            }
        }

        public static List<T> GetCollection<T>(this IOrmModel me) where T : IOrmModel, new() {
            string _message = string.Format("Process {0} failed;", ReflectionHelper.GetProcessName(me));
            string _cacheName = ReflectionHelper.GetProcessName(me);
            List<T> retval = new List<T>();
            object sync = new object();
            if (!me.CacheStatus(_cacheName)) {
                try {
                    using (SqlConnection cn = new SqlConnection(Connstring())) {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand(me.OrmContext.Read, cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader r = cmd.ExecuteReader();

                        while (r.Read()) {
                            Dictionary<string, object> row = r.ToDataRow();
                            IOrmModel item = (IOrmModel)new T();
                            item.AutoMapFromDictionary(row);
                            retval.Add((T) item);
                        }
                        while (r.NextResult()) ;//get Error... Foobar..
                        cn.Close();
                    }
                    if (retval.Count > 0) {
                        OrmCaching.SetCache(_cacheName, me);
                    }
                }
                catch (SqlException ex) {
                    // err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                    throw;
                }
                catch (Exception ex) {
                    // err.WriteError(_message, GetProcessName(me), ex);
                }
                return retval;
            }
            else {
                return (List<T>)OrmCaching.GetCache(_cacheName);
            }
        }

        //as a general rule of thumb, consider the parallel library as being faster on 
        //recordsets of greater then 100 rows, don't use this for small datasets.
        public static List<T> GetInParallel<T>(this IOrmModel me) where T : IOrmModel, new() {
            string ProcessName = ReflectionHelper.GetProcessName(me);
            string _message = string.Format("Process {0} failed;", ProcessName);
            string _cacheName = ProcessName;
            ConcurrentQueue<T> retval = new ConcurrentQueue<T>();

            object sync = new object();
            if (!me.CacheStatus(_cacheName)) {
                try {
                    using (SqlConnection cn = new SqlConnection(Connstring())) {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand(me.OrmContext.Read, cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader r = cmd.ExecuteReader();

                        Parallel.ForEach(r.Cast<System.Data.Common.DbDataRecord>(), row => {
                            IOrmModel item = (IOrmModel)new T();
                            item.AutoMapFromDictionary(row.ToDataRow());    
                            lock (sync) {
                                retval.Enqueue((T)item);
                            }
                        });
                        while (r.NextResult()) ;//get Error from SQL... Foobar..
                        cn.Close();
                    }
                    if (retval.Count > 0) {
                        OrmCaching.SetCache(_cacheName, retval.ToList<T>());
                    }
                }
                catch (SqlException ex) {
                    // err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                    throw;
                }
                catch (Exception ex) {
                    // err.WriteError(_message, GetProcessName(me), ex);
                }
                return retval.ToList<T>();
            }
            else {
                return (List<T>)OrmCaching.GetCache(_cacheName);
            }
        }

        public class Parameter {
            public Parameter() { }

            public Parameter(string _name) {
                Name = _name; 
            }
            public Parameter(string _name, object _value) {
                Name = _name;
                Value = _value; 
            }

            public string Name { get; set; }
            public object Value { get; set; }
        }

        //public static List<T> Query<T>(this IOrmModel me, params IEnumerable<dynamic> Constraints) where T : IOrmModel, new() {
        public static List<T> Query<T>(this IOrmModel me, params Parameter[] Constraints) where T : IOrmModel, new() {
            /*Move donot use reflection unless error*/
            string _message = string.Format("Process {0} failed;", ReflectionHelper.GetProcessName(me));
            List<T> retval = new List<T>();
            try {
                using (SqlConnection cn = new SqlConnection(Connstring())) {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(me.OrmContext.Read, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (Parameter Constraint in Constraints) {
                        cmd.Parameters.AddWithValue(Constraint.Name, Constraint.Value);
                    }
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read()) {
                        Dictionary<string, object> row = r.ToDataRow();
                        IOrmModel item = (IOrmModel)new T();
                        item.AutoMapFromDictionary(row); 
                        retval.Add((T)item);
                    }
                    while (r.NextResult()) ;//get Error... Foobar..
                    cn.Close();
                }
            }
            catch (SqlException ex) {
                //err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                throw;
            }
            catch (Exception ex) {
                //err.WriteError(_message, GetProcessName(me), ex);
            }
            return retval;
        }

        public static List<T> Query<T>(this IOrmModel me) where T : IOrmModel, new() {
            /*Move donot use reflection unless error*/
            string _message = string.Format("Process {0} failed;", ReflectionHelper.GetProcessName(me));
            List<T> retval = new List<T>();
            try {
                using (SqlConnection cn = new SqlConnection(Connstring())) {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(me.OrmContext.Read, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (DataAnnotationsModelMetadata field in ModelMetadataProviders.Current.GetMetadataForProperties(me, me.GetType())) {
                        if (field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM", true))) {
                            object PropertyVal = ReflectionHelper.GetProperty(me, field.PropertyName);
                            if ((PropertyVal != null)) {
                                cmd.Parameters.AddWithValue(field.PropertyName, PropertyVal);
                            }
                        }
                    }
                    SqlDataReader r = cmd.ExecuteReader();
                    while (r.Read()) {
                        Dictionary<string, object> row = r.ToDataRow();
                        IOrmModel item = (IOrmModel)new T();
                        item.AutoMapFromDictionary(row); 

                        retval.Add((T)item);
                    }
                    while (r.NextResult()) ;//get Error... Foobar..
                    cn.Close();
                }
            }
            catch (SqlException ex) {
                //err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                throw;
            }
            catch (Exception ex) {
                //err.WriteError(_message, GetProcessName(me), ex);
            }
            return retval;
        }

        public static IOrmModel Create(this IOrmModel me) {
            KeyValuePair<string, int?> _PrimaryKey = ReflectionHelper.PrimaryKey(me);
            /*Move donot use reflection unless error*/
            //string _message = string.Format("Process {0} failed;", ReflectionHelper.GetProcessName(me));
            //_message = string.Format("Process {0} failed; {1}={2}", ReflectionHelper.GetProcessName(me), _PrimaryKey.Key, _PrimaryKey.Value);
            try {
                using (SqlConnection cn = new SqlConnection(Connstring())) {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(me.OrmContext.Create, cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    foreach (DataAnnotationsModelMetadata field in ModelMetadataProviders.Current.GetMetadataForProperties(me, me.GetType())) {
                        if (
                                field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM", true)) 
                                & !field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM_Update", false))
                            ) {
                            if (field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM_PrimaryKey", true))) {
                                cmd.Parameters.Add(new SqlParameter(field.PropertyName, SqlDbType.Int));
                                cmd.Parameters[field.PropertyName].Direction = ParameterDirection.Output;
                            }
                            else {
                                System.Reflection.PropertyInfo PropInfo = me.GetType().GetProperty(field.PropertyName);
                                object PropVal = PropInfo.GetValue(me, null);
                                cmd.Parameters.AddWithValue(field.PropertyName, PropVal);
                            }
                        }
                    }
                    cmd.ExecuteNonQuery();
                    ReflectionHelper.SetProperty(
                        me
                        , _PrimaryKey.Key
                        , cmd.Parameters[_PrimaryKey.Key].Value
                        );
                    cn.Close();
                }
                 OrmCaching.ClearCacheForProcessType(me.GetType().FullName);
            }
            catch (SqlException ex) {
               // err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                throw; 
            }
            catch (Exception ex) {
                // err.WriteError(_message, GetProcessName(me), ex);
            }
            return me;
        }

        public static IOrmModel Update(this IOrmModel me) {
            KeyValuePair<string, int?> _PrimaryKey = ReflectionHelper.PrimaryKey(me);
            /*Move donot use reflection unless error*/
            string _message = string.Format("Process {0} failed;", ReflectionHelper.GetProcessName(me));
            _message = string.Format("Process {0} failed; {1}={2}", ReflectionHelper.GetProcessName(me), _PrimaryKey.Key, _PrimaryKey.Value);
            try {
                using (SqlConnection cn = new SqlConnection(Connstring())) {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(me.OrmContext.Update, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //iterate fields in POCO
                    foreach (System.Web.Mvc.DataAnnotationsModelMetadata field in ModelMetadataProviders.Current.GetMetadataForProperties(me, me.GetType())) {
                        if (field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM", true)) & !field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM_Update", false))) {
                            if (field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM_PrimaryKey", true))) {
                                cmd.Parameters.Add(new SqlParameter(field.PropertyName, SqlDbType.Int));
                                cmd.Parameters[field.PropertyName].Direction = ParameterDirection.InputOutput;
                                cmd.Parameters[field.PropertyName].Value = ReflectionHelper.GetProperty(me, field.PropertyName);
                            }
                            else {
                                cmd.Parameters.AddWithValue(field.PropertyName, ReflectionHelper.GetProperty(me, field.PropertyName));
                            }
                        }
                    }
                    cmd.ExecuteNonQuery();
                    ReflectionHelper.SetProperty(me, _PrimaryKey.Key, cmd.Parameters[_PrimaryKey.Key].Value);
                    cn.Close();
                }
                OrmCaching.ClearCacheForProcessType(me.GetType().FullName);
            }
            catch (SqlException ex) {
                //err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                throw; 
            }
            catch (Exception ex) {
                //err.WriteError(_message, GetProcessName(me), ex);
            }
            return me;
        }

        public static bool Delete(this IOrmModel me) {
            KeyValuePair<string, int?> _PrimaryKey = ReflectionHelper.PrimaryKey(me);
            /*Move donot use reflection unless error*/
            string _message = string.Format("Process {0} failed;", ReflectionHelper.GetProcessName(me));
            _message = string.Format("Process {0} failed; {1}={2}", ReflectionHelper.GetProcessName(me), _PrimaryKey.Key, _PrimaryKey.Value);
            try {
                Delete(me, (int)_PrimaryKey.Value); 
                return true;
            }
            catch (SqlException ex) {
                //err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                throw; 
            }
            catch (Exception ex) {
                //err.WriteError(_message, GetProcessName(me), ex);
            }
            return false;
        }

        public static bool Delete(this IOrmModel me, int PrimaryKeyValue) {
            //Note, this returns true if no row is found matching the key... bubble exception maybe from SQL?
            KeyValuePair<string, int?> _PrimaryKey = ReflectionHelper.PrimaryKey(me);
            /*Move donot use reflection unless error*/
            string _message = string.Format("Process {0} failed;", ReflectionHelper.GetProcessName(me));
            _message = string.Format("Process {0} failed; {1}={2}", ReflectionHelper.GetProcessName(me), _PrimaryKey.Key, _PrimaryKey.Value);
            try {
                using (SqlConnection cn = new SqlConnection(Connstring())) {
                    cn.Open();
                    SqlCommand cmd = new SqlCommand(me.OrmContext.Delete, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(_PrimaryKey.Key, SqlDbType.Int));
                    cmd.Parameters[_PrimaryKey.Key].Direction = ParameterDirection.InputOutput;
                    cmd.Parameters[_PrimaryKey.Key].Value = PrimaryKeyValue;
                    cmd.Parameters.AddWithValue("delete", true);
                    cmd.ExecuteNonQuery();
                    cn.Close();
                }
                OrmCaching.ClearCacheForProcessType(me.GetType().FullName);
                return true;
            }
            catch (SqlException ex) {
                //err.WriteError("SQL Exception: " + _message, GetProcessName(me), ex);
                throw; 
            }
            catch (Exception ex) {
                //err.WriteError(_message, GetProcessName(me), ex);
            }
            return false;
        }

        #endregion
    }


}