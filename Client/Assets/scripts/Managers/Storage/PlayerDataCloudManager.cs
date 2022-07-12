using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
//using MySql.Data.MySqlClient;
using Newtonsoft.Json;

public class PlayerDataCloudManager : MonoBehaviour
{
    //    private string response;
    //    protected string serverAddress = "";

    //    //void Awake() => BatchUpdateToMySqlServer();

    //    void BatchUpdateToMySqlServer()
    //    {
    //        var persons = JsonConvert.DeserializeObject<List<PlayerDataManager>>(response);

    //        DataTable dt = new DataTable();
    //        dt.Columns.Add(new DataColumn("nyangs_pocket", typeof(string)));
    //        // duplicate player data vars here

    //        foreach (var item in persons)
    //        {
    //            DataRow dr = dt.NewRow();
    //            dr["nyangs_pocket"] = item.nyangsPocket;
    //            // duplicate player data vars here
    //            dt.Rows.Add(dr);
    //        }

    //        string connectionString = "server=" + serverAddress + ";user=user;database=db;password=*****;";

    //        MySqlConnection sqlConnection = new MySqlConnection(connectionString);
    //        if (sqlConnection.State == ConnectionState.Open)
    //        {
    //            sqlConnection.Close();
    //        }

    //        sqlConnection.Open();

    //        MySqlCommand cmd = new MySqlCommand("Your Insert Command", sqlConnection);
    //        cmd.CommandType = CommandType.Text;

    //        cmd.UpdatedRowSource = UpdateRowSource.None;

    //        MySqlDataAdapter da = new MySqlDataAdapter();
    //        da.InsertCommand = cmd;
    //        da.UpdateBatchSize = 100000; //If json contains 100000 persons object;
    //        int records = da.Update(dt);
    //        sqlConnection.Close();
    //    }
}
