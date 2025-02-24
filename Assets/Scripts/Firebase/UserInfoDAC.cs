using Firebase;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class UserInfoDAC : Singleton<UserInfoDAC>
{
    private DatabaseReference databaseReference;

    private void Awake()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.GetReference("users");

        // databaseReference.ChildChanged += HandleChildChanged;
    }

    public async Task Insert(int id, UserInfo userInfo)
    {
        try
        {
            // 데이터 받기
            var existingData = await databaseReference.Child(id.ToString()).GetValueAsync();

            // 데이터 있는 경우 반환 함수
            if (existingData.Exists)
            {
                Debug.LogWarning($"Key alredy Exists");
                return;
            }

            // 키 값 생성 및 데이터 쓰기
            await databaseReference.Child(id.ToString()).SetValueAsync(userInfo.ToDictionary());
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"Data insert Failed {e.Message}");
        }
    }

    public async Task UpdateInfo(int id, UserInfo userInfo)
    {
        try
        {
            // 데이터 받기
            var existingData = await databaseReference.Child(id.ToString()).GetValueAsync();

            // 데이터 있는 경우 반환 함수
            if (!existingData.Exists)
            {
                Debug.LogWarning($"None Key");
                return;
            }

            // 원본의 정보를 지우고 다시 쓰게 됨.
            // 개별 정보를 변경하고 싶은 경우 따로 작업을 진행해야 함
            await databaseReference.Child(id.ToString()).SetValueAsync(userInfo.ToDictionary());
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"Data Update Failed {e.Message}");
        }
    }

    public async Task DeleteInfo(int id)
    {
        try
        {
            await databaseReference.Child(id.ToString()).RemoveValueAsync();
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"Data Delete Failed {e.Message}");
        }
    }
    
    public async Task Search()
    {
        try
        {
            await databaseReference.OrderByChild("age").EndAt(29).GetValueAsync().ContinueWith(
                task =>
                {
                    if(task.IsCompletedSuccessfully)
                    {
                        var snapshot = task.Result;
                        if(snapshot.Exists)
                        {
                            var list = snapshot.Children.ToList();

                            foreach (var item in snapshot.Children)
                            {
                                Debug.Log($"{item.Key}, {item.Value}");
                            }
                        }
                    }
                }
                
                );
        }
        catch (FirebaseException e)
        {
            Debug.LogError($"Search Failed {e.Message}");
        }
    }

    private void HandleChildChanged(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            return;
        }

        foreach (var item in args.Snapshot.Children)
        {

        }
    }
}
