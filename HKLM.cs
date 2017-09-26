using System;
using System.Collections.Generic;

using System.Text;

namespace z.Registry
{
   public class HKLM : IDisposable
    {
       public string Key { get; set; }

       private Microsoft.Win32.RegistryKey skms;

       /// <summary>
       /// Specifies new LocalMachine key
       /// eg. @"SOFTWARE\Company Name\Application Name
       /// </summary>
       /// <param name="keyName"></param>
       /// <param name="Writable"></param>
       public HKLM(string keyName, bool Writable = true)
       {
           try
           {
               this.Key = keyName;
               skms = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyName, Writable);

               if (skms == null) //try to create if not exists
               {
                   Microsoft.Win32.Registry.LocalMachine.CreateSubKey(keyName, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);

                   skms = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(keyName, Writable);

                   if (skms == null)
                   {
                       throw new Exception("Cannot Create / Open specified key");
                   }
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void SetValue(string name, object value)
       {
           try
           {
               if (value != null)
               {
                   this.skms.SetValue(name, value);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public object GetValue(string name, object defValue)
       {
           try
           {
               return this.skms.GetValue(name, defValue);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void DeleteValue(string name)
       {
           try
           {
               this.skms.DeleteValue(name, true);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public void DeleteKey()
       {
           try
           {
               Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(skms.Name);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public string[] GetKeys()
       {
          return skms.GetSubKeyNames();
       }

       public string[] GetKeys(Microsoft.Win32.RegistryKey key)
       {
           return key.GetSubKeyNames();
       }

       public Microsoft.Win32.RegistryKey GetSubKey(string KeyName){
           return skms.OpenSubKey(KeyName);
       }

       public Dictionary<string, object> rList()
       {
           try
           {
               Dictionary<string, object> jk = new Dictionary<string, object>();

               foreach (string k in skms.GetValueNames())
               {
                   jk.Add(k, this.GetValue(k, null));
               }

               return jk;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       void IDisposable.Dispose()
       {
           this.skms = null;
           GC.Collect();
           GC.SuppressFinalize(this);
       }

    }
}
