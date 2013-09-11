using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using App_a_matic.Orm;
//using System.Web.Http.Metadata; 

namespace App_a_matic.Helper {
    static public class ReflectionHelper {

        #region "Reflection Worker Functions"

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <lift>dschrenker lib</lift>
        /// <returns></returns>
        public static string GetProcessName(object callingObject) {
            StackTrace stackTrace = new StackTrace();
            string retval = string.Format("{0}.{1}", callingObject.GetType().ToString(), stackTrace.GetFrame(1).GetMethod().Name);
            return retval;
        }

        public static KeyValuePair<string, int?> PrimaryKey(this IOrmModel me) {
            string _primaryKeyFieldName = string.Empty;
            int? Id = 0;
            try {

                foreach (System.Web.Mvc.ModelMetadata field in System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(me, me.GetType())) {

                    if (field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM_PrimaryKey", true))) {
                        _primaryKeyFieldName = field.PropertyName;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                System.Reflection.PropertyInfo _PrimaryKey = me.GetType().GetProperty(_primaryKeyFieldName);
                Id = (int?)_PrimaryKey.GetValue(me, null);
            }
            catch (Exception ex) {
            }
            return new KeyValuePair<string, int?>(_primaryKeyFieldName, Id);
        }


        public static string PrimaryKeyName(this IOrmModel me) {
            try {
                foreach (System.Web.Mvc.ModelMetadata field in System.Web.Mvc.ModelMetadataProviders.Current.GetMetadataForProperties(me, me.GetType())) {

                    if (field.AdditionalValues.Contains(new KeyValuePair<string, object>("ORM_PrimaryKey", true))) {
                        return field.PropertyName;
                    }
                }
            }
            catch (Exception ex) {
            }
            return string.Empty;
        }

        public static void SetProperty(this IOrmModel me, string Name, object Value) {
            string _message = string.Format("Set Property {0}={1} Dynamic Operation failed;", Name, Value);
            object[] SystemValue = { Value };
            try {
                me.GetType().InvokeMember(Name, System.Reflection.BindingFlags.SetProperty, Type.DefaultBinder, me, SystemValue);
            }
            catch (Exception ex) {
                //err.WriteError(_message, GetProcessName(me), ex);

            }
        }

        public static object GetProperty(this IOrmModel me, string Name) {
            System.Reflection.PropertyInfo PropInfo = me.GetType().GetProperty(Name);
            return PropInfo.GetValue(me, null);
        }

        #endregion

    }
}