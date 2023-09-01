#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.UI;
using FTOptix.Retentivity;
using FTOptix.NativeUI;
using FTOptix.Core;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using System.Globalization;
#endregion

public class DatabaseLogic : BaseNetLogic
{
    Store myStore;
    Table myTable;
    string[] dbColumns = { "Timestamp", "Value" };

    public override void Start()
    {
        // Insert code to be executed when the user-defined logic is started
        myStore = Project.Current.Get<Store>("DataStores/Database");
        myTable = myStore.Tables.Get<Table>("Demo");
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void Insert(int value) {
        var values = new object[1,2];
        values[0,0] = DateTime.Now;
        values[0,1] = value;
        myTable.Insert(dbColumns, values);
        Log.Info("Insert","Inserted value: " + value.ToString());
    }

    [ExportMethod]
    public void Update(int value, DateTime timestamp) {
        Object[,] ResultSet;
        String[] Header;
        myStore.Query("UPDATE Demo SET Value = " + value + " WHERE Timestamp = \"" + timestamp.ToString("o", CultureInfo.InvariantCulture) + "\"" , out Header, out ResultSet);
        Log.Info("Update", "Updated last record");
    }

    [ExportMethod]
    public void Delete(int value) {
        Object[,] ResultSet;
        String[] Header;
        myStore.Query("DELETE FROM Demo WHERE Value<=65535 ORDER BY Timestamp DESC LIMIT 1", out Header, out ResultSet);
        Log.Info("Delete", "Deleted last record");
    }

    [ExportMethod]
    public void Select(out int value) {
        Object[,] ResultSet;
        String[] Header;
        myStore.Query("SELECT * FROM Demo ORDER BY Timestamp DESC LIMIT 1", out Header, out ResultSet);
        value = Convert.ToInt32(ResultSet[0,1]);
    }
}
