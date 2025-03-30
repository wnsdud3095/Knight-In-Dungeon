using UnityEngine;
using Firebase.Database;

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

    public void LoadUserData(string user_id)
    {
        ReadUserData(user_id);
    }

    private void ReadUserData(string user_id)
    {
        m_database_ref.Child("users").Child(user_id).GetValueAsync().ContinueWith(
            task => {
                if(task.IsFaulted)
                {
                    Debug.Log("데이터를 불러오는데 실패했습니다.");
                }
                else if(task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;

                    if(snapshot.Exists)
                    {
                        var json_data = snapshot.GetRawJsonValue();

                        Data = JsonUtility.FromJson<UserData>(json_data);
                    }
                }
        });
    }
}
