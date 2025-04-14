using UnityEngine;
using Firebase.Database;
using System;
using System.Threading.Tasks;

public static class ExpData
{
    public static int[] m_exp_list = new int[3]
    {
        5000, 10000, 15000
    };
}

public class DataManager : Singleton<DataManager>
{
    private DatabaseReference m_database_ref;
    private UserData m_data;
    public UserData Data
    {
        get { return m_data; }
        set { m_data = value; }
    }

    private new void Awake()
    {
        base.Awake();
        
        m_database_ref = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveUserData(UserData user_data)
    {
        var json_data = JsonUtility.ToJson(user_data);

        m_database_ref.Child("users").Child(user_data.m_user_id).SetRawJsonValueAsync(json_data).ContinueWith(
            task => {
                if(task.IsCompleted)
                {
                    Debug.Log("데이터 저장에 성공했습니다.");
                }
                else
                {
                    Debug.LogError("데이터 저장에 실패했습니다.");
                }
        });
    }

    public async void LoadUserData(string user_id)
    {
        await ReadUserData(user_id);
    }

    private async Task ReadUserData(string user_id)
    {
        try
        {
            var result = await m_database_ref.Child("users").Child(user_id).GetValueAsync();

            if(result is not null)
            {
                DataSnapshot snapshot = result;

                if(snapshot.Exists)
                {
                    var json_data = snapshot.GetRawJsonValue();

                    Data = JsonUtility.FromJson<UserData>(json_data);
                }
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
}